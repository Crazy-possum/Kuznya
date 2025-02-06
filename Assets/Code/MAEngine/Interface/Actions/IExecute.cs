namespace MAEngine
{
    public interface IExecute : IAction
    {
        public void Execute(float deltaTime);
    }
}