namespace GameLogic.FSM
{
    public interface IFsmState
    {
        void Enter();
        void Tick();
        void Exit(bool disabled);
    }
}