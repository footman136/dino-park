using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Assets.Gamelogic.Utils;
using UnityEngine;
using UnityEngine.AI;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Assets.Gamelogic.Core;
using Assets.Gamelogic.Tree;
using DinoPark;
using Dinopark.Plants;
using Dinopark.Life;
using Dinopark.Npc;
using Improbable.Gdk.Core.Commands;

/// <summary>
/// 恐龙AI逻辑框架，用于挂接在UntiyGameLogic（服务器）端
/// </summary>
[WorkerType(WorkerUtils.UnityGameLogic)]
public class DinoBehaviour : MonoBehaviour
{
    [Require] public DinoAiDataWriter aiDataWriter;
    [Require] public DinoAttrsWriter attrsWriter;
    [Require] private HealthWriter health;
    [Require] private AgeWriter age;
    [Require] public EntityId _entityId;
    [Require] private DinoAiDataCommandSender cmdSender;
    [Require] private DinoAiDataCommandReceiver cmdReceiver;
    [Require] private WorldCommandSender worldCommandSender;
    
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
    [SerializeField] private int _age; // 年龄，仅用于显示
    [SerializeField] private float _curFood; // 当前的粮食，仅用于显示
    [SerializeField] private float _maxFood; // 最大的粮食储量，仅用于显示
    [SerializeField] private float _liveCost; // 能量消耗，仅用于显示
    [SerializeField] private float _curHealth; // 当前生命值，仅用于显示
    [SerializeField] private float _maxHealth; // 最大生命值，仅用于显示
    [SerializeField] private float _lastHatchTime; // 距离上次孵蛋的时间
    public string Species { private set; get; }

    public bool Dead => stateMachine.CurrentState == DinoAiFSMState.StateEnum.DEAD ||
                        stateMachine.CurrentState == DinoAiFSMState.StateEnum.DEAD;

    public bool IsVanish => stateMachine.CurrentState == DinoAiFSMState.StateEnum.VANISH;

    public bool IsAlive => (stateMachine.CurrentState != DinoAiFSMState.StateEnum.DEAD &&
                            stateMachine.CurrentState != DinoAiFSMState.StateEnum.VANISH);
    private bool IsEating => (stateMachine.CurrentState == DinoAiFSMState.StateEnum.EAT ||
                              stateMachine.CurrentState == DinoAiFSMState.StateEnum.EAT_FOOD);

    private bool IsHatching => stateMachine.CurrentState == DinoAiFSMState.StateEnum.HATCH;
    private bool IsFighting => stateMachine.CurrentState == DinoAiFSMState.StateEnum.ATTACK;
    private bool IsChild => age.Data.Age < age.Data.GrowUpAge;

    private long OwnerTokenId => attrsWriter.Data.OwnerTokenId;


    [Space(), Header("Debug"), Space(5)]
    [SerializeField, Tooltip("If true, AI changes to this animal will be logged in the console.")]
    public bool logChanges = false;
    
#region 主函数
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = ScriptableAnimalStats.contingencyDistance;
        navMeshAgent.angularSpeed = ScriptableAnimalStats.turnSpeed;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        allAnimals.Add(_entityId.Id, this);
        _id = _entityId.Id;
        Species = ScriptableAnimalStats.species;
        bFirst = true;
        gameObject.SetActive(true);
        newBorn();
        StartCoroutine(Running());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();        
    }

    // Update is called once per frame
//    private float TIME_DELAY = 0.03f;
//    private float timeNow = 0;
    void Update()
    {
        stateMachine.Tick();
//        timeNow += Time.deltaTime;
//        if (timeNow < TIME_DELAY)
//        {
//            return;
//        }
//
//        timeNow = 0;
        
        
        _currentAiState = stateMachine.CurrentState;
        _curFood = attrsWriter.Data.CurrentFood;
        _maxFood = attrsWriter.Data.MaxFood;
        _liveCost = attrsWriter.Data.LiveCost;
        _curHealth = health.Data.CurrentHealth;
        _maxHealth = health.Data.MaxHealth;
        _age = Mathf.FloorToInt(age.Data.Age);
        
        if (navMeshAgent.enabled && navMeshAgent.gameObject.activeSelf)
        {
            distance = navMeshAgent.remainingDistance;
        }
    }
    
    private void OnEnable()
    {
        stateMachine = new DinoStateMachine(this);
        stateMachine.OnEnable(aiDataWriter.Data.CurrentAiState);
        _stateCount = stateMachine.StateCount();
        cmdReceiver.OnAttackRequestReceived += OnAttack;
        cmdReceiver.OnEatRequestReceived += OnEat;
    }

    private void OnDisable()
    {
        stateMachine.OnDisable();
        cmdReceiver.OnAttackRequestReceived -= OnAttack;
        cmdReceiver.OnEatRequestReceived -= OnEat;
    }
