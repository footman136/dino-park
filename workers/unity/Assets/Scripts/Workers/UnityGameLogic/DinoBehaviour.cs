using System.Collections;
using System.Collections.Generic;
using Assets.Gamelogic.Utils;
using UnityEngine;
using UnityEngine.AI;
using Dinopark.Npc;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Assets.Gamelogic.Core;
using Assets.Gamelogic.Tree;
using Dinopark.Fire;
using Dinopark.Plants;

/// <summary>
/// 恐龙AI逻辑框架，用于挂接在UntiyGameLogic（服务器）端
/// </summary>
[WorkerType(WorkerUtils.UnityGameLogic)]
public class DinoBehaviour : MonoBehaviour
{
    [Require] public DinoAiDataWriter aiDataWriter;
    [Require] public DinoAttrsWriter attrsWriter;
    [Require] public EntityId _entityId;
    
    public DinoStateMachine stateMachine;
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
    public string Species { private set; get; }
    public bool Dead(){return _isDead;}
    
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
        Species = ScriptableAnimalStats.species;
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

        var update2 = new DinoAiData.Update
        {
            CurrentAiState = _currentAiState,
            TargetEntityId = new EntityId(),
            TargetPosition = DinoStateMachine.InvalidPosition.ToVector3f()
        };
        aiDataWriter.SendUpdate(update2);
    }

    public void Die()
    {
        var update = new DinoAttrs.Update
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
    /// <summary>
    ///  获取与给定的目的地相反方向，一定距离的点的坐标
    /// </summary>
    /// <param name="destination">给定的目的地</param>
    /// <param name="range">距离</param>
    /// <returns></returns>
    private Vector3 RandomPointAwayFromDestination(Vector3 destination, float range)
    {
        Vector3 randomPoint = transform.position + (transform.position - destination).normalized * range;
        return new Vector3(randomPoint.x, transform.position.y, randomPoint.z);
    }

    private bool IfWithinMap(Vector3 pos)
    {
        if (pos.x >= 60 || pos.x <= -60 || pos.z >= 60 || pos.z <= -60)
            return false;
        return true;
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

        _currentAiState = stateMachine.CurrentState;

        if (bFirst)
        {
            ReBorn();
            bFirst = false;
        }

        // No.1 判断是否该死了
        if (IfShouldDie())
            return;
        // No.2. 是否正在战斗中
        if (IsFighting())
            return;

        // No.6 躲避天敌
        if (LookforPredator())
            return;
        
        // No.7 吃饭的时候，不再寻找其他的猎物
        if (IsEating())
            return;
        
        // No.8 寻找猎物
        if (LookforPrey())
            return;
        // No.9 寻找植物性食物
        if (LookforFood())
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

    private bool IsFighting()
    {
        if (_currentAiState == DinoAiFSMState.StateEnum.ATTACK || _currentAiState == DinoAiFSMState.StateEnum.BE_ATTACK)
            return true;
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

    private bool LookforPredator()
    {
        if (ScriptableAnimalStats.awareness <= 0) // 没有感知能力，不进入此逻辑
            return false;
        if (_currentAiState == DinoAiFSMState.StateEnum.RUN_AWAY) // 如果已经处于本状态，则继续
            return true;
           
        float distMin = float.MaxValue;
        DinoBehaviour predator = null;
        foreach(var iter in allAnimals)
        {
            var animal = iter.Value;
            if (animal._isDead || animal == this || animal.Species == Species ||
                animal.ScriptableAnimalStats.dominance <= ScriptableAnimalStats.dominance ||
                animal.ScriptableAnimalStats.stealthy)
            {
                continue;
            }

            float dist = Vector3.Distance(transform.position, animal.transform.position);
            if (dist > ScriptableAnimalStats.awareness)
            {
                continue;
            }

            if (dist < distMin)
            {
                distMin = dist;
                predator = animal;
            }
        }

        if (predator != null)
        {
            if (logChanges)
            {
                Debug.Log(string.Format("{0}: Found predator ({1}), running away.", gameObject.name,
                    predator.gameObject.name));
            }

            var targetPosition = RandomPointAwayFromDestination(predator.transform.position, 30f);
            if (!IfWithinMap(targetPosition))
            { // 目标点无法走到，就随机一个点
                targetPosition = RandomPoint(predator.transform.position, 30f);
            }
            // 这里不能给天敌的ID，因为WanderingState逻辑里，有对目标物体距离的判定，如果太近，就意味着到达了，状态会结束。
            stateMachine.TriggerTransition(DinoAiFSMState.StateEnum.RUN_AWAY, new EntityId(), targetPosition);
            return true;
        }

        return false;
    }

    private bool IsEating()
    {
        if (_currentAiState == DinoAiFSMState.StateEnum.EAT || _currentAiState == DinoAiFSMState.StateEnum.EAT_FOOD) // 吃饭的时候，不寻找其他的猎物
            return true;
        return false;
    }

    private bool LookforPrey()
    {
        if (ScriptableAnimalStats.dominance <= 0) // 处于食物链的低端，不会寻找猎物
            return false;
        if (_currentAiState == DinoAiFSMState.StateEnum.CHASE) // 已经处于本状态，继续
            return true;
        
        float distMin = float.MaxValue;
        DinoBehaviour prey = null;
        foreach(var iter in allAnimals)
        {
            var animal = iter.Value;
            if (animal._isDead == true || animal == this ||
                (animal.Species == Species && !ScriptableAnimalStats.territorial) ||
                animal.ScriptableAnimalStats.dominance > ScriptableAnimalStats.dominance ||
                animal.ScriptableAnimalStats.stealthy)
            {
                continue;
            }

            int p = System.Array.IndexOf(ScriptableAnimalStats.nonAgressiveTowards, animal.Species);
            if (p > -1)
            {
                continue;
            }

            float dist = Vector3.Distance(transform.position, animal.transform.position);
            if (dist > ScriptableAnimalStats.scent)
            {
                continue;
            }

            if (Random.Range(0, 100) > ScriptableAnimalStats.agression)
            {
                continue;
            }

            if (dist < distMin)
            {
                distMin = dist;
                prey = animal;
            }
        }

        if (prey != null)
        {
            if (logChanges)
            {
                Debug.Log(string.Format("{0}: Found prey ({1}), chasing.", gameObject.name, prey.gameObject.name));
            }

            stateMachine.TriggerTransition(DinoAiFSMState.StateEnum.CHASE, prey._entityId, prey.transform.position);
            return true;
        }
        return false;
    }

    private bool LookforFood()
    {
        if (ScriptableAnimalStats.vegetarian == false) // 素食主义者才进入本逻辑
            return false;
        if (_currentAiState == DinoAiFSMState.StateEnum.LOOK_FOR_FOOD)
            return true;
        _currentFood = attrsWriter.Data.CurrentFood;
        if (_currentFood / ScriptableAnimalStats.foodStorage < ScriptableAnimalStats.HungryRate)
        {
            float distMin = float.MaxValue;
            TreeBehaviour aTree = null;
            foreach (var iter in TreeBehaviour.AllTrees)
            {
                var tree = iter.Value; 
                float dist = Vector3.Distance(transform.position, tree.transform.position);
                if (dist > ScriptableAnimalStats.scent)
                {
                    continue;
                }

                if (!tree.IsHavestable())
                {
                    continue;
                }

                if (dist < distMin)
                {
                    distMin = dist;
                    aTree = tree;
                }
            }

            if (aTree != null)
            {
                var targetPosition = aTree.transform.position;
                stateMachine.TriggerTransition(DinoAiFSMState.StateEnum.LOOK_FOR_FOOD, aTree._entityId, targetPosition);
                return true;
            }
        }
        return false;
    }
#endregion
    
#region AI - 逻辑 - 第二层
    public void TakeDamage(float damage)
    {
        _currentToughness = attrsWriter.Data.CurrentToughness;
        _currentToughness -= damage;
        if (logChanges)
        {
            Debug.Log(string.Format("{0}: Taking Damage {1} HP {2}!", gameObject.name, damage, _currentToughness));
        }
        
        var update = new DinoAttrs.Update
        {
            CurrentToughness = _currentToughness
        };
        attrsWriter.SendUpdate(update);

        // 如果血不够了，立即死亡。不用等到下一个AI周期了。
        if (attrsWriter.Data.CurrentToughness <= 0)
            Die();
    }

    public void EatFood(TreeBehaviour aTree)
    {
        // resNeed：每次想吃appetite数量的粮食，如果快饱了，则仅吃饱为止，不多吃
        var hungry = ScriptableAnimalStats.foodStorage - attrsWriter.Data.CurrentFood;
        var resNeed = ScriptableAnimalStats.appetite;
        if (hungry < ScriptableAnimalStats.appetite)
            resNeed = hungry;
        var request = new HarvestRequest()
        {
            Harvester = _entityId,
            ResourcesNeed = resNeed
        };
        aTree.Harvest(request, OnEatFood);
    }
    public void OnEatFood(Harvestable.Harvest.ReceivedResponse response)
    {
        // resTaken：实际吃到的粮食，如果树上粮食已经不足了，则剩余多少都给过来，所以不一定是appetite，也不一定是resNeed
        HarvestResponse? payload = response.ResponsePayload;
        float resTaken = 0f;
        if (payload.HasValue)
            resTaken = payload.Value.ResourcesTaken;
        //Debug.Log("Command<HarvestRequest> sent! Harvester<"+response.SendingEntity.Index+"> Tree<"+response.EntityId+"> Resource Taken<"+resTaken+"> StatusCode<"+response.StatusCode+"> Message<"+response.Message+">");
        _currentFood = attrsWriter.Data.CurrentFood;
        var update = new DinoAttrs.Update
        {
            CurrentFood = _currentFood + resTaken
        };
        attrsWriter.SendUpdate(update);
    }
    
#endregion
}
