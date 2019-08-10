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
                exampleEntity = EntityTemplateFactory.CreateDinoBrachioTemplate(transform.position.ToCoordinates(), 0, 0); // 0岁小恐龙
                break;
            case EggTypeEnum.TRex:
                exampleEntity = EntityTemplateFactory.CreateDinoTRexTemplate(transform.position.ToCoordinates(), 0, 0); // 0岁小恐龙
                break;
        }

        if (exampleEntity != null)
        {
            var request1 = new WorldCommands.CreateEntity.Request(exampleEntity);
            worldCommandSender.SendCreateEntityCommand(request1, OnCreateDinoResponse);
            Debug.Log("EggBehaviour HatchOut! Type:"+_eggType);
        }
    }
    
    private void OnCreateDinoResponse(WorldCommands.CreateEntity.ReceivedResponse response)
    {
        if (response.EntityId.HasValue)
        {
            var entityId = response.EntityId.Value;
            Debug.Log("Server - new dinosaur created:"+entityId);
            DinoBehaviour newBorn = null;
            if(DinoBehaviour.AllAnimals.TryGetValue(entityId.Id, out newBorn))
            {
                newBorn.newBorn();
                Debug.Log("newBorn baby!");
            }
        }
    }

    public void DestroyEgg()
    {
        //var linkentity = GetComponent<LinkedEntityComponent>();
        var request = new WorldCommands.DeleteEntity.Request(_entityId);
        worldCommandSender.SendDeleteEntityCommand(request);
        Debug.Log("Server - destroy an egg:"+_entityId);
    }
}
