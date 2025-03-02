using MAEngine;
using UnityEngine;

namespace Core
{
    public class GameModulesFactory : ModulesFactory
    {
        private void Start()
        {
            //DebugModeActions();
        }

        private void DebugModeActions()
        {
            Debug.Log("Modules debug mode ON");
            Debug.Log("Modules list : ");
            foreach (IModule module in Modules)
            {
                Debug.Log(module);

                if (module.Actions.ActionsList.Count != 0)
                {
                    Debug.Log("Actions : ");
                    foreach (IAction action in module.Actions.ActionsList)
                    {
                        Debug.Log(action);
                    }
                }
                else
                {
                    Debug.Log("No actions");
                }
            }
        }
    }
}

