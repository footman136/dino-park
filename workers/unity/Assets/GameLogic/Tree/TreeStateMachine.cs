using System.Collections.Generic;
using Assets.Gamelogic.Fire;
using Assets.Gamelogic.FSM;
using Dinopark.Fire;
using Dinopark.Life;
using Dinopark.Plants;
using UnityEngine;
// flammable

namespace Assets.Gamelogic.Tree
{
    public class TreeStateMachine : FiniteStateMachine<TreeFSMState>
    {
        private readonly TreeStateWriter tree;
        public TreeStateData Data;

        public TreeStateMachine(
              TreeBehaviour owner,
              TreeStateWriter inTree,
              HealthWriter health,
              FlammableBehaviour flammableInterface,
              FlammableWriter flammable
        )
        {
            tree = inTree;

            var healthyState = new TreeHealthyState(this, flammable, health);
            var burningState = new TreeBurningState(this, flammable, health);
            var burntState = new TreeBurntState(this, owner, flammable, flammableInterface);
            var stumpState = new TreeStumpState(this, owner, flammable);

            var stateList = new Dictionary<TreeFSMState, IFsmState>();
            stateList.Add(TreeFSMState.HEALTHY, healthyState);
            stateList.Add(TreeFSMState.BURNING, burningState);
            stateList.Add(TreeFSMState.BURNT, burntState);
            stateList.Add(TreeFSMState.STUMP, stumpState);

            SetStates(stateList);

            var allowedTransitions = new Dictionary<TreeFSMState, IList<TreeFSMState>>();

            allowedTransitions.Add(TreeFSMState.HEALTHY, new List<TreeFSMState>()
            {
                TreeFSMState.BURNING,
                TreeFSMState.STUMP
            });

            allowedTransitions.Add(TreeFSMState.BURNING, new List<TreeFSMState>()
            {
                TreeFSMState.HEALTHY,
                TreeFSMState.BURNT
            });

            allowedTransitions.Add(TreeFSMState.BURNT, new List<TreeFSMState>()
            {
                TreeFSMState.HEALTHY
            });

            allowedTransitions.Add(TreeFSMState.STUMP, new List<TreeFSMState>()
            {
                TreeFSMState.HEALTHY
            });

            SetTransitions(allowedTransitions);
        }

        protected override void OnEnableImpl()
        {
            Data.TreeType = tree.Data.TreeType;
            Data.CurrentState = tree.Data.CurrentState;
        }

        public void TriggerTransition(TreeFSMState newState)
        {
            if (tree == null)
            {
                Debug.LogError("Trying to change state without authority.");
                return;
            }

            if (IsValidTransition(newState))
            {
                Data.CurrentState = newState;

                var update = new TreeState.Update()
                {
                    CurrentState = Data.CurrentState,
                };
                tree.SendUpdate(update);

                TransitionTo(newState);
            }
            else
            {
                Debug.LogErrorFormat("Tree: Invalid transition from {0} to {1} detected.", Data.CurrentState, newState);
            }
        }
    }

}
