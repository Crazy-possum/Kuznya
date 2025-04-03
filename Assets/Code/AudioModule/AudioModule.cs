using MAEngine;

namespace Audio
{
    public class AudioModule : BasicModule
    {
        public override void Initialise()
        {
            base.Initialise();
            InitializeAudioOperator();
        }

        private void InitializeAudioOperator()
        {
            AudioOperator audioOperator =
                _di.Resolve<AudioOperator>();
            _actions.Add(audioOperator);
        }
    }
}