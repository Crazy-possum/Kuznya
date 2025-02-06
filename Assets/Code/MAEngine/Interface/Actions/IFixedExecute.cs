namespace MAEngine
{
    public interface IFixedExecute : IAction
    {
        public void FixedExecute(float fixedDeltaTime);
    }
}