#endregion

#region 工具函数

    public void newBorn()
    {
        // 如果是儿童，一些属性要适当缩小
        float ratioChild = 1.0f;
        if (age.Data.Age < age.Data.GrowUpAge)
            ratioChild = SimulationSettings.NPCChildhoodMinWeekness;
        
        var update = new DinoAttrs.Update
        {
            IsDead = false,
            OriginalAgression = ScriptableAnimalStats.agression,
            OriginalDominance = ScriptableAnimalStats.dominance,
            OriginalScent = ScriptableAnimalStats.scent,
            OriginPosition = transform.position.ToVector3f(),
            LastHatchTime = 0,
            CurrentFood = Random.Range(ScriptableAnimalStats.foodStorage * SimulationSettings.NPCOriginalMinFoodRate * ratioChild, ScriptableAnimalStats.foodStorage * ratioChild),  // 儿童
            MaxFood = ScriptableAnimalStats.foodStorage * ratioChild, // 儿童
            Power = ScriptableAnimalStats.power * ratioChild, // 儿童
            LiveCost = ScriptableAnimalStats.liveCost * ratioChild, // 儿童
        };
        attrsWriter.SendUpdate(update);
        
        // 小恐龙的生命值也要打折扣
        var update3 = new Health.Update
        {
            CurrentHealth = ScriptableAnimalStats.toughness * ratioChild,  // 儿童
            CanBeChanged = true,
            MaxHealth = ScriptableAnimalStats.toughness * ratioChild,  // 儿童
        };
        health.SendUpdate(update3);
    }

    public void GrowUp()
    {
        if (age.Data.Age < age.Data.GrowUpAge)
            return;
        float ratioChild = 1.0f;
        
        // 恢复小恐龙的粮食，攻击力，生命消耗等属性，变成成人的
        float curFood = attrsWriter.Data.CurrentFood;
        float maxFood = attrsWriter.Data.MaxFood;
        var update = new DinoAttrs.Update
        {
            CurrentFood = ScriptableAnimalStats.foodStorage * curFood / maxFood, // 当前的粮食按照比例提升
            MaxFood = ScriptableAnimalStats.foodStorage, // 恢复成人数值
            Power = ScriptableAnimalStats.power, // 恢复成人数值
            LiveCost = ScriptableAnimalStats.liveCost,// 恢复成人数值
        };
        attrsWriter.SendUpdate(update);
        
        // 恢复小恐龙的生命值，变成成人的
        float curHealth = health.Data.CurrentHealth;
        float maxHealth = health.Data.MaxHealth;
        var update3 = new Health.Update
        {
            CurrentHealth = ScriptableAnimalStats.toughness * curHealth / maxHealth, // 当前的生命按照比例提升
            CanBeChanged = true,
            MaxHealth = ScriptableAnimalStats.toughness
        };
        health.SendUpdate(update3);
    }

    public void Die()
    {
        var update2 = new Health.Update
        {
            CurrentHealth = 0,
        };
        health.SendUpdate(update2);
        
        stateMachine.TriggerTransition(DinoAiFSMState.StateEnum.DEAD, new EntityId(), DinoStateMachine.InvalidPosition);
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
        if (pos.x >= 120 || pos.x <= -120 || pos.z >= 120 || pos.z <= -120)
            return false;
        return true;
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
            ScriptableAnimalStats.dominance = attrsWriter.Data.OriginalDominance;
            ScriptableAnimalStats.scent = attrsWriter.Data.OriginalScent;
            ScriptableAnimalStats.agression = attrsWriter.Data.OriginalAgression;
        }
    }
#endregion
    
