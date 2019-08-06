using System.Collections.Generic;
using UnityEngine;
using Dinopark.Npc;
using UnityEngine.AI;
using LowPolyAnimalPack;
using System.Collections;
using UnityEditor;
using Random = UnityEngine.Random;

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

        private static Dictionary<int, LogicDino> allAnimals = new Dictionary<int, LogicDino>();
        public static Dictionary<int, LogicDino> AllAnimals { get { return allAnimals; } }
        private static int _animalKey = 0;
    
        private Animator animator;
        private CharacterController characterController;
        private NavMeshAgent navMeshAgent;
        
        private float originalScent = 0f;
        private float originalToughness = 0f;
        private float originalAgression = 0f;
        private int originalDominance = 0;
        private Vector3 origin;
        [SerializeField]
        private float _deltaTime;
        
        public bool IsAttacking()
        {
            return _aiStatus == AI_STATUS.AI_ATTACKING;
            
        }
        public bool Dead { private set; get; }
        //public bool Dying { private set; get; }
        private Vector3 targetLocation = Vector3.zero;
        private float currentTurnSpeed = 0f;
        [SerializeField]
        private float _currentFood = 0f; // 当前的粮食
        [SerializeField]
        private float _currentToughness = 0; // 当前生命值

        public enum AI_STATUS
        {
          AI_IDLE = 0,
          AI_EATING = 1,
          AI_WANDERING = 2,
          AI_LOOKFOR_FOOD = 3,
          AI_CHASING = 4,
          AI_ESCAPING = 5,
          AI_ATTACKING = 6,
          AI_BEINGATTACKED = 7,
          AI_DEAD = 8,
        };

        [SerializeField]
        private AI_STATUS _aiStatus;

        public int Id { private set; get; } // 我自己的id，allAnimals里的key
        private int _attackerId; // 打我的人的id，这里保存的是id，因为这个id所代表的恐龙可能会被销毁，按照C++的习惯，就不直接保存指针了，访问以前要判定是否有效
        private int _eatingTreeId; // 被吃的树的id

        private delegate void ArrivedDelegate(int targetId);
        
        [Space(), Header("Debug"), Space(5)]
        [SerializeField, Tooltip("If true, AI changes to this animal will be logged in the console.")]
        private bool logChanges = false;

        #region 主要函数
        void Awake()
        {
            animator = GetComponent<Animator>();
            animator.applyRootMotion = false;
            //animator.Play("Idle", 0, Random.Range(0,1));
            characterController = GetComponent<CharacterController>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            // 初始化数值
            Born();
            
            // 初始化所有动画状态
            dinoStates = new AnimalState[stateCount];
            var stateNames = new string[stateCount] {"Idle", "Eat", "Walk", "Run", "Attack", "Death"};
            var animationBools = new string[stateCount] {"isIdling", "isEating", "isWalking", "isRunning", "isAttacking", "isDead"};
            for (int i = 0; i < stateCount; ++i)
            {
                dinoStates[i] = new AnimalState {stateName = stateNames[i], animationBool = animationBools[i]};
            }

            if (navMeshAgent && navMeshAgent.isActiveAndEnabled)
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

            Id = _animalKey;
            allAnimals.Add(_animalKey++, this);
        }
        
        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating("AI_running", 3, ScriptableAnimalStats.thinkingFrequency);
            StopAllCoroutines();
        }

        
        void Update()
        {
            if (AnimalManager.Instance.PeaceTime)
            {
                SetPeaceTime(true);
            }
        }

        private float _lastTime = 0f;
        private void AI_running()
        {
            // 这里的_deltaTime是真实的每次本函数调用的时间间隔（而不是Time.deltaTime）。
            //_deltaTime = Time.deltaTime;
            var nowTime = Time.fixedTime;
            _deltaTime = nowTime - _lastTime;
            _lastTime = nowTime;
            

            // No.1 死了就什么都不干了
            if (Dead)
                return;
            
            // No.2 消耗粮食
            float cost = ScriptableAnimalStats.liveCost * _deltaTime;
            if(_currentFood >= cost)
                _currentFood -= cost;
            else
            {
                _currentFood = 0;
                _currentToughness -= cost - _currentFood;
            }
            
            // No.3 判断是否该死
            if (_currentToughness <= 0)
            {
                Die();
                return;
            }

            // No.4 是否正在被别人攻击，被攻击的时候，则要么还击（其实只是播放动画），要么等死
            if (BeingAttacked())
                return;
            
            // No.5 是否正在攻击别人
            if (IsAttacking())
                return;
            
            // No.6 判断粮食还有存货，则补充生命值，立即补满
//            if (_currentFood > 0 && _currentToughness < ScriptableAnimalStats.toughness)
//            {
//                float needToughness = ScriptableAnimalStats.toughness - _currentToughness;
//                if (_currentFood > needToughness)
//                {
//                    _currentToughness = ScriptableAnimalStats.toughness;
//                    _currentFood -= needToughness;
//                }
//                else
//                {
//                    _currentToughness += needToughness;
//                    _currentFood = 0;
//                }
//            }
            
            // No.7 躲避天敌的逻辑
            if (LookforPredator())
                return;
            
            // No.8 寻找猎物的逻辑
            if (LookforPrey())
                return;
            
            // No.9 寻找植物性食物的逻辑
            if (LookforFood())
                return;

            // No.10 随机漫游的逻辑
            Wandering();
            
        }
        #endregion
        
        #region 工具函数
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
            _attackerId = -1;
            _eatingTreeId = -1;
        }

        public void Die()
        {
            if (logChanges)
            {
                Debug.Log(string.Format("{0} died. Current Health{1}", gameObject.name, _currentToughness));
            }
            StopAllCoroutines();
            Dead = true;
            PlayAnimation(DinoFSMState.StateEnum.DEAD);
            StopMoving();
            _currentFood = ScriptableAnimalStats.foodStorage;
            _currentToughness = ScriptableAnimalStats.toughness;
            _aiStatus = AI_STATUS.AI_DEAD;
            
            _attackerId = -1;
            _eatingTreeId = -1;
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

        private IEnumerator TurnToLookAtTarget(Transform target)
        {
            while (true)
            {
                Vector3 direction = target.position - transform.position;

                if (Vector3.Angle(direction, transform.forward) < 1f)
                {
                    break;
                }

                float step = 2f * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
                yield return null;
            }
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

        private Vector3 RandomPoint(float range)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere* range;
            return new Vector3(randomPoint.x, transform.position.y, randomPoint.z);
        }
        #endregion

        #region AI逻辑-第一层

        private bool BeingAttacked()
        {
            if (_aiStatus != AI_STATUS.AI_BEINGATTACKED)
            {
                LogicDino attacker = null;
                if (allAnimals.TryGetValue(_attackerId, out attacker))
                {
                    if (!attacker.Dead)
                    {
                        if (logChanges)
                        {
                            Debug.Log(string.Format("{0}: Getting attacked by {1}!", gameObject.name,
                                attacker.gameObject.name));
                        }

                        //StopMoving();
                        
                        StartCoroutine(TurnToLookAtTarget(attacker.transform));
                        if (ScriptableAnimalStats.agression > 0)
                        {
                            // 不真正还击，仅播放攻击动作，防止双方都一起死了
                            PlayAnimation(DinoFSMState.StateEnum.ATTACK);
                        }
                        else
                        {
                            PlayAnimation(DinoFSMState.StateEnum.IDLE);
                        }

                        _aiStatus = AI_STATUS.AI_BEINGATTACKED;

                        return true;
                    }
                }
            }
            else
            {
                LogicDino attacker = null;
                bool endBeingAttacked = true;
                if (allAnimals.TryGetValue(_attackerId, out attacker))
                {
                    float distance = Vector3.Distance(transform.position, attacker.transform.position);
                    if (!attacker.Dead && attacker._aiStatus == AI_STATUS.AI_ATTACKING && distance > ScriptableAnimalStats.contingencyDistance)
                    {
                        endBeingAttacked = false;
                    }
                }
                // 攻击者死亡，或者攻击者不是“攻击状态”了，那么自己也解除攻击状态
                if(endBeingAttacked)
                {
                    _attackerId = -1;
                    PlayAnimation(DinoFSMState.StateEnum.IDLE);
                    _aiStatus = AI_STATUS.AI_IDLE;
                    return false;
                }

                return true;
            }

            // 我没有处于被攻击状态，也没有任何人攻击我，什么都不做
            return false;
        }
        
        private bool LookforPredator()
        {
            if (_aiStatus != AI_STATUS.AI_ESCAPING)
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
                        if (logChanges)
                        {
                            Debug.Log(string.Format("{0}: Found predator ({1}), running away.", gameObject.name,
                                predator.gameObject.name));
                        }

                        if (useNavMesh)
                        {
                            var targetPosition = RandomPointAwayFromDestination(predator.transform.position, 30f);
                            Debug.Log("Escaping Distance : "+Vector3.Distance(transform.position, targetPosition));
                            StartCoroutine(WanderingState(targetPosition, predator.Id, ScriptableAnimalStats.runSpeed, DinoFSMState.StateEnum.RUN, AI_STATUS.AI_ESCAPING, null));

                        }
                        else
                        {
                            var targetPosition = RandomPointAwayFromDestination(predator.transform.position, 30f);
                            StartCoroutine(NonNavMeshWanderingState(targetPosition, predator.Id, ScriptableAnimalStats.runSpeed, DinoFSMState.StateEnum.RUN, AI_STATUS.AI_ESCAPING, null));
                        }

                        // 进入“躲避天敌”的状态
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        private bool LookforPrey()
        {
            if (_aiStatus != AI_STATUS.AI_CHASING)
            {
                if (ScriptableAnimalStats.dominance > 0)
                {
                    float distMin = float.MaxValue;
                    int index = -1;
                    for (int i = 0; i < allAnimals.Count; i++)
                    {
                        if (allAnimals[i].Dead == true || allAnimals[i] == this ||
                            (allAnimals[i].Species == Species && !ScriptableAnimalStats.territorial) ||
                            allAnimals[i].ScriptableAnimalStats.dominance > ScriptableAnimalStats.dominance ||
                            allAnimals[i].ScriptableAnimalStats.stealthy)
                        {
                            continue;
                        }

                        int p = System.Array.IndexOf(ScriptableAnimalStats.nonAgressiveTowards, allAnimals[i].Species);
                        if (p > -1)
                        {
                            continue;
                        }

                        float dist = Vector3.Distance(transform.position, allAnimals[i].transform.position);
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
                            index = i;
                        }
                    }

                    if (index >= 0 && index < allAnimals.Count)
                    {
                        var prey = allAnimals[index];
                        if (logChanges)
                        {
                            Debug.Log(string.Format("{0}: Found prey ({1}), chasing.", gameObject.name,
                                prey.gameObject.name));
                        }

                        if (prey == this)
                        {
                            Debug.LogError("Cannot eat myself!!!");
                        }

                        if (useNavMesh)
                        {
                            StartCoroutine(WanderingState(prey.transform.position, prey.Id,
                                ScriptableAnimalStats.runSpeed, DinoFSMState.StateEnum.RUN, AI_STATUS.AI_CHASING,
                                AttackAnimal));
                        }
                        else
                        {
                            StartCoroutine(NonNavMeshWanderingState(prey.transform.position, prey.Id,
                                ScriptableAnimalStats.runSpeed, DinoFSMState.StateEnum.RUN, AI_STATUS.AI_CHASING,
                                AttackAnimal));
                        }

                        return true;
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }
        
        private bool LookforFood()
        {
            if (_aiStatus != AI_STATUS.AI_LOOKFOR_FOOD)
            {
                if (_currentFood / ScriptableAnimalStats.foodStorage < ScriptableAnimalStats.HungryRate)
                {
                    float distMin = float.MaxValue;
                    int index = -1;
                    for (int i = 0; i < LogicTree.AllTrees.Count; ++i)
                    {
                        var tree = LogicTree.AllTrees[i];
                        float dist = Vector3.Distance(transform.position, tree.transform.position);
                        if (dist > ScriptableAnimalStats.scent)
                        {
                            continue;
                        }

                        if (dist < distMin)
                        {
                            distMin = dist;
                            index = i;
                        }
                    }

                    if (index >= 0 && index < LogicTree.AllTrees.Count)
                    {
                        var targetPosition = LogicTree.AllTrees[index].transform.position;
                        _eatingTreeId = index;
                        if (useNavMesh)
                        {
                            StartCoroutine(WanderingState(targetPosition, -1, ScriptableAnimalStats.moveSpeed,
                                DinoFSMState.StateEnum.WALK, AI_STATUS.AI_LOOKFOR_FOOD, EatFood));
                        }
                        else
                        {
                            StartCoroutine(NonNavMeshWanderingState(targetPosition, -1, ScriptableAnimalStats.moveSpeed,
                                DinoFSMState.StateEnum.WALK, AI_STATUS.AI_LOOKFOR_FOOD, EatFood));
                        }
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        private void Wandering()
        {
            if (_aiStatus != AI_STATUS.AI_WANDERING)
            {
                int ran = Random.Range(0, 1);
                if (ran < 1) // 每秒都有一半的概率，原地不动，休息。
                {
                    var targetPosition = RandomPoint(30f);
                    Debug.Log("Wandering Distance : "+ Vector3.Distance(transform.position, targetPosition));
                    if (useNavMesh)
                    {
                        StartCoroutine(WanderingState(targetPosition, -1, ScriptableAnimalStats.moveSpeed,
                            DinoFSMState.StateEnum.WALK, AI_STATUS.AI_WANDERING, null));
                    }
                    else
                    {
                        StartCoroutine(NonNavMeshWanderingState(targetPosition, -1, ScriptableAnimalStats.moveSpeed,
                            DinoFSMState.StateEnum.WALK, AI_STATUS.AI_WANDERING, null));
                    }
                }
            }
        }
        #endregion

        #region AI逻辑-第二层

        private void AttackAnimal(int targetId)
        {
            // 如果对方已经处于攻击状态，则我不能攻击对方，否则会导致navMeshAgent报错(即便是双方同归于尽也不是我们想看到的)，我感觉应该是多线程导致的报错。
//            if (!target.IfCanBeAttacked())
//                return;
            LogicDino target = null;
            if (allAnimals.TryGetValue(targetId, out target))
            {
                if (logChanges)
                {
                    Debug.Log(string.Format("{0}: Making Attack {1} !", gameObject.name, target.gameObject.name));
                }

                if (!target.Dead)
                {
                    float distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance < ScriptableAnimalStats.contingencyDistance)
                    {
                        StartCoroutine(MakeAttack(target));
                    }
                    else
                    {
                        Debug.Log("Should Attack but it's too far...");
                    }
                }
            }
        }

        private bool IfCanBeAttacked()
        {
            return _aiStatus != AI_STATUS.AI_ATTACKING && _aiStatus != AI_STATUS.AI_DEAD;
        }

        private IEnumerator MakeAttack(LogicDino target)
        {
            PlayAnimation(DinoFSMState.StateEnum.ATTACK);
            _aiStatus = AI_STATUS.AI_ATTACKING;
            target.SetAttacked(Id);
            
            while (!target.Dead)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance > ScriptableAnimalStats.contingencyDistance)
                {
                    PlayAnimation(DinoFSMState.StateEnum.IDLE);
                    _aiStatus = AI_STATUS.AI_IDLE;
                    yield break;
                }

                target.TakeDamage(ScriptableAnimalStats.power);
                
                yield return new WaitForSeconds(ScriptableAnimalStats.attackSpeed);
            }
            
            PlayAnimation(DinoFSMState.StateEnum.IDLE);
            _aiStatus = AI_STATUS.AI_IDLE;
        }

        private void SetAttacked(int attackerId)
        {
            _attackerId = attackerId;
        }

        private void TakeDamage(float damage)
        {
            _currentToughness -= damage;
            if (logChanges)
            {
                Debug.Log(string.Format("{0}: Taking Damage {1} HP {2}!", gameObject.name, damage, _currentToughness));
            }

            // 如果血不够了，立即死亡。不用等到下一个AI周期了。
            if (_currentToughness <= 0)
                Die();
        }

        private void EatFood(int targetId)
        {
            EatingFood(_eatingTreeId);
        }
        private IEnumerator EatingFood(int treeId)
        {
            PlayAnimation(DinoFSMState.StateEnum.IDLE);
            
            //......未完成
            
            yield return null;
        }

        private IEnumerator WanderingState(Vector3 targetPosition, int targetId, float speed, DinoFSMState.StateEnum aniState, AI_STATUS aiStatus, ArrivedDelegate onArrivedDelegate)
        {
            navMeshAgent.speed = speed;
            navMeshAgent.angularSpeed = ScriptableAnimalStats.turnSpeed;
            navMeshAgent.SetDestination(targetPosition);
            PlayAnimation(aniState);
            _aiStatus = aiStatus;
            float distance = Vector3.Distance(transform.position, targetPosition);
            float timeMoving = 0f;

            while ((navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance || timeMoving < 0.1f)
                   && timeMoving < ScriptableAnimalStats.stamina
                   && distance > ScriptableAnimalStats.contingencyDistance)
            {
                timeMoving += Time.deltaTime;
                
                LogicDino target = null;
                if (allAnimals.TryGetValue(targetId, out target))
                {
                    distance = Vector3.Distance(transform.position, target.transform.position);
                }

                yield return null;
            }

            if (logChanges)
            {
                Debug.Log(string.Format("Leave the {0} status", _aiStatus));
            }

            PlayAnimation(DinoFSMState.StateEnum.IDLE);
            navMeshAgent.speed = ScriptableAnimalStats.moveSpeed;
            navMeshAgent.angularSpeed = ScriptableAnimalStats.turnSpeed;
            navMeshAgent.SetDestination(transform.position);
            _aiStatus = AI_STATUS.AI_IDLE;
            onArrivedDelegate?.Invoke(targetId);
        }

        private IEnumerator NonNavMeshWanderingState(Vector3 targetPosition, int targetId, float speed, DinoFSMState.StateEnum aniState, AI_STATUS aiStatus, ArrivedDelegate onArrivedDelegate)
        {
            targetLocation = targetPosition;
            currentTurnSpeed = ScriptableAnimalStats.turnSpeed;
            PlayAnimation(aniState);
            _aiStatus = aiStatus;
            float distance = Vector3.Distance(transform.position, targetPosition);
            float distance2 = distance;
            float timeMoving = 0f;

            while (distance > ScriptableAnimalStats.contingencyDistance
                   && timeMoving < ScriptableAnimalStats.stamina
                   && distance2 > ScriptableAnimalStats.contingencyDistance)
            {
                timeMoving += Time.deltaTime;
                
                characterController.SimpleMove(transform.TransformDirection(Vector3.forward) * speed);
                Vector3 relativePos = targetPosition - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (currentTurnSpeed / 10));
                currentTurnSpeed += Time.deltaTime;
                
                LogicDino target = null;
                if (allAnimals.TryGetValue(targetId, out target))
                {
                    distance2 = Vector3.Distance(transform.position, target.transform.position);
                }
                distance = Vector3.Distance(transform.position, targetPosition);
                
                yield return null;
                
            }
            
            PlayAnimation(DinoFSMState.StateEnum.IDLE);
            currentTurnSpeed = ScriptableAnimalStats.turnSpeed;
            _aiStatus = AI_STATUS.AI_IDLE;
            targetLocation = transform.position;
            characterController.SimpleMove(Vector3.zero);
            onArrivedDelegate?.Invoke(targetId);
        }

        private void StopMoving()
        {
            if (useNavMesh)
            {
                navMeshAgent.SetDestination(transform.position);
            }
            else
            {
                characterController.SimpleMove(Vector3.zero);
            }
            
        }
        
    #endregion
    }
}