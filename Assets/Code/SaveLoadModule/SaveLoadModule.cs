using MAEngine;

namespace SaveLoad
{
    public class SaveLoadModule : BasicModule
    {
        public override void Initialise()
        {
            base.Initialise();
            
            InitializeSaveLoadActions();
        }

        private void InitializeSaveLoadActions()
        {
            SaveLoadActions saveLoadActions =
                _di.Resolve<SaveLoadActions>();
            _actions.Add(saveLoadActions);
        }
    }
}

