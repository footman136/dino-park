using Assets.Gamelogic.Core;
using Assets.Gamelogic.Fire;
using Assets.Gamelogic.FSM;
using Assets.Gamelogic.Utils;
using Dinopark.Fire;
using Dinopark.Plants;
using UnityEngine;
// SimulationSettings
// flammable

namespace Assets.Gamelogic.Tree
{
    public class TreeBurntState : FsmBaseState<TreeStateMachine, TreeFSMState>
    {
        private readonly TreeBehaviour parentBehaviour;
        private readonly FlammableWriter flammable;
        private readonly FlammableBehaviour flammableInterface;

        private Coroutine regrowingCoroutine;

        public TreeBurntState(TreeStateMachine owner, TreeBehaviour inParentBehaviour, FlammableWriter inFlammable, FlammableBehaviour inflammableInterface) : base(owner)
        {
            parentBehaviour = inParentBehaviour;
            flammable = inFlammable;
            flammableInterface = inflammableInterface;
        }

        public override void Enter()
        {
            flammableInterface.SelfExtinguish();
            if (regrowingCoroutine == null)
            {
                regrowingCoroutine = parentBehaviour.StartCoroutine(TimerUtils.WaitAndPerform(SimulationSettings.BurntTreeRegrowthTimeSecs, Regrow));
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
