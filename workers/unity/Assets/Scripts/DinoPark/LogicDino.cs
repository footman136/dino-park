//using System;
using System.Collections.Generic;
using UnityEngine;
using Dinopark.Npc;
using UnityEngine.AI;
using LowPolyAnimalPack;
using System.Collections;

namespace DinoPark
{
    /// <summary>
    /// 恐龙的AI逻辑
    /// </summary>
    public class LogicDino : MonoBehaviour
    {
        [Header("Animation States"), Space(5)]
        [SerializeField]
        private AnimalState[] dinoStates;
        [SerializeField]
        const int stateCount = 6;

        [SerializeField]
        private DinoFSMState.StateEnum _currentState;
        
        [Space(), Header("AI"), Space(5)]
        [SerializeField, Tooltip("This specific dino stats asset, create a new one from the asset menu under (LowPolyAnimals/NewAnimalStats)")]
        AnimalStats ScriptableAnimalStats;

        [HideInInspector]
        public string Species { private set; get; }

        [Space(), Header("Surface Rotation"), Space(5)]
        [SerializeField, Tooltip("If true, this animal will rotate to match the terrain. Ensure you have set the layer of the terrain as 'Terrain'.")]
        private bool matchSurfaceRotation = false;
        [SerializeField, Tooltip("How fast the animnal rotates to match the surface rotation.")]
        private float surfaceRotationSpeed = 2f;

        [Space(), Header("Attributes"), Space(5)]
        [SerializeField] private bool useNavMesh = false;
        [SerializeField, Tooltip("How far away from it's origin this animal will wander by itself.")]
        private float wanderZone = 10f;

        private static List<LogicDino> allAnimals = new List<LogicDino>();
        public static List<LogicDino> AllAnimals { get { return allAnimals; } }
    
        private Animator animator;
        private CharacterController characterController;
        private NavMeshAgent navMeshAgent;
        
        private float originalScent = 0f;
        private float originalToughness = 0f;
        private float originalAgression = 0f;
        private int originalDominance = 0;
        private Vector3 origin;
        
        public bool Attacking { private set; get; }
        public bool Dead { private set; get; }
        public bool Dying { private set; get; }
        private Vector3 targetLocation = Vector3.zero;
        private float currentTurnSpeed = 0f;
        [SerializeField]
        private float _currentFood = 0f; // 当前的粮食
        [SerializeField]
        private float _currentToughness = 0; // 当前生命值

        public enum AI_STATUS
        {
          AI_IDLE = 0,
          AI_WANDERING = 1,
          AI_LOOKFOR_FOOD = 2,
          AI_CHASING = 3,
          AI_ESCAPING = 4,
          AI_DEAD = 5,
        };

        [SerializeField]
        private AI_STATUS _aiStatus;
        
        
        void Awake()
        {
            animator = GetComponent<Animator>();
            animator.applyRootMotion = false;
            characterController = GetComponent<CharacterController>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            // 初始化数值
            Born();
            
            // 初始化所有动画状态
            dinoStates = new AnimalState[stateCount];
            var stateNames = new string[stateCount] {"Idle", "Eat", "Walk", "Run", "Attack", "Death"};
            var animationBools = new string[stateCount] {"IsIdling", "IsEating", "IsWalking", "IsRunning", "IsAttacking", "IsDead"};
            for (int i = 0; i < stateCount; ++i)
            {
                dinoStates[i] = new AnimalState {stateName = stateNames[i], animationBool = animationBools[i]};
            }

            if (navMeshAgent)
            {
                useNavMesh = true;
                navMeshAgent.stoppingDistance = ScriptableAnimalStats.contingencyDistance;
            }
            else
            {
                useNavMesh = false;
            }

            if (matchSurfaceRotation && transform.childCount > 0)
            {
                transform.GetChild(0).gameObject.AddComponent<SurfaceRotation>().SetRotationSpeed(surfaceRotationSpeed);
            }

            allAnimals.Add(this);
        }
        
        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("AI_running", Random.Range(0,1), ScriptableAnimalStats.thinkingFrequency);
        }

