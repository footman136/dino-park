namespace GameLogic.Fire
{
    public interface IFlammable
    {
        void OnIgnite();
        void OnExtinguish();
    }
}