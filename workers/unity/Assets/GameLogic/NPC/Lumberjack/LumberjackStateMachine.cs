using Assets.Gamelogic.FSM;
using Assets.Gamelogic.NPC.Lumberjack;
using Assets.Gamelogic.Utils;
using Dinopark.Npc;
using System.Collections.Generic;
using UnityEngine;
using Improbable.Gdk.Core;
using Dinopark.Core;

namespace Assets.Gamelogic.NPC.LumberJack
{
    public class LumberjackStateMachine : FiniteStateMachine<LumberjackFSMState.StateEnum>
    {
        private readonly NPCLumberjackWriter npcLumberjack;
        public NPCLumberjackData Data;

        public LumberjackStateMachine(LumberjackBehaviour behaviour,
                                      TargetNavigationBehaviour navigation,
                                      InventoryWriter inventory,
                                      TargetNavigationWriter targetNavigation,
                                      NPCLumberjackWriter inNpcLumberjack/*,
                                      TeamAssignment.Reader teamAssignment*/)
        {
            npcLumberjack = inNpcLumberjack;

            var idleState = new LumberjackIdleState(this, behaviour/*, teamAssignment*/, inventory);
            var moveToTargetState = new LumberjackMoveToTargetState(this, behaviour, targetNavigation, navigation);
            var harvestingState = new LumberjackHarvestingState(this, behaviour, inventory);
            var stockPilingState = new LumberjackStockpilingState(this, behaviour, inventory);
            var onFireState = new LumberjackOnFireState(this, navigation, targetNavigation);

            var stateList = new Dictionary<LumberjackFSMState.StateEnum, IFsmState>
            {
                { LumberjackFSMState.StateEnum.IDLE, idleState },
                { LumberjackFSMState.StateEnum.MOVING_TO_TARGET, moveToTargetState },
                { LumberjackFSMState.StateEnum.HARVESTING, harvestingState },
                { LumberjackFSMState.StateEnum.STOCKPILING, stockPilingState },
                { LumberjackFSMState.StateEnum.ON_FIRE, onFireState }
            };

            SetStates(stateList);

            var allowedTransitions = new Dictionary<LumberjackFSMState.StateEnum, IList<LumberjackFSMState.StateEnum>>();

            allowedTransitions.Add(LumberjackFSMState.StateEnum.IDLE, new List<LumberjackFSMState.StateEnum>
                                   {
                                       LumberjackFSMState.StateEnum.MOVING_TO_TARGET,
                                       LumberjackFSMState.StateEnum.ON_FIRE
                                   });

            allowedTransitions.Add(LumberjackFSMState.StateEnum.MOVING_TO_TARGET, new List<LumberjackFSMState.StateEnum>
                                   {
                                       LumberjackFSMState.StateEnum.IDLE,
                                       LumberjackFSMState.StateEnum.HARVESTING,
                                       LumberjackFSMState.StateEnum.STOCKPILING,
                                       LumberjackFSMState.StateEnum.ON_FIRE
                                   });

            allowedTransitions.Add(LumberjackFSMState.StateEnum.HARVESTING, new List<LumberjackFSMState.StateEnum>
                                   {
                                       LumberjackFSMState.StateEnum.IDLE,
                                       LumberjackFSMState.StateEnum.ON_FIRE
                                   });

            allowedTransitions.Add(LumberjackFSMState.StateEnum.STOCKPILING, new List<LumberjackFSMState.StateEnum>
                                   {
                                       LumberjackFSMState.StateEnum.IDLE,
                                       LumberjackFSMState.StateEnum.ON_FIRE
                                   });

            allowedTransitions.Add(LumberjackFSMState.StateEnum.ON_FIRE, new List<LumberjackFSMState.StateEnum>
                                   {
                                       LumberjackFSMState.StateEnum.IDLE,
                                       LumberjackFSMState.StateEnum.ON_FIRE
                                   });

            SetTransitions(allowedTransitions);
        }

        public void TriggerTransition(LumberjackFSMState.StateEnum newState, EntityId targetEntityId, Vector3 targetPosition)
        {
            if (npcLumberjack == null)
            {
                Debug.LogError("Trying to change state without authority.");
                return;
            }

            if (IsValidTransition(newState))
            {
                Data.CurrentState = newState;
                Data.TargetEntityId = targetEntityId;
                Data.TargetPosition = targetPosition.FlattenVector().ToVector3f();

                var update = new NPCLumberjack.Update()
                {
                    CurrentState = Data.CurrentState,
                    TargetEntityId = Data.TargetEntityId,
                    TargetPosition = Data.TargetPosition
                };
                npcLumberjack.SendUpdate(update);

                TransitionTo(newState);
            }
            else
            {
                Debug.LogErrorFormat("NPCLumberjack: Invalid transition from {0} to {1} detected.", Data.CurrentState, newState);
            }
        }

        protected override void OnEnableImpl()
        {
            Data.CurrentState = npcLumberjack.Data.CurrentState;
            Data.TargetEntityId = npcLumberjack.Data.TargetEntityId;
            Data.TargetPosition = npcLumberjack.Data.TargetPosition;
        }
    }
}
