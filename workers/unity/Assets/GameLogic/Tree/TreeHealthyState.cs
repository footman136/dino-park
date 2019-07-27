using GameLogic.Core; // SimulationSettings
using GameLogic.FSM;
using Dinopark.Fire; // flammable
using Dinopark.Life;
using Dinopark.Plants;

namespace GameLogic.Tree
{
    public class TreeHealthyState : FsmBaseState<TreeStateMachine, TreeFSMState>
    {
        private readonly FlammableWriter flammable;
        private readonly HealthWriter health;

        public TreeHealthyState(TreeStateMachine owner, FlammableWriter inFlammable, HealthWriter inHealth) 
            : base(owner)
        {
            flammable = inFlammable;
            health = inHealth;
        }

        public override void Enter()
        {
            var update = new Health.Update
            {
                CurrentHealth = SimulationSettings.TreeMaxHealth
            };
            health.SendUpdate(update);
            var update2 = new Flammable.Update
            {
                CanBeIgnited = true
            };
            flammable.SendUpdate(update2);

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
            if (update.CurrentHealth.HasValue && update.CurrentHealth.Value <= 0) 
            {
                Owner.TriggerTransition(TreeFSMState.STUMP);
            }
        }

        private void OnFlammableUpdated(Flammable.Update update)
        {
            if (HasBeenIgnited(update))
            {
                Owner.TriggerTransition(TreeFSMState.BURNING);
            }
        }

        private bool HasBeenIgnited(Flammable.Update flammableUpdate)
        {
            return flammableUpdate.IsOnFire.HasValue && flammableUpdate.IsOnFire.Value;
        }
    }
}
