using System.Collections.Generic;

namespace MAEngine
{
    public class Actions : IActions
    {
        private List<IEnableAction> _enableActions;
        private List<IPreInitialisation> _preInitialisations;
        private List<IInitialisation> _initialisations;
        private List<IExecute> _executes;
        private List<IFixedExecute> _fixedExecutes;
        private List<ICleanUp> _cleanups;
        private List<IAction> _actionsList;

        public List<IAction> ActionsList
        {
            get => _actionsList;
        }

        public List<IEnableAction> EnableActions
        {
            get => _enableActions;
        }

        public List<IPreInitialisation> PreInitialisations
        {
            get => _preInitialisations;
        }

        public List<IInitialisation> Initialisations
        {
            get => _initialisations;
        }

        public List<IExecute> Executes
        {
            get => _executes;
        }

        public List<IFixedExecute> FixedExecutes
        {
            get => _fixedExecutes;
        }

        public List<ICleanUp> Cleanups
        {
            get => _cleanups;
        }

        internal Actions()
        {
            InitializeLists();
        }

        public void ClearActions()
        {
            InitializeLists();
        }

        public void Add(IAction action)
        {
            _actionsList.Add(action);
            if (action is IEnableAction)
            {
                AddEnableAction((IEnableAction)action);
            }
            if (action is IPreInitialisation)
            {
                AddPreInitialisation((IPreInitialisation)action);
            }
            if (action is IInitialisation)
            {
                AddInitialisation((IInitialisation)action);
            }
            if (action is IExecute)
            {
                AddExecute((IExecute)action);
            }
            if (action is IFixedExecute)
            {
                AddFixedExecute((IFixedExecute)action);
            }
            if (action is ICleanUp)
            {
                AddCleanup((ICleanUp)action);
            }
        }
        
        public void Remove(IAction action)
        {
            _actionsList.Remove(action);
            if (action.GetType() == typeof(IEnableAction))
            {
                RemoveEnableAction((IEnableAction)action);
            }
            if (action.GetType() == typeof(IPreInitialisation))
            {
                RemovePreInitialisation((IPreInitialisation)action);
            }
            if (action.GetType() == typeof(IInitialisation))
            {
                RemoveInitialisation((IInitialisation)action);
            }
            if (action.GetType() == typeof(IExecute))
            {
                RemoveExecute((IExecute)action);
            }
            if (action.GetType() == typeof(IFixedExecute))
            {
                RemoveFixedExecute((IFixedExecute)action);
            }
            if (action.GetType() == typeof(ICleanUp))
            {
                RemoveCleanup((ICleanUp)action);
            }
        }

        public void AddEnableAction(IEnableAction enableAction)
        {
            _enableActions.Add(enableAction);
        }

        public void RemoveEnableAction(IEnableAction enableAction)
        {
            _enableActions.Remove(enableAction);
        }

        public void AddPreInitialisation(IPreInitialisation preInitialisation)
        {
            _preInitialisations.Add(preInitialisation);
        }

        public void RemovePreInitialisation(IPreInitialisation preInitialisation)
        {
            _preInitialisations.Remove(preInitialisation);
        }

        public void AddInitialisation(IInitialisation initialisation)
        {
            _initialisations.Add(initialisation);
        }

        public void RemoveInitialisation(IInitialisation initialisation)
        {
            _initialisations.Remove(initialisation);
        }

        public void AddExecute(IExecute execute)
        {
            _executes.Add(execute);
        }

        public void RemoveExecute(IExecute execute)
        {
            _executes.Remove(execute);
        }

        public void AddFixedExecute(IFixedExecute fixedExecute)
        {
            _fixedExecutes.Add(fixedExecute);
        }

        public void RemoveFixedExecute(IFixedExecute fixedExecute)
        {
            _fixedExecutes.Remove(fixedExecute);
        }

        public void AddCleanup(ICleanUp cleanup)
        {
            _cleanups.Add(cleanup);
        }

        public void RemoveCleanup(ICleanUp cleanup)
        {
            _cleanups.Remove(cleanup);
        }

        public void RunEnableActions()
        {
            foreach (IEnableAction action in _enableActions)
            {
                action.EnableAction();
            }
        }

        public void RunPreInitialisations()
        {
            foreach (IPreInitialisation action in _preInitialisations)
            {
                action.PreInitialisation();
            }
        }

        public void RunInitialisations()
        {
            foreach (IInitialisation action in _initialisations)
            {
                action.Initialisation();
            }
        }

        public void RunExecutes(float deltaTime)
        {
            foreach (IExecute action in _executes)
            {
                action.Execute(deltaTime);
            }
        }

        public void RunFixedExecutes(float fixedDeltaTime)
        {
            foreach (IFixedExecute action in _fixedExecutes)
            {
                action.FixedExecute(fixedDeltaTime);
            }
        }

        public void RunCleanups()
        {
            foreach (ICleanUp action in _cleanups)
            {
                action.Cleanup();
            }
        }

        private void InitializeLists()
        {
            _actionsList = new List<IAction>();
            _enableActions = new List<IEnableAction>();
            _preInitialisations = new List<IPreInitialisation>();
            _initialisations = new List<IInitialisation>();
            _executes = new List<IExecute>();
            _fixedExecutes = new List<IFixedExecute>();
            _cleanups = new List<ICleanUp>();
        }
        
    }

}