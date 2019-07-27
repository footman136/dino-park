using GameLogic.FSM;
using Dinopark.Fire; // flammable
using Dinopark.Life;
using Dinopark.Plants;

namespace GameLogic.Tree
{
    public class TreeBurningState : FsmBaseState<TreeStateMachine, TreeFSMState>
    {
        private readonly FlammableWriter flammable;
        private readonly HealthWriter health;

        public TreeBurningState(TreeStateMachine owner, FlammableWriter inFlammable, HealthWriter inHealth) 
            : base(owner)
        {
            flammable = inFlammable;
            health = inHealth;
        }

        public override void Enter()
        {
            var update = new Flammable.Update()
            {
                CanBeIgnited = false
            };
            flammable.SendUpdate(update);

            flammable.OnUpdate += (OnFlammableUpdated);
            health.OnUpdate += (OnHealthUpdated);
        }

        public override void Tick()
        {

        }

        public override void Exit(bool disabled)
        {
            health.OnUpdate -= (OnHealthUpdated);
            flammable.OnUpdate -= (OnFlammableUpdated);
        }

        private void OnHealthUpdated(Health.Update update)
        {
            if (update.CanBeChanged.HasValue && update.CurrentHealth.Value <= 0)
            {
                Owner.TriggerTransition(TreeFSMState.BURNT);
            }
        }

        private void OnFlammableUpdated(Flammable.Update update)
        {
            if (HasBeenExtinguished(update))
            {
                Owner.TriggerTransition(TreeFSMState.HEALTHY);
            }
        }

        private bool HasBeenExtinguished(Flammable.Update flammableUpdate)
        {
            return flammableUpdate.IsOnFire.HasValue && !flammableUpdate.IsOnFire.Value;
        }
    }
}
