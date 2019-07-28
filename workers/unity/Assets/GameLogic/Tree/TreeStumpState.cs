using Assets.Gamelogic.Utils;
using Assets.Gamelogic.Core;
using Assets.Gamelogic.FSM;
using Dinopark.Fire;
using Dinopark.Plants;
using UnityEngine;
// Coroutine
// flammable

namespace Assets.Gamelogic.Tree
{
    public class TreeStumpState : FsmBaseState<TreeStateMachine, TreeFSMState>
    {
        private readonly TreeBehaviour parentBehaviour;
        private readonly FlammableWriter flammable;

        private Coroutine regrowingCoroutine;

        public TreeStumpState(TreeStateMachine owner, TreeBehaviour inParentBehaviour, FlammableWriter inFlammable) : base(owner)
        {
            parentBehaviour = inParentBehaviour;
            flammable = inFlammable;
        }

        public override void Enter()
        {
            var update = new Flammable.Update()
            {
                CanBeIgnited = false
            };
            flammable.SendUpdate(update);

            if (regrowingCoroutine == null)
            {
                regrowingCoroutine = parentBehaviour.StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.TreeStumpRegrowthTimeSecs, Regrow));
            }
        }

        private void Regrow()
        {
            Owner.TriggerTransition(TreeFSMState.HEALTHY);
        }

        public override void Tick()
        {

        }

        public override void Exit(bool disabled)
        {
            if (regrowingCoroutine != null)
            {
                parentBehaviour.StopCoroutine(regrowingCoroutine);
                regrowingCoroutine = null;
            }
        }
    }
}
