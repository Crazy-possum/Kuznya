using MAEngine;

namespace Audio
{
    public class AudioModule : BasicModule
    {
        public override void Initialise()
        {
            base.Initialise();
            InitializeAudioOperator();
            InitializeAudioActions();
        }

        private void InitializeAudioOperator()
        {
            AudioOperator audioOperator =
                _di.Resolve<AudioOperator>();
            _actions.Add(audioOperator);
        }
        
        private void InitializeAudioActions()
        {
            GameAudioActions gameAudioActions =
                _di.Resolve<GameAudioActions>();
            _actions.Add(gameAudioActions);
        }
    }
}