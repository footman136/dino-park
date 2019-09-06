using UnityEngine;
using System.Collections.Generic;
using Assets.Gamelogic.Core;
using Assets.Gamelogic.Fire;
using Dinopark.Fire;
using Dinopark.Life;
using Dinopark.Plants;
using DinoPark;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using System;

namespace Assets.Gamelogic.Tree
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class TreeBehaviour : MonoBehaviour
    {
        private static Dictionary<long, TreeBehaviour> allTrees = new Dictionary<long, TreeBehaviour>();
        public static Dictionary<long, TreeBehaviour> AllTrees { get { return allTrees; } }

        [Require] private TreeStateWriter tree;
        [Require] private FlammableWriter flammable;
        [Require] private HealthWriter health;
        [Require] public EntityId _entityId;
        [Require] private HarvestableCommandSender cmdSender;

        private long _id;
        private HarvestableBehaviour _harvestable;

        
        [SerializeField] private FlammableBehaviour flammableInterface;

        private TreeStateMachine stateMachine;

        private void Awake()
        {
            flammableInterface = gameObject.GetComponentIfUnassigned(flammableInterface);
            _harvestable = gameObject.GetComponent<HarvestableBehaviour>();
        }

        void Start()
        {
            allTrees.Add(_entityId.Id, this);
            _id = _entityId.Id;
        }

        private void OnEnable()
        {
            stateMachine = new TreeStateMachine(this, 
                tree,
                health,
                flammableInterface,
                flammable);

            stateMachine.OnEnable(tree.Data.CurrentState);
        }

        private void OnDisable()
        {
            stateMachine.OnDisable();
        }
        public bool IsHavestable()
        {
            if (_harvestable != null)
            {
                if (stateMachine.CurrentState == TreeFSMState.HEALTHY)
                    return true;
            }

            return false;
        }

        public void Harvest(HarvestRequest request, Action<Harvestable.Harvest.ReceivedResponse> callback)
        {
            cmdSender.SendHarvestCommand(_entityId, request, callback);
            //Debug.Log("TreeBehaviour SendHarvestCommand Harvester<"+request.Harvester+"> Tree<"+_entityId.Id+"> Resource Need<"+request.ResourcesNeed+">");
        }

        public static int AliveCount()
        {
            int count = 0;
            foreach(var treePair in TreeBehaviour.AllTrees)
            {
                if (treePair.Value.IsAlive())
                {
                    count++;
                }
            }
            return count;
        }

        public bool IsAlive() => stateMachine.Data.CurrentState == TreeFSMState.HEALTHY;
    }
}
