using Assets.Gamelogic.Core;
using Assets.Gamelogic.FSM;
using Assets.Gamelogic.NPC.LumberJack;
using Dinopark.Npc;

namespace Assets.Gamelogic.NPC.Lumberjack
{
    public class LumberjackOnFireState : FsmBaseState<LumberjackStateMachine, LumberjackFSMState.StateEnum>
    {
        private readonly TargetNavigationBehaviour navigation;
        private readonly TargetNavigationWriter targetNavigation;

        public LumberjackOnFireState(LumberjackStateMachine owner,
                                     TargetNavigationBehaviour inNavigation,
                                     TargetNavigationWriter inTargetNavigation)
            : base(owner)
        {
            navigation = inNavigation;
            targetNavigation = inTargetNavigation;
        }

        public override void Enter()
        {
            targetNavigation.OnUpdate += (OnTargetNavigationUpdated);
            NPCUtils.NavigateToRandomNearbyPosition(navigation, navigation.transform.position, SimulationSettings.NPCOnFireWaypointDistance, SimulationSettings.NPCDefaultInteractionSqrDistance);
        }

        public override void Tick()
        {
        }

        public override void Exit(bool disabled)
        {
            targetNavigation.OnUpdate -= (OnTargetNavigationUpdated);
            if (!disabled)
            {
                navigation.StopNavigation();
            }
        }

        private void OnTargetNavigationUpdated(TargetNavigation.Update update)
        {
            //if (update.navigationFinished.Count > 0)
            if (update.NavigationState == NavigationState.INACTIVE)
            {
                NPCUtils.NavigateToRandomNearbyPosition(navigation, navigation.transform.position, SimulationSettings.NPCOnFireWaypointDistance, SimulationSettings.NPCDefaultInteractionSqrDistance);
            }
        }
    }
}