#region AI 函数 - 第一层

    IEnumerator Running()
    {
        yield return new WaitForSeconds(ScriptableAnimalStats.thinkingFrequency);
        while (true)
        {
            AI_Running();
            
            yield return new WaitForSeconds(ScriptableAnimalStats.thinkingFrequency);
        }
    }

    private bool bFirst = true;
    private float _lastTime = 0f;
    private void AI_Running()
    {
        // 这里的_deltaTime是真实的每次本函数调用的时间间隔（而不是Time.deltaTime）。
        //_deltaTime = Time.deltaTime;
        var nowTime = Time.time;
        _deltaTime = nowTime - _lastTime;
        _lastTime = nowTime;

        if (bFirst)
        {
            //newBorn();
            _deltaTime = 0; // 第一次不记录时间延迟
            bFirst = false;
        }

        // No.1 判断是否该死了
        if (IfShouldDie())
            return;
        // No.2. 是否正在战斗中
        if (IsFighting)
            return;

        // No.8 下蛋
        if (Hatching())
            return;

        // No.4 躲避天敌
        if (LookforPredator())
            return;
        
        // No.5 吃饭的时候，不再寻找其他的猎物
        if (IsEating)
            return;
        
        // No.6 寻找食物(可以立即吃的)
        if (LookforFood())
            return;
        
        // No.7 寻找猎物
        if (LookforPrey())
            return;
        
        // No.10 进入随便溜达（WANDER）的状态
        Wandering();
    }

    private bool IfShouldDie()
    {
        // No.1 如果已经死了，就啥都不干了
        if (!IsAlive)
            return true;
        
        // No.2 恢复健康
        // BUG：如果服务器算不过来，而导致[UnityGameLogic] Worker disconnected
        // 这时候health,age等变量会被置空，从而抛出异常。这是正常现象。
        // 应该去查找为什么会与服务器断开链接，往往是因为计算量太大了，算不过来了而导致的本问题。Sep.7.2019, Liu Gang.
        _curHealth = health.Data.CurrentHealth;
        _maxHealth = health.Data.MaxHealth;
        if (_curHealth + ScriptableAnimalStats.liveCost < _maxHealth)
        {// 健康的恢复速度是粮食消耗速度的一半，不能大于或者粮食消耗的速度，否则就不会饿死了
            _curHealth += ScriptableAnimalStats.liveCost / 2;
        }
        
        // No.3 消耗粮食
        float cost = ScriptableAnimalStats.liveCost * _deltaTime;
        _curFood = attrsWriter.Data.CurrentFood;
        if( _curFood >= cost)
            _curFood -= cost;
        else
        {
            _curFood = 0;
            _curHealth -= cost - _curFood;// 粮食不够，就消耗健康
        }
        var update = new DinoAttrs.Update
        {
            CurrentFood = _curFood,
        };
        attrsWriter.SendUpdate(update);
        var update3 = new Health.Update
        {
            CurrentHealth = _curHealth
        };
        health.SendUpdate(update3);
            
        // No.4 年龄增加
        float lastAge = age.Data.Age;
        float nowAge = age.Data.Age + _deltaTime;
        float growupAge = age.Data.GrowUpAge;
        _age = Mathf.FloorToInt(nowAge); // 仅用于显示
        var update2 = new Age.Update
        {
            Age = nowAge
        };
        age.SendUpdate(update2);
        
        if (_age >= growupAge && lastAge < growupAge)
        {// 成人以后，血量恢复至原始大小
            GrowUp();
        }

        // No.5 判断是否该死了
        if (_curHealth <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    /// <summary>
    /// 寻找天敌
    /// </summary>
    /// <returns></returns>
    private bool LookforPredator()
    {
        if (ScriptableAnimalStats.awareness <= 0) // 没有感知能力，不进入此逻辑
            return false;
        if (stateMachine.CurrentState == DinoAiFSMState.StateEnum.RUN_AWAY) // 如果已经处于本状态，则继续
            return true;
           
        float distMin = float.MaxValue;
        DinoBehaviour predator = null;
        foreach(var iter in allAnimals)
        {
            var animal = iter.Value;
            if (!animal.IsAlive || animal == this || animal.Species == Species ||
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

    /// <summary>
    ///  寻找猎物
    /// </summary>
    /// <returns></returns>
    private bool LookforPrey()
    {
        if (ScriptableAnimalStats.dominance <= 0) // 处于食物链的低端，不会寻找猎物
            return false;
        if (stateMachine.CurrentState == DinoAiFSMState.StateEnum.CHASE) // 已经处于本状态，继续
            return true;
        
        List<DinoBehaviour> preyList = new List<DinoBehaviour>();
        foreach(var iter in allAnimals)
        {
            var animal = iter.Value;
            if (!animal.IsAlive || animal == this ||
                animal.ScriptableAnimalStats.dominance > ScriptableAnimalStats.dominance ||
                animal.ScriptableAnimalStats.stealthy)
            {
                continue;
            }

            if (animal.Species == Species && !ScriptableAnimalStats.territorial)
            { // 不可以攻击同类
                continue;
            }
            if (animal.Species == Species && ScriptableAnimalStats.territorial &&
                 (animal.IsChild || IsChild))
            { // 不可以攻击同类的孩子
                continue;
            }

            if (animal.Species == Species && ScriptableAnimalStats.territorial &&
                (animal.OwnerTokenId == OwnerTokenId))
            { // 不可以攻击归属同一个玩家的恐龙
                continue;
            }

            int p = System.Array.IndexOf(ScriptableAnimalStats.nonAgressiveTowards, animal.Species);
            if (p > -1)
            {
                continue;
            }

            // 如果太饿了，嗅觉会增加一倍的距离
            float scent = ScriptableAnimalStats.scent;
            _curFood = attrsWriter.Data.CurrentFood;
            if (_curFood / attrsWriter.Data.MaxFood < ScriptableAnimalStats.HungryRate)
            {
                scent *= 2;
            }

            float dist = Vector3.Distance(transform.position, animal.transform.position);
            if (dist > scent)
            {
                continue;
            }

            if (Random.Range(0, 100) > ScriptableAnimalStats.agression)
            {
                continue;
            }

            preyList.Add(animal);
        }

        // 收集范围内的所有目标，随机找一个作为最终目标
        DinoBehaviour prey = null;
        if (preyList.Count > 0)
        {
            int rand = Random.Range(0, preyList.Count);
            prey = preyList[rand];
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
        if (stateMachine.CurrentState == DinoAiFSMState.StateEnum.LOOK_FOR_FOOD)
            return true;
        _curFood = attrsWriter.Data.CurrentFood;
        if (_curFood / attrsWriter.Data.MaxFood >= ScriptableAnimalStats.HungryRate)
        { // 不饿
            return false;
        }

        if (ScriptableAnimalStats.vegetarian) // 素食主义者，吃树
        {
            List<TreeBehaviour> treeList = new List<TreeBehaviour>();
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
                treeList.Add(tree);
            }

            // 收集范围内的所有目标，随机找一个作为最终目标
            TreeBehaviour aTree = null;
            if (treeList.Count > 0)
            {
                int rand = Random.Range(0, treeList.Count);
                aTree = treeList[rand];
            }
            
            if (aTree != null)
            {
                var targetPosition = aTree.transform.position;
                if(aTree._entityId.Id == 0)
                    Debug.LogError("Look For Food EntityId is ZERO!!!");
                stateMachine.TriggerTransition(DinoAiFSMState.StateEnum.LOOK_FOR_FOOD, aTree._entityId,
                    targetPosition);
                return true;
            }
        }
        else // 肉食主义者，吃尸体
        {
            List<DinoBehaviour> corpseList = new List<DinoBehaviour>();
            foreach (var iter in allAnimals)
            {
                var animal = iter.Value;
                if (!(animal.Dead && !animal.IsVanish))
                {
                    continue;
                }
                float dist = Vector3.Distance(transform.position, animal.transform.position);
                if (dist > ScriptableAnimalStats.scent)
                {
                    continue;
                }
                corpseList.Add(animal);
            }

            // 收集范围内的所有目标，随机找一个作为最终目标
            DinoBehaviour corpse = null;
            if (corpseList.Count > 0)
            {
                int rand = Random.Range(0, corpseList.Count);
                corpse = corpseList[rand];
            }
            
            if (corpse != null)
            {
                var targetPosition = corpse.transform.position;
                if(!corpse.Dead)
                    Debug.LogError("DinoBehaviour - Cannot eat live animals!");
                stateMachine.TriggerTransition(DinoAiFSMState.StateEnum.LOOK_FOR_FOOD, corpse._entityId,
                    targetPosition);
                return true;
            }
        }

        return false;
    }

    private bool Hatching()
    {
        if (stateMachine.CurrentState == DinoAiFSMState.StateEnum.HATCH)
            return true;
        
        // 未成年，不能下蛋
        if (_age < ScriptableAnimalStats.growUpTime)
        {
            return false;
        }
        _curFood = attrsWriter.Data.CurrentFood;
        if (_curFood / attrsWriter.Data.MaxFood < ScriptableAnimalStats.hatchRate)
        { // 粮食储备不够，不能下蛋
            return false;
        }

        float deltaTime = Time.time - attrsWriter.Data.LastHatchTime;
        if (deltaTime < ScriptableAnimalStats.hatchDelay)
        { // 距离上次下蛋时间间隔太短，不能下蛋
            return false;
        }

        var update = new DinoAttrs.Update
        {
            LastHatchTime = Time.time,
        };
        attrsWriter.SendUpdate(update);

        stateMachine.TriggerTransition(DinoAiFSMState.StateEnum.HATCH, new EntityId(), transform.position);
        return true;
    }
    
    private void Wandering()
    {
        if (stateMachine.CurrentState != DinoAiFSMState.StateEnum.WANDER)
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
    
#region AI - 逻辑 - 第二层

    public void DoAttack(DinoBehaviour target)
    {
        var request = new AttackRequest()
        {
            Attacker = _entityId,
            Damage = attrsWriter.Data.Power
        };
        
        cmdSender.SendAttackCommand(target._entityId, request, OnDoAttack);   
    }

    public void OnDoAttack(DinoAiData.Attack.ReceivedResponse response)
    {
        // do nothing for now.
    }

    public void OnAttack(DinoAiData.Attack.ReceivedRequest request)
    {
        var payload = request.Payload;
        _curHealth = health.Data.CurrentHealth;
        _curHealth -= payload.Damage;
        if (logChanges)
        {
            Debug.Log(string.Format("{0}: Taking Damage {1} HP {2}!", gameObject.name, payload.Damage, _curHealth));
        }
        
        // 生命减少
        var update = new Health.Update
        {
            CurrentHealth = _curHealth
        };
        health.SendUpdate(update);
        // 把生命减少的一半变成粮食储存起来，便于死后给食肉恐龙提供食物，否则如果食肉恐龙杀死一只饥饿（Food为零）的食草恐龙以后，什么都没得吃
        _curFood = attrsWriter.Data.CurrentFood;
        float addFood = payload.Damage / 2;
        if (addFood + _curFood > attrsWriter.Data.MaxFood)
        {
            addFood = attrsWriter.Data.MaxFood - _curFood;
        }
        var update2 = new DinoAttrs.Update
        {
            CurrentFood = _curFood + addFood
        };
        attrsWriter.SendUpdate(update2);

        // 如果血不够了，立即死亡。不用等到下一个AI周期了。
        if (_curHealth <= 0)
        {
            if(IsAlive)
                Die();
        }
        else
        {// 挨打的一方也进入战斗状态
            DinoBehaviour attacker = null;
            if(allAnimals.TryGetValue(payload.Attacker.Id, out attacker))
            {
                if (IsAlive && stateMachine.CurrentState != DinoAiFSMState.StateEnum.ATTACK)
                {
                    stateMachine.TriggerTransition(DinoAiFSMState.StateEnum.ATTACK, payload.Attacker,attacker.transform.position);
                }
            }
        }
    }

    public void DoEat(DinoBehaviour corpse)
    {
        var hungry = attrsWriter.Data.MaxFood - attrsWriter.Data.CurrentFood;
        var resNeed = ScriptableAnimalStats.appetite;
        if (hungry < ScriptableAnimalStats.appetite)
            resNeed = hungry;
        var request = new EatRequest()
        {
            Attacker = _entityId,
            ResNeed = resNeed
        };
        
        if (!corpse.Dead)
            Debug.LogError("DoEat - Cannot eat live animals! Id:"+corpse._entityId+" state:" + corpse.stateMachine.CurrentState);
        cmdSender.SendEatCommand(corpse._entityId, request, OnDoEat);   
        
    }
    public void OnDoEat(DinoAiData.Eat.ReceivedResponse response)
    {
        // resTaken：实际吃到的粮食，如果树上粮食已经不足了，则剩余多少都给过来，所以不一定是appetite，也不一定是resNeed
        EatResponse? payload = response.ResponsePayload;
        float resTaken = 0f;
        if (payload.HasValue)
            resTaken = payload.Value.ResTaken;
        //Debug.Log("Command<HarvestRequest> sent! Harvester<"+response.SendingEntity.Index+"> Tree<"+response.EntityId+"> Resource Taken<"+resTaken+"> StatusCode<"+response.StatusCode+"> Message<"+response.Message+">");
        _curFood = attrsWriter.Data.CurrentFood;
        var update = new DinoAttrs.Update
        {
            CurrentFood = _curFood + resTaken
        };
        attrsWriter.SendUpdate(update);
    }

    public void OnEat(DinoAiData.Eat.ReceivedRequest request)
    {
        var payload = request.Payload;
        _curFood = attrsWriter.Data.CurrentFood;
        float resTaken = 0;
        if (_curFood > payload.ResNeed)
        {
            resTaken = payload.ResNeed;
            _curFood -= payload.ResNeed;
        }
        else
        {
            resTaken = _curFood;
            _curFood = 0;
        }

        var update = new EatResponse()
        {
            ResTaken = resTaken
        };
        cmdReceiver.SendEatResponse(request.RequestId, update);
        //Debug.Log("OnHarvest ResourceNeed<"+payload.ResourcesNeed+"> Harvester<"+payload.Harvester.Id+">");

        var update2 = new DinoAttrs.Update
        {
            CurrentFood = _curFood
        };
        attrsWriter.SendUpdate(update2);

    }

    public void HarvestFood(TreeBehaviour aTree)
    {
        // resNeed：每次想吃appetite数量的粮食live，如果快饱了，则仅吃饱为止，不多吃
        var hungry = attrsWriter.Data.MaxFood - attrsWriter.Data.CurrentFood;
        var resNeed = ScriptableAnimalStats.appetite;
        if (hungry < ScriptableAnimalStats.appetite)
            resNeed = hungry;
        var request = new HarvestRequest()
        {
            Harvester = _entityId,
            ResourcesNeed = resNeed
        };
        aTree.Harvest(request, OnHarvestFood);
    }
    public void OnHarvestFood(Harvestable.Harvest.ReceivedResponse response)
    {
        // resTaken：实际吃到的粮食，如果树上粮食已经不足了，则剩余多少都给过来，所以不一定是appetite，也不一定是resNeed
        HarvestResponse? payload = response.ResponsePayload;
        float resTaken = 0f;
        if (payload.HasValue)
            resTaken = payload.Value.ResourcesTaken;
        //Debug.Log("Command<HarvestRequest> sent! Harvester<"+response.SendingEntity.Index+"> Tree<"+response.EntityId+"> Resource Taken<"+resTaken+"> StatusCode<"+response.StatusCode+"> Message<"+response.Message+">");
        _curFood = attrsWriter.Data.CurrentFood;
        var update = new DinoAttrs.Update
        {
            CurrentFood = _curFood + resTaken
        };
        attrsWriter.SendUpdate(update);
    }

    /// <summary>
    /// 删除一个恐龙
    /// </summary>
    public void DestroyDino()
    {
        allAnimals.Remove(_entityId.Id);
        transform.parent = null;
        var linkentity = GetComponent<LinkedEntityComponent>();
        var request = new WorldCommands.DeleteEntity.Request(linkentity.EntityId);
        worldCommandSender.SendDeleteEntityCommand(request);
        Destroy(gameObject);
    }

    public void LayEgg()
    {
        EggTypeEnum eggType = 0;
        if (Species == SimulationSettings.Dino_Brachio_PrefabName)
        {
            eggType = EggTypeEnum.Brachiosaurus;
        }
        else if(Species == SimulationSettings.Dino_TRex_PrefabName)
        {
            eggType = EggTypeEnum.TRex;
        }
        // 恐龙自己下蛋
        var exampleEntity = EntityTemplateFactory.CreateEggTemplate(transform.position.ToCoordinates(), attrsWriter.Data.OwnerTokenId, eggType);
        var request1 = new WorldCommands.CreateEntity.Request(exampleEntity);
        worldCommandSender.SendCreateEntityCommand(request1, OnCreateEggResponse);
        //Debug.Log("DinoBehaviour LayEgg! Egg type : "+eggType);
    }

    private void OnCreateEggResponse(WorldCommands.CreateEntity.ReceivedResponse response)
    {
        if (response.EntityId.HasValue)
        {
            var entityId = response.EntityId.Value;
            //Debug.Log("Server - new egg created:"+entityId);
        }
        // 下蛋成功，身上的粮食要减半
        _curFood = attrsWriter.Data.CurrentFood;
        var update = new DinoAttrs.Update
        {
            CurrentFood = _curFood / 2,
        };
        attrsWriter.SendUpdate(update);
    }

#endregion
}
