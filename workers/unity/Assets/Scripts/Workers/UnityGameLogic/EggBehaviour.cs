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
        _currentState = EggStateEnum.GOOD; //stateMachine.CurrentState;
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
        var exampleEntity = EntityTemplateFactory.CreateDinoBrachioTemplate(transform.position.ToCoordinates(), 0);
        var request1 = new WorldCommands.CreateEntity.Request(exampleEntity);
        worldCommandSender.SendCreateEntityCommand(request1, OnCreateEntityResponse);
        Debug.Log("EggBehaviour HatchOut!");
    }
    
    private void OnCreateEntityResponse(WorldCommands.CreateEntity.ReceivedResponse response)
    {
        if (response.EntityId.HasValue)
        {
            var entityId = response.EntityId.Value;
            Debug.Log("Server - new entity created:"+entityId);
            
        }
    }

    public void DestroyEgg()
    {
        //var linkentity = GetComponent<LinkedEntityComponent>();
        var request = new WorldCommands.DeleteEntity.Request(_entityId);
        worldCommandSender.SendDeleteEntityCommand(request);
    }
}
