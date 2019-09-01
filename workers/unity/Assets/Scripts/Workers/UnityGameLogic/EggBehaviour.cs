using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Improbable.Gdk.Subscriptions;
using Assets.Gamelogic.Core;
using Dinopark.Npc;
using Improbable.Gdk.Core;
using Assets.Gamelogic.Utils;
using DinoPark;
using Improbable.Gdk.Core.Commands;

[WorkerType(WorkerUtils.UnityGameLogic)]
public class EggBehaviour : MonoBehaviour
{
    [Require] public EggDataWriter egg;
    [Require] private EntityId _entityId;
    [Require] private WorldCommandSender worldCommandSender;

    private static Dictionary<long, EggBehaviour> allEggs = new Dictionary<long, EggBehaviour>();
    public static Dictionary<long, EggBehaviour> AllEggs { get { return allEggs; } }
    
    public EggStats eggStats;
    public EggStateMachine stateMachine;
    
    [Space(), Header("Attributes"), Space(5)]
    [SerializeField] private long _id;
    [SerializeField] private EggTypeEnum _eggType;
    [SerializeField] private float _currentFood;
    [SerializeField] private EggStateEnum _currentState;
    
    [Space(), Header("Debug"), Space(5)]
    [SerializeField, Tooltip("If true, AI changes to this animal will be logged in the console.")]
    public bool logChanges = false;
    
    void Awake()
    {
        eggStats = GetComponent<EggStats>();
    }
    // Start is called before the first frame update
    void Start()
    {
        allEggs.Add(_entityId.Id, this);
        _id = _entityId.Id;
        _currentState = egg.Data.CurrentState; //stateMachine.CurrentState;
        _eggType = egg.Data.EggType;
        _currentFood = egg.Data.CurrentFood;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Tick();
    }

    private void OnEnable()
    {
        stateMachine = new EggStateMachine(this);
        stateMachine.OnEnable(egg.Data.CurrentState);
    }
    private void OnDisable()
    {
        stateMachine.OnDisable();
    }
    
    public void HatchOut()
    {
        EntityTemplate exampleEntity = null;
        switch (_eggType)
        {
            case EggTypeEnum.Brachiosaurus:
                exampleEntity = EntityTemplateFactory.CreateDinoBrachioTemplate(transform.position.ToCoordinates(), egg.Data.OwnEntityId, 0); // 0岁小恐龙
                break;
            case EggTypeEnum.TRex:
                exampleEntity = EntityTemplateFactory.CreateDinoTRexTemplate(transform.position.ToCoordinates(), egg.Data.OwnEntityId, 0); // 0岁小恐龙
                break;
        }

        if (exampleEntity != null)
        {
            var request1 = new WorldCommands.CreateEntity.Request(exampleEntity);
            worldCommandSender.SendCreateEntityCommand(request1, OnCreateDinoResponse);
            //Debug.Log("EggBehaviour HatchOut! Type:"+_eggType);
        }
    }
    
    private void OnCreateDinoResponse(WorldCommands.CreateEntity.ReceivedResponse response)
    {
        if (response.EntityId.HasValue)
        {
            var entityId = response.EntityId.Value;
            // 这个函数回调的时候，被创建出来的恐龙的GameObject还没有被创建，所以本函数不能有任何访问被创建出来的物体的操作
            // 只能写一些针对自己的操作
        }
    }

    public void DestroyEgg()
    {
        allEggs.Remove(_entityId.Id);
        var linkentity = GetComponent<LinkedEntityComponent>();
        var request = new WorldCommands.DeleteEntity.Request(linkentity.EntityId);
        worldCommandSender.SendDeleteEntityCommand(request);
        //Debug.Log("Server - destroy an egg:"+_entityId);
    }
}
