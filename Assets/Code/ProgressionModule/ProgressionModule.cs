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
    }
}
