using System.Collections.Generic;

namespace MAEngine
{
    public interface IModulesFactory
    {
        public List<IModule> GetModulesList();
    }
}