using GameCoreModule;
using MAEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Audio
{
    public class GameAudioActions : IAction, IInitialisation
    {
        private AudioEventBus _audioEventBus;

        [Inject]
        public void Construct(AudioEventBus audioEventBus)
        {
            _audioEventBus = audioEventBus;
        }
        public void Initialisation()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                _audioEventBus.OnPlayMusic?.Invoke(AudioResourceID.Music_Menu);
            }
            else
            {
                _audioEventBus.OnPlayMusic?.Invoke(AudioResourceID.Music_InGame_1);
            } 
        }
    }
}