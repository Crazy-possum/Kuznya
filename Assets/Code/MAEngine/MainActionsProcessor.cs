using System.Collections.Generic;
using UnityEngine;

namespace MAEngine
{
    public class MainActionsProcessor : MonoBehaviour
    {
        public ModulesFactory _modulesFactory;
        private List<IModule> _modules;
        private List<IActions> _actions;

        public List<IModule> Modules
        {
            get => _modules;
        }

        private void Awake()
        {
            _modules = _modulesFactory.GetModulesList();
            _actions = new List<IActions>();
            foreach (IModule module in _modules)
            {
                _actions.Add(module.Actions);
            }
            foreach (IActions actions in _actions)
            {
                actions.RunPreInitialisations();
            }
        }
        
        private void OnEnable()
        {
            foreach (IActions actions in _actions)
            {
                actions.RunEnableActions();
            }
        }

        private void Start()
        {
            foreach (IActions actions in _actions)
            {
                actions.RunInitialisations();
            }
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            foreach (IActions actions in _actions)
            {
                actions.RunExecutes(deltaTime);
            }
        }

        private void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;
            foreach (IActions actions in _actions)
            {
                actions.RunFixedExecutes(fixedDeltaTime);
            }
        }

        private void OnDisable()
        {
            foreach (IActions actions in _actions)  
            {
                actions.RunCleanups();
            }
        }
    }
}
