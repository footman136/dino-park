﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Dinopark.Npc;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using LowPolyAnimalPack;
using Assets.Gamelogic.Core;
using Dinopark.Life;

[WorkerType(WorkerUtils.UnityClient)]
public class DinoVisualizer : MonoBehaviour
{
    [Require] private DinoAiDataReader aiDataReader;
    [Require] public  DinoAttrsReader attrsReader;
    [Require] private HealthReader health;
    [Require] private AgeReader age;
    [Require] private EntityId _entityId;
    
    private Animator animator;
    private const int stateCount = 6; 
    private NavMeshAgent navMeshAgent;
    private static Dictionary<long, DinoVisualizer> allAnimals = new Dictionary<long, DinoVisualizer>();
    public static Dictionary<long, DinoVisualizer> AllAnimals { get { return allAnimals; } }
    
    [Header("Animation States"), Space(5)]
    [SerializeField]
    private AnimalState[] dinoStates;

    [Space(), Header("AI"), Space(5)]
    [SerializeField, Tooltip("This specific dino stats asset, create a new one from the asset menu under (LowPolyAnimals/NewAnimalStats)")]
    AnimalStats ScriptableAnimalStats;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float distance;

    [Space(), Header("Attributes"), Space(5)]
    [SerializeField] private long _id;
    [SerializeField] private DinoAiFSMState.StateEnum _currentAiState; // AI的状态
    [SerializeField] private string _aniStateName;// 当前正在播放的动画的名字
    
    [SerializeField] private bool _isDead;
    [SerializeField] private float _curFood; // 当前的粮食
    [SerializeField] private float _maxFood; // 最大的粮食储量
    [SerializeField] private float _curHealth; // 当前生命值
    [SerializeField] private float _maxHealth; // 最大生命值
    [SerializeField] private int _age; // 年龄

    private float originalScent = 0f;
    private float originalAgression = 0f;
    private int originalDominance = 0;
    private Vector3 origin;

    private Vector3 _originScale;
    
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.applyRootMotion = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = ScriptableAnimalStats.contingencyDistance;
       
        // 初始化所有动画状态
        dinoStates = new AnimalState[stateCount];
        var stateNames = new string[stateCount] {"Idle", "Walk", "Eat", "Run", "Attack", "Death"};
        var animationBools = new string[stateCount] {"isIdling", "isWalking", "isEating", "isRunning", "isAttacking", "isDead"};
        for (int i = 0; i < stateCount; ++i)
        {
            dinoStates[i] = new AnimalState {stateName = stateNames[i], animationBool = animationBools[i]};
        }

