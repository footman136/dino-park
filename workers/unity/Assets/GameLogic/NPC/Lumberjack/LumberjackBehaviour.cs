using Assets.Gamelogic.Core;
using Assets.Gamelogic.Fire;
using Assets.Gamelogic.NPC.LumberJack;
using Dinopark.Npc;
using Dinopark.Core;
using UnityEngine;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.Core;

namespace Assets.Gamelogic.NPC.Lumberjack
{
    [WorkerType(WorkerUtils.UnityGameLogic)]
    public class LumberjackBehaviour : MonoBehaviour, IFlammable
    {
        [Require] private NPCLumberjackWriter npcLumberjack;
        [Require] private TargetNavigationWriter targetNavigation;
        [Require] private InventoryWriter inventory;
//        [Require] private TeamAssignment.Reader teamAssignment;

        [SerializeField] private TargetNavigationBehaviour navigation;

        private LumberjackStateMachine stateMachine;

        private void Awake()
        {
            navigation = gameObject.GetComponentIfUnassigned(navigation);
        }

        private void OnEnable()
        {
            stateMachine = new LumberjackStateMachine(this, navigation, inventory, targetNavigation, npcLumberjack/*, teamAssignment*/);
            stateMachine.OnEnable(npcLumberjack.Data.CurrentState);
        }

        private void OnDisable()
        {
            stateMachine.OnDisable();
        }

        public void OnIgnite()
        {
			stateMachine.TriggerTransition(LumberjackFSMState.StateEnum.ON_FIRE, new EntityId(), SimulationSettings.InvalidPosition);
        }

        public void OnExtinguish()
        {
			stateMachine.TriggerTransition(LumberjackFSMState.StateEnum.IDLE, new EntityId(), SimulationSettings.InvalidPosition);
        }
    }
}
