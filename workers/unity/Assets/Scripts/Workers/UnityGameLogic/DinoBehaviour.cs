using System.Collections;
using System.Collections.Generic;
using Assets.Gamelogic.Utils;
using UnityEngine;
using UnityEngine.AI;
using Dinopark.Npc;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Assets.Gamelogic.Core;

/// <summary>
/// 恐龙AI逻辑框架，用于挂接在UntiyGameLogic（服务器）端
/// </summary>
public class DinoBehaviour : MonoBehaviour
{
    [Require] public  DinoAiDataWriter aiDataWriter;
    [Require] public  DinoAttrsWriter attrsWriter;
    [Require] private EntityId _entityId;
    
    private DinoStateMachine stateMachine;
    public NavMeshAgent navMeshAgent;
    private static Dictionary<long, DinoBehaviour> allAnimals = new Dictionary<long, DinoBehaviour>();
    public static Dictionary<long, DinoBehaviour> AllAnimals { get { return allAnimals; } }
    
    [Space(), Header("AI"), Space(5)]
    [SerializeField, Tooltip("This specific dino stats asset, create a new one from the asset menu under (LowPolyAnimals/NewAnimalStats)")]
    public AnimalStats ScriptableAnimalStats;
    [SerializeField] private DinoAiFSMState.StateEnum _currentAiState; // AI的状态
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float distance;

    [Space(), Header("Attributes"), Space(5)]
    [SerializeField] private long _id;
    [SerializeField] private int _stateCount;
    [SerializeField] private float _deltaTime;
    [SerializeField] private bool _isDead;
    [SerializeField] private float _currentFood; // 当前的粮食
    [SerializeField] private float _currentToughness; // 当前生命值
    
    [Space(), Header("Debug"), Space(5)]
    [SerializeField, Tooltip("If true, AI changes to this animal will be logged in the console.")]
    public bool logChanges = false;
    
#region 主函数
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = ScriptableAnimalStats.contingencyDistance;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        allAnimals.Add(_entityId.Id, this);
        _id = _entityId.Id;
        InvokeRepeating("AI_Running", 3, ScriptableAnimalStats.thinkingFrequency);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Tick();
        _currentAiState = stateMachine.CurrentState;
        _currentFood = attrsWriter.Data.CurrentFood;
        _currentToughness = attrsWriter.Data.CurrentToughness;
        distance = Vector3.Distance(transform.position, targetPosition);
    }
    
    private void OnEnable()
    {
        stateMachine = new DinoStateMachine(this);
        stateMachine.OnEnable(aiDataWriter.Data.CurrentAiState);
        _stateCount = stateMachine.StateCount();
    }

    private void OnDisable()
    {
        stateMachine.OnDisable();
    }
#endregion

#region 工具函数

    public void ReBorn()
    {
        var update = new DinoAttrs.Update
        {
            IsDead = false,
            CurrentFood = ScriptableAnimalStats.foodStorage,
            CurrentToughness = ScriptableAnimalStats.toughness,
            OriginalAgression = ScriptableAnimalStats.agression,
            OriginalDominance = ScriptableAnimalStats.dominance,
            OriginalScent = ScriptableAnimalStats.scent,
            OriginPosotion = transform.position.ToVector3f()
        };
        attrsWriter.SendUpdate(update);

//        _currentAiState = aiDataWriter.Data.CurrentAiState;
//        if (_currentAiState == DinoAiFSMState.StateEnum.DEAD)
//            Dead = true;
        var oldState = aiDataWriter.Data.CurrentAiState;
        _currentAiState = DinoAiFSMState.StateEnum.IDLE;
        if(oldState != _currentAiState)
            stateMachine.TransitionTo(_currentAiState);

        var update2 = new DinoAiData.Update()
        {
            CurrentAiState = _currentAiState,
            TargetEntityId = new EntityId(),
            TargetPosition = DinoStateMachine.InvalidPosition.ToVector3f()
        };
        aiDataWriter.SendUpdate(update2);
    }

    public void Die()
    {
        var update = new DinoAttrs.Update()
        {
            IsDead = true,
            CurrentFood = ScriptableAnimalStats.foodStorage,
            CurrentToughness = ScriptableAnimalStats.toughness,
            OriginalAgression = ScriptableAnimalStats.agression,
            OriginalDominance = ScriptableAnimalStats.dominance,
            OriginalScent = ScriptableAnimalStats.scent,
            OriginPosotion = transform.position.ToVector3f()
        };
        attrsWriter.SendUpdate(update);
        stateMachine.TriggerTransition(DinoAiFSMState.StateEnum.DEAD, new EntityId(), DinoStateMachine.InvalidPosition);
        _isDead = true;
    }
    private Vector3 RandomPoint(Vector3 destination, float range)
    {
        Vector3 randomPoint = destination + Random.insideUnitSphere* range;
        return new Vector3(randomPoint.x, destination.y, randomPoint.z);
    }
#endregion
    
#region AI 函数 - 第一层

    private bool bFirst = true;
    private float _lastTime = 0f;
    private void AI_Running()
    {
        // 这里的_deltaTime是真实的每次本函数调用的时间间隔（而不是Time.deltaTime）。
        //_deltaTime = Time.deltaTime;
        var nowTime = Time.fixedTime;
        _deltaTime = nowTime - _lastTime;
        _lastTime = nowTime;

        if (bFirst)
        {
            ReBorn();
            bFirst = false;
        }

        // No.1 判断是否该死了
        if (IfShouldDie())
            return;
        
        // No.10 进入随便溜达（WANDER）的状态
        Wandering();
    }

    private bool IfShouldDie()
    {
        // No.1 如果已经死了，就啥都不干了
        if (_isDead)
            return true;
        
        // No.2 消耗粮食
        float cost = ScriptableAnimalStats.liveCost * _deltaTime;
        float currentFood = attrsWriter.Data.CurrentFood;
        float currentToughness = attrsWriter.Data.CurrentToughness; 
        if( currentFood >= cost)
            currentFood -= cost;
        else
        {
            currentFood = 0;
            currentToughness -= cost - currentFood;
        }
            
        var update = new DinoAttrs.Update()
        {
            CurrentFood = currentFood,
            CurrentToughness = currentToughness
        };
        attrsWriter.SendUpdate(update);

        // No.3 判断是否该死
        if (currentToughness <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    private void Wandering()
    {
        if (_currentAiState != DinoAiFSMState.StateEnum.WANDER)
        {
            int ran = Random.Range(0, 3);
            if (ran < 1) // 每秒都有一半的概率，原地不动，休息。
            {
                targetPosition = RandomPoint(transform.position, 30f);
                stateMachine.TriggerTransition(DinoAiFSMState.StateEnum.WANDER, new EntityId(), targetPosition); 
            }
        }
    }
        
#endregion
}
