using UnityEngine; // Coroutine
using GameLogic.Core;
using GameLogic.Fire;
using GameLogic.FSM;
using GameLogic.Utils;
using Dinopark.Fire; // flammable
using Dinopark.Plants;

namespace GameLogic.Tree
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
