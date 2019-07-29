using Assets.Gamelogic.ComponentExtensions;
using Assets.Gamelogic.Core;
using Assets.Gamelogic.FSM;
using Assets.Gamelogic.NPC.Lumberjack;
using Dinopark.Core;
using Dinopark.Npc;
using UnityEngine;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;

namespace Assets.Gamelogic.NPC.LumberJack
{
    public class LumberjackIdleState : FsmBaseState<LumberjackStateMachine, LumberjackFSMState.StateEnum>
    {
        //private readonly TeamAssignmentReader teamAssignment;
        private readonly InventoryReader inventory;
        private readonly LumberjackBehaviour parentBehaviour;
        
        public LumberjackIdleState(LumberjackStateMachine owner,
                                   LumberjackBehaviour inParentBehaviour/*,
                                   TeamAssignment.Reader inTeamAssignment*/,
                                   InventoryReader inInventory)
            : base(owner)
        {
            parentBehaviour = inParentBehaviour;
            //teamAssignment = inTeamAssignment;
            inventory = inInventory;
        }

        public override void Enter()
        {
            CheckForNearbyTreesOrStockpiles();
        }

        public override void Tick()
        {
        }

        public override void Exit(bool disabled)
        {
        }

        private void CheckForNearbyTreesOrStockpiles()
        {
            GameObject targetEntity;
            if (inventory.HasResources())
            {
                targetEntity = NPCUtils.FindNearestTarget(parentBehaviour.gameObject, SimulationSettings.NPCViewRadius, NPCUtils.IsTargetATeamStockpile, LayerMask.GetMask(SimulationSettings.BarrackLayerName));
                if (targetEntity == null)
                {
                    MoveCloserToHQ();
                    return;
                }
            }
            else
            {
                targetEntity = NPCUtils.FindNearestTarget(parentBehaviour.gameObject, SimulationSettings.NPCViewRadius, NPCUtils.IsTargetAHealthyTree, LayerMask.GetMask(SimulationSettings.TreeLayerName));
                if (targetEntity == null)
                {
                    MoveToRandomPlaceNearby();
                    return;
                }
            }
            var linkedComponent = targetEntity.GetComponent<LinkedEntityComponent>(); // 终于找到一个gameObject和EntityId挂钩的地方
            if (linkedComponent != null)
            {
                MoveToEntity(linkedComponent.EntityId);
            }
        }

        private void MoveCloserToHQ()
        {
            //var hqPosition = SimulationSettings.TeamHQLocations[(int)teamAssignment.Data.teamId].ToVector3();
            var hqPosition = Vector3.zero;
            var offset = Random.insideUnitSphere * SimulationSettings.NPCWanderWaypointDistance;
            MoveToPosition(hqPosition + offset);
        }

        private void MoveToRandomPlaceNearby()
        {
            var offset = Random.insideUnitSphere * SimulationSettings.NPCWanderWaypointDistance;
            MoveToPosition(parentBehaviour.transform.position + offset);
        }
        
        private void MoveToEntity(EntityId targetEntityId)
        {
            Owner.TriggerTransition(LumberjackFSMState.StateEnum.MOVING_TO_TARGET, targetEntityId, Vector3.zero);
        }

        private void MoveToPosition(Vector3 position)
        {
            Owner.TriggerTransition(LumberjackFSMState.StateEnum.MOVING_TO_TARGET, new EntityId(), position);
        }
    }
}
