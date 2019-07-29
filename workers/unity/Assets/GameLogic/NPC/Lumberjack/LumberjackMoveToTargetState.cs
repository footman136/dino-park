using Assets.Gamelogic.Core;
using Assets.Gamelogic.FSM;
using Assets.Gamelogic.NPC.LumberJack;
using Assets.Gamelogic.Utils;
using Dinopark.Npc;
using UnityEngine;
using Improbable.Gdk.Core;

namespace Assets.Gamelogic.NPC.Lumberjack
{
    public class LumberjackMoveToTargetState : FsmBaseState<LumberjackStateMachine, LumberjackFSMState.StateEnum>
    {
        private readonly TargetNavigationWriter targetNavigation;
        private readonly LumberjackBehaviour parentBehaviour;
        private readonly TargetNavigationBehaviour navigation;
        
        private Coroutine interactionWithTargetDelayCoroutine;

        public LumberjackMoveToTargetState(LumberjackStateMachine owner,
                                           LumberjackBehaviour inParentBehaviour,
                                           TargetNavigationWriter inTargetNavigation,
                                           TargetNavigationBehaviour inNavigation)
            : base(owner)
        {
            targetNavigation = inTargetNavigation;
            parentBehaviour = inParentBehaviour;
            navigation = inNavigation;
        }

        public override void Enter()
        {
            targetNavigation.OnUpdate += (OnTargetNavigationUpdated);
            StartMovingTowardsTarget();
        }

        public override void Tick()
        {
        }

        public override void Exit(bool disabled)
        {
            targetNavigation.OnUpdate -= (OnTargetNavigationUpdated);
            StopInteractionWithTargetDelayCoroutine();
        }

        private void StopInteractionWithTargetDelayCoroutine()
        {
            if (interactionWithTargetDelayCoroutine != null)
            {
                parentBehaviour.StopCoroutine(interactionWithTargetDelayCoroutine);
                interactionWithTargetDelayCoroutine = null;
            }
        }

        private void StartMovingTowardsTarget()
        {
            if (TargetIsEntity())
            {
                StartMovingTowardsTargetEntity();
            }
            else
            {
                StartMovingTowardsTargetPosition();
            }
        }

        private void StartMovingTowardsTargetEntity()
        {
            var targetGameObject = NPCUtils.GetTargetGameObject(Owner.Data.TargetEntityId);
            if (targetGameObject == null)
            {
                Owner.TriggerTransition(LumberjackFSMState.StateEnum.IDLE, new EntityId(), SimulationSettings.InvalidPosition);
                return;
            }
            if (NPCUtils.IsWithinInteractionRange(parentBehaviour.transform.position, targetGameObject.transform.position, SimulationSettings.NPCDefaultInteractionSqrDistance))
            {
                InitiateInteractionWithTarget();
                return;
            }
            navigation.StartNavigation(Owner.Data.TargetEntityId, SimulationSettings.NPCDefaultInteractionSqrDistance);
        }

        private void InitiateInteractionWithTarget()
        {
            StopInteractionWithTargetDelayCoroutine();
            interactionWithTargetDelayCoroutine = parentBehaviour.StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.NPCInteractionDelay, AttemptInteractionWithTarget));
        }

        private void StartMovingTowardsTargetPosition()
        {
            var targetPosition = Owner.Data.TargetPosition.ToVector3();
            if (MathUtils.CompareEqualityEpsilon(targetPosition, SimulationSettings.InvalidPosition))
            {
                Owner.TriggerTransition(LumberjackFSMState.StateEnum.IDLE, new EntityId(), SimulationSettings.InvalidPosition);
                return;
            }
            if (NPCUtils.IsWithinInteractionRange(parentBehaviour.transform.position, targetPosition, SimulationSettings.NPCDefaultInteractionSqrDistance))
            {
                Owner.TriggerTransition(LumberjackFSMState.StateEnum.IDLE, new EntityId(), SimulationSettings.InvalidPosition);
                return;
            }
            navigation.StartNavigation(targetPosition, SimulationSettings.NPCDefaultInteractionSqrDistance);
        }

        private void OnTargetNavigationUpdated(TargetNavigation.Update update)
        {
            //if (update.navigationFinished.Count > 0)
            if(update.NavigationState == NavigationState.INACTIVE)
            {
                if (TargetIsEntity())
                {
                    InitiateInteractionWithTarget();
                }
                else
                {
                    Owner.TriggerTransition(LumberjackFSMState.StateEnum.IDLE, new EntityId(), SimulationSettings.InvalidPosition);
                }
            }
        }

        private void AttemptInteractionWithTarget()
        {
            var targetGameObject = NPCUtils.GetTargetGameObject(Owner.Data.TargetEntityId);
            if (targetGameObject == null)
            {
                Owner.TriggerTransition(LumberjackFSMState.StateEnum.IDLE, new EntityId(), SimulationSettings.InvalidPosition);
                return;
            }
            if (NPCUtils.IsTargetAHealthyTree(parentBehaviour.gameObject, targetGameObject) &&
                NPCUtils.IsWithinInteractionRange(parentBehaviour.transform.position, targetGameObject.transform.position, SimulationSettings.NPCDefaultInteractionSqrDistance))
            {
                Owner.TriggerTransition(LumberjackFSMState.StateEnum.HARVESTING, Owner.Data.TargetEntityId, SimulationSettings.InvalidPosition);
                return;
            }
            if (NPCUtils.IsTargetATeamStockpile(parentBehaviour.gameObject, targetGameObject) &&
                NPCUtils.IsWithinInteractionRange(parentBehaviour.transform.position, targetGameObject.transform.position, SimulationSettings.NPCDefaultInteractionSqrDistance))
            {
                Owner.TriggerTransition(LumberjackFSMState.StateEnum.STOCKPILING, Owner.Data.TargetEntityId, SimulationSettings.InvalidPosition);
                return;
            }
            Owner.TriggerTransition(LumberjackFSMState.StateEnum.IDLE, new EntityId(), SimulationSettings.InvalidPosition);
        }

        private bool TargetIsEntity()
        {
            return Owner.Data.TargetEntityId.IsValid();
        }
    }
}
