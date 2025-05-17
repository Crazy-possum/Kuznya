using MAEngine;
using System;

namespace Progression
{
    public class ProgressionModule : BasicModule
    {
        public override void Initialise()
        {
            InitializeFields();
            InitializeProgressionOperator();
            InitializeStagesCheckActions();
            InitializeTutorialActions();
        }

        private void InitializeFields()
        {
            _actions = new Actions();
        }

        private void InitializeProgressionOperator()
        {
            ProgressionOperator progressionOperator =
                _di.Resolve<ProgressionOperator>();
            _actions.Add(progressionOperator);
        }
        
        private void InitializeStagesCheckActions()
        {
            StagesCheckActions stagesCheckActions =
                _di.Resolve<StagesCheckActions>();
            _actions.Add(stagesCheckActions);
        }
        
        private void InitializeTutorialActions()
        {
            TutorialActions tutorialActions =
                _di.Resolve<TutorialActions>();
            _actions.Add(tutorialActions);
        }
        
    }
}
