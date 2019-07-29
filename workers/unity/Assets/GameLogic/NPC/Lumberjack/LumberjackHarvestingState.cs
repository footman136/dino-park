using Assets.Gamelogic.ComponentExtensions;
using UnityEngine;
using Assets.Gamelogic.FSM;
using Dinopark.Core;
using Dinopark.Npc;
using Dinopark.Plants;
using Assets.Gamelogic.Core;
using Assets.Gamelogic.NPC.Lumberjack;
using Assets.Gamelogic.Utils;
using Improbable.Gdk.Core;
using Improbable.Worker.CInterop;

namespace Assets.Gamelogic.NPC.LumberJack
{
    public class LumberjackHarvestingState : FsmBaseState<LumberjackStateMachine, LumberjackFSMState.StateEnum>
    {
        private readonly LumberjackBehaviour parentBehaviour;
        private readonly InventoryWriter inventory;
        private readonly HarvestableCommandSender sender;

        private Coroutine harvestTreeDelayCoroutine;
        private Coroutine transitionToIdleDelayCoroutine;

        public LumberjackHarvestingState(LumberjackStateMachine owner,
                                         LumberjackBehaviour inParentBehaviour,
                                         InventoryWriter inInventory)
            : base(owner)
        {
            parentBehaviour = inParentBehaviour;
            inventory = inInventory;
        }

        public override void Enter()
        {
            if (!(inventory.HasResources()))
            {
                harvestTreeDelayCoroutine = parentBehaviour.StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.NPCChoppingAnimationStartDelay, AttemptToHarvestTree));
            }
            else
            {
                TransitionToIdle();
            }
        }

        public override void Tick()
        {
        }

        public override void Exit(bool disabled)
        {
            StopHarvestTreeDelayRoutine();
            StopTransitionToRoutine();
        }

        private void StopHarvestTreeDelayRoutine()
        {
            if (harvestTreeDelayCoroutine != null)
            {
                parentBehaviour.StopCoroutine(harvestTreeDelayCoroutine);
                harvestTreeDelayCoroutine = null;
            }
        }

        private void StopTransitionToRoutine()
        {
            if (transitionToIdleDelayCoroutine != null)
            {
                parentBehaviour.StopCoroutine(transitionToIdleDelayCoroutine);
                transitionToIdleDelayCoroutine = null;
            }
        }

        private void AttemptToHarvestTree()
        {
            var targetGameObject = NPCUtils.GetTargetGameObject(Owner.Data.TargetEntityId);
            if (targetGameObject != null && NPCUtils.IsTargetAHealthyTree(parentBehaviour.gameObject, targetGameObject))
            {
                Harvestable.Harvest.Request request = new Harvestable.Harvest.Request();
                request.TargetEntityId = Owner.Data.TargetEntityId;
                sender.SendHarvestCommand(request, OnHarvestResponse);
//                SpatialOS.Commands.SendCommand(inventory,
//                                               Harvestable.Commands.Harvest.Descriptor,
//                                               new HarvestRequest(parentBehaviour.gameObject.EntityId()),
//                                               Owner.Data.targetEntityId,
//                                               OnHarvestResponse,
//                                               new System.TimeSpan(0, 0, 5));
            }
            else
            {
                Owner.TriggerTransition(LumberjackFSMState.StateEnum.IDLE, new EntityId(), SimulationSettings.InvalidPosition);
            }
        }

        private void OnHarvestResponse(Harvestable.Harvest.ReceivedResponse response)
        {
            if (response.StatusCode != StatusCode.Success)
            {
                Debug.LogWarning("NPC failed to receive Harvest response");
            }
            else
            {
                //inventory.AddToInventory(response.Response.Value.resourcesTaken);
                var quantity = 1;
                var update = new Inventory.Update()
                {
                    Resources = inventory.Data.Resources + quantity
                };
                inventory.SendUpdate(update);
            }
            TransitionToIdle();
        }

        private void TransitionToIdle()
        {
            var waitAndPerfromTransition = TimerUtils.WaitAndPerform(SimulationSettings.NPCChoppingAnimationFinishDelay, () =>
            {
                Owner.TriggerTransition(LumberjackFSMState.StateEnum.IDLE, new EntityId(), SimulationSettings.InvalidPosition);
            });
            transitionToIdleDelayCoroutine = parentBehaviour.StartCoroutine(waitAndPerfromTransition);
        }
    }
}