        void Update()
        {
            if (AnimalManager.Instance.PeaceTime)
            {
                SetPeaceTime(true);
            }
        }
    
        private void AI_running()
        {
            float deltaTime = Time.deltaTime;
            
            // No.1 消耗粮食
            float cost = ScriptableAnimalStats.liveCost * deltaTime;
            if(_currentFood >= cost)
                _currentFood -= cost;
            else
            {
                _currentFood = 0;
                _currentToughness -= cost - _currentFood;
            }
            
            // No.2 判断是否该死
            if (_currentToughness <= 0)
            {
                Die();
            }
            
            // No.3 判断粮食还有存货，则补充生命值，立即补满
            if (_currentFood > 0 && _currentToughness < ScriptableAnimalStats.toughness)
            {
                float needToughness = ScriptableAnimalStats.toughness - _currentToughness;
                if (_currentFood > needToughness)
                {
                    _currentToughness = ScriptableAnimalStats.toughness;
                    _currentFood -= needToughness;
                }
                else
                {
                    _currentToughness += needToughness;
                    _currentFood = 0;
                }
            }
            
            // No.4 躲避天敌的逻辑
            if (LookforPredator())
                return;
            
            // No.5 寻找猎物的逻辑
            if (LookforPrey())
                return;
            
            // No.6 寻找植物性食物的逻辑
            if (LookforFood())
                return;

            // No.7 随机漫游的逻辑
            Wandering();
            
        }
        public void SetPeaceTime(bool peace)
        {
            if (peace)
            {
                ScriptableAnimalStats.dominance = 0;
                ScriptableAnimalStats.scent = 0f;
                ScriptableAnimalStats.agression = 0f;
            }
            else
            {
                ScriptableAnimalStats.dominance = originalDominance;
                ScriptableAnimalStats.scent = originalScent;
                ScriptableAnimalStats.agression = originalAgression;
            }
        }

        public void Born()
        {
            Species = ScriptableAnimalStats.species;// 恐龙的种类，名称

            origin = transform.position;
            originalDominance = ScriptableAnimalStats.dominance;
            originalScent = ScriptableAnimalStats.scent;
            originalAgression = ScriptableAnimalStats.agression;
            originalToughness = ScriptableAnimalStats.toughness;
            _currentFood = ScriptableAnimalStats.foodStorage;
            _currentToughness = ScriptableAnimalStats.toughness;
            _currentState = DinoFSMState.StateEnum.NONE;
        }

        public void Die()
        {
            StopAllCoroutines();
            Dead = true;
            PlayAnimation(DinoFSMState.StateEnum.DEAD);
            _currentFood = 0;
            _currentToughness = 0;
            _aiStatus = AI_STATUS.AI_DEAD;
        }

        private void PlayAnimation(DinoFSMState.StateEnum inState)
        {
            foreach (var state in dinoStates)
            {
                animator.SetBool(state.animationBool, false);
            }
            _currentState = inState;
            animator.SetBool(dinoStates[(int)_currentState].animationBool, true);
        }

        private void TurnAround(Vector3 destination, bool forward)
        {
            Quaternion startRotation = transform.rotation;
            if(forward)
            {// 朝向目的地方向
                transform.rotation = Quaternion.LookRotation(destination - transform.position);
            }
            else
            {// 与目的地方向相反
                transform.rotation = Quaternion.LookRotation(transform.position - destination);
            }
        }
        /// <summary>
        ///  获取与给定的目的地相反方向，一定距离的点的坐标
        /// </summary>
        /// <param name="destination">给定的目的地</param>
        /// <param name="range">距离</param>
        /// <returns></returns>
        private Vector3 RandonPointAwayFromDestination(Vector3 destination, float range)
        {
            Vector3 randomPoint = transform.position + (transform.position - destination) * range;
            return new Vector3(randomPoint.x, transform.position.y, randomPoint.z);
        }
        

        #region AI逻辑