        _originScale = transform.localScale;
    }
    // Start is called before the first frame update
    void Start()
    {
        allAnimals.Add(_entityId.Id, this);
        _id = _entityId.Id;
        var update = new DinoAiData.Update
        {
            CurrentAiState = aiDataReader.Data.CurrentAiState
        };
        OnAiDataChanged(update);

        var update2 = new Age.Update
        {
            Age = age.Data.Age
        };
        OnAgeChanged(update2);

        var update3 = new DinoAttrs.Update
        {
        };
        OnAttrsChanged(update3);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, targetPosition);
    }

    void OnEnable()
    {
        aiDataReader.OnUpdate += OnAiDataChanged;
        attrsReader.OnUpdate += OnAttrsChanged;
        age.OnUpdate += OnAgeChanged;
    }

    void OnDisable()
    {
        aiDataReader.OnUpdate -= OnAiDataChanged;
        attrsReader.OnUpdate -= OnAttrsChanged;
        age.OnUpdate -= OnAgeChanged;
    }

    void OnAiDataChanged(DinoAiData.Update update)
    {
        if (update.CurrentAiState.HasValue)
        {
            _currentAiState = update.CurrentAiState.Value;
        }

        targetPosition = transform.position;
        if (update.TargetPosition.HasValue)
        {
            if (update.TargetPosition.Value.ToUnityVector() != DinoStateMachine.InvalidPosition)
            {
                targetPosition = update.TargetPosition.Value.ToUnityVector();
            }
        }
        
        int aniState = 0;
        switch (_currentAiState)
        {
            case DinoAiFSMState.StateEnum.IDLE:
            {
                Vector3 newpos = transform.position;
                newpos.y = 0;
                transform.position = newpos;
                gameObject.SetActive(true);
            }
                aniState = 0;
                break;
            case DinoAiFSMState.StateEnum.WANDER:
                targetPosition = targetPosition;
                navMeshAgent.speed = ScriptableAnimalStats.moveSpeed;
                aniState = 1;
                break;
            case DinoAiFSMState.StateEnum.EAT:
                aniState = 2;
                break;
            case DinoAiFSMState.StateEnum.RUN_AWAY:
                targetPosition = targetPosition;
                navMeshAgent.speed = ScriptableAnimalStats.runSpeed;
                aniState = 3;
                break;
            case DinoAiFSMState.StateEnum.CHASE:
                targetPosition = targetPosition;
                navMeshAgent.speed = ScriptableAnimalStats.runSpeed;
                aniState = 3;
                break;
            case DinoAiFSMState.StateEnum.ATTACK:
                aniState = 4;
                StartCoroutine(TurnToLookAtTarget(targetPosition));
                break;
            case DinoAiFSMState.StateEnum.DEAD:
                aniState = 5;
                break;
            case DinoAiFSMState.StateEnum.HATCH:
                aniState = 4;
                break;
            case DinoAiFSMState.StateEnum.LOOK_FOR_FOOD:
                targetPosition = targetPosition;
                navMeshAgent.speed = ScriptableAnimalStats.moveSpeed;
                aniState = 1;
                break;
            case DinoAiFSMState.StateEnum.EAT_FOOD:
                aniState = 2;
                break;
            case DinoAiFSMState.StateEnum.VANISH:
                aniState = 5;
                break;
        }

        navMeshAgent.SetDestination(targetPosition);
        PlayAnimation(aniState);

        if (_currentAiState == DinoAiFSMState.StateEnum.VANISH)
        {
            StartCoroutine(Vanishing());
        }
    }

    void OnAttrsChanged(DinoAttrs.Update update)
    {
        _isDead = attrsReader.Data.IsDead;
        _curFood = attrsReader.Data.CurrentFood; // 当前的粮食
        _maxFood = attrsReader.Data.MaxFood; // 最大的粮食储量
        _curHealth = health.Data.CurrentHealth; // 当前生命值
        _maxHealth = health.Data.MaxHealth; // 最大生命值
    
        originalScent = attrsReader.Data.OriginalScent;
        originalAgression = attrsReader.Data.OriginalAgression;
        originalDominance = attrsReader.Data.OriginalDominance;
        origin = attrsReader.Data.OriginPosition.ToUnityVector();
    }

    void OnAgeChanged(Age.Update update)
    {
        _age = Mathf.FloorToInt(age.Data.Age);
        if (update.Age.HasValue)
            _age = Mathf.FloorToInt(update.Age.Value);
        float growUpAge = age.Data.GrowUpAge;
        if (update.GrowUpAge.HasValue)
            growUpAge = update.GrowUpAge.Value;
        if (_age >= growUpAge)
        {
            transform.localScale = _originScale;
            return;
        }

        transform.localScale = _originScale * (SimulationSettings.NPCChildhoodMinWeekness +
                                               (_age / growUpAge) *
                                               (1 - SimulationSettings.NPCChildhoodMinWeekness));
    }

    private void PlayAnimation(int inState)
    {
        foreach (var state in dinoStates)
        {
            animator.SetBool(state.animationBool, false);
        }
        var stateNames = new string[stateCount] {"Idle", "Walk", "Eat", "Run", "Attack", "Death"};
        _aniStateName = stateNames[inState]; 
        animator.SetBool(dinoStates[inState].animationBool, true);
    }

    private IEnumerator TurnToLookAtTarget(Vector3 targetPosition)
    {
        while (true)
        {
            Vector3 direction = targetPosition - transform.position;

            if (Vector3.Angle(direction, transform.forward) < 1f)
            {
                yield break;
            }

            float step = 2f * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            yield return null;
        }
    }

    private IEnumerator Vanishing()
    {
        navMeshAgent.enabled = false;
        while (true)
        {
            Vector3 newpos = transform.position;
            newpos.y -= 0.1f;
            transform.position = newpos;
            if (newpos.y <= -3f)
            {
                gameObject.SetActive(false);
                //Destroy(this);
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
