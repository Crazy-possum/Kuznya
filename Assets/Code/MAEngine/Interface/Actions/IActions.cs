using System.Collections.Generic;

namespace MAEngine
{
    public interface IActions
    {
        public List<IAction> ActionsList { get; }
        public List<IEnableAction> EnableActions { get; }
        public List<IPreInitialisation> PreInitialisations { get; }
        public List<IInitialisation> Initialisations { get; }
        public List<IExecute> Executes { get; }
        public List<IFixedExecute> FixedExecutes { get; }
        public List<ICleanUp> Cleanups { get; }

        public void ClearActions();
        public void Add(IAction action);
        public void AddEnableAction(IEnableAction enableAction);
        public void RemoveEnableAction(IEnableAction enableAction);
        public void AddPreInitialisation(IPreInitialisation preInitialisation);
        public void RemovePreInitialisation(IPreInitialisation preInitialisation);
        public void AddInitialisation(IInitialisation initialisation);
        public void RemoveInitialisation(IInitialisation initialisation);
        public void AddExecute(IExecute execute);
        public void RemoveExecute(IExecute execute);
        public void AddFixedExecute(IFixedExecute fixedExecute);
        public void RemoveFixedExecute(IFixedExecute fixedExecute);
        public void AddCleanup(ICleanUp cleanup);
        public void RemoveCleanup(ICleanUp cleanup);

        public void RunEnableActions();
        public void RunPreInitialisations();
        public void RunInitialisations();
        public void RunExecutes(float deltaTime);
        public void RunFixedExecutes(float fixedDeltaTime);
        public void RunCleanups();
    }
}