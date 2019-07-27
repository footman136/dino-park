using GameLogic.Core; // SimulationSettings
using GameLogic.Utils;
using GameLogic.Fire;
using GameLogic.FSM;
using Dinopark.Fire; // flammable
using Dinopark.Plants;
using UnityEngine;

namespace GameLogic.Tree
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
