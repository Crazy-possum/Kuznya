using System.Collections.Generic;
using UnityEngine;

namespace MAEngine
{
    public class ModulesFactory : MonoBehaviour, IModulesFactory
    {
        public List<BasicModule> Modules;
        public List<IModule> GetModulesList()
        {
            List<IModule> modulesList = new List<IModule>();
            foreach (var module in Modules)
            {
                module.Initialise();
                modulesList.Add(module);
            }
            return modulesList;
        }
    }
}