        private bool LookforPredator()
        {
            if (ScriptableAnimalStats.awareness > 0)
            {
                float distMin = float.MaxValue;
                int index = -1;
                for (int i = 0; i < allAnimals.Count; i++)
                {
                    if (allAnimals[i].Dead == true || allAnimals[i] == this || allAnimals[i].Species == Species ||
                        allAnimals[i].ScriptableAnimalStats.dominance <= ScriptableAnimalStats.dominance ||
                        allAnimals[i].ScriptableAnimalStats.stealthy)
                    {
                        continue;
                    }

                    float dist = Vector3.Distance(transform.position, allAnimals[i].transform.position);
                    if (dist > ScriptableAnimalStats.awareness)
                    {
                        continue;
                    }

                    if (dist < distMin)
                    {
                        distMin = dist;
                        index = i;
                    }
                }

                if (index >= 0 && index < allAnimals.Count)
                {
                    var predator = allAnimals[index];
                    if (useNavMesh)
                    {
                        var target = RandonPointAwayFromDestination(predator.transform.position, 5f);
                        StopCoroutine("RunAwayState");
                        StartCoroutine(RunAwayState(target));

                    }
                    else
                    {
                        targetLocation = RandonPointAwayFromDestination(predator.transform.position, 5f);
                        StopCoroutine("NonNavMeshRunAwayState");
                        StartCoroutine(NonNavMeshRunAwayState(targetLocation));
                    }
                    // 进入“躲避天敌”的状态
                    return true;
                }

            }

            StopCoroutine("RunAwayState");
            StopCoroutine("NonNavMeshRunAwayState");
            // 感知距离为0，没有感知力，所以永远不会进入“躲避天敌”的状态
            return false;
        }

        private IEnumerator RunAwayState(Vector3 target)
        {
            navMeshAgent.speed = ScriptableAnimalStats.runSpeed;
            navMeshAgent.angularSpeed = ScriptableAnimalStats.turnSpeed;
            navMeshAgent.SetDestination(target);
            PlayAnimation(DinoFSMState.StateEnum.RUN);
            _aiStatus = AI_STATUS.AI_ESCAPING;

            float timeMoving = 0f;
            while ((navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance || timeMoving < 0.1f) && timeMoving < ScriptableAnimalStats.stamina)
            {
                timeMoving += Time.deltaTime;
                yield return null;
            }
            PlayAnimation(DinoFSMState.StateEnum.IDLE);
            navMeshAgent.speed = ScriptableAnimalStats.moveSpeed;
            navMeshAgent.angularSpeed = ScriptableAnimalStats.turnSpeed;
            _aiStatus = AI_STATUS.AI_IDLE;
        }
        private IEnumerator NonNavMeshRunAwayState(Vector3 target)
        {
            currentTurnSpeed = ScriptableAnimalStats.turnSpeed;
            PlayAnimation(DinoFSMState.StateEnum.RUN);
            _aiStatus = AI_STATUS.AI_ESCAPING;

            float walkTime = 0f;
            float timeUntilAbortWalk = Vector3.Distance(transform.position, target) / ScriptableAnimalStats.runSpeed;

            while (Vector3.Distance(transform.position, target) > ScriptableAnimalStats.contingencyDistance && walkTime < timeUntilAbortWalk && ScriptableAnimalStats.stamina > 0)
            {
                characterController.SimpleMove(transform.TransformDirection(Vector3.forward) * ScriptableAnimalStats.runSpeed);

                Vector3 relativePos = target - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (currentTurnSpeed / 10));
                currentTurnSpeed += Time.deltaTime;

                walkTime += Time.deltaTime;
                ScriptableAnimalStats.stamina -= Time.deltaTime;
                yield return null;
            }

            targetLocation = Vector3.zero;
            PlayAnimation(DinoFSMState.StateEnum.IDLE);
            currentTurnSpeed = ScriptableAnimalStats.turnSpeed;
            _aiStatus = AI_STATUS.AI_IDLE;
        }

        private bool LookforPrey()
        {
            return false;
        }

        private bool LookforFood()
        {
            return false;
        }

        private void Wandering()
        {
            
        }
        
        #endregion
    }
}