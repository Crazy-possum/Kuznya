using GameCoreModule;
using UnityEngine;
using Zenject;

namespace MainGUI
{
    public class SoundButtonView : ButtonView 
    {
        [SerializeField] private AudioResourceID _audioResourceID;
        private AudioEventBus _audioEventBus;
        
        [Inject]
        public void Construct(AudioEventBus audioEventBus)
        {
            _audioEventBus = audioEventBus;
        }

        private void Start()
        {
            Button.onClick.AddListener(OnButtonClick);
        }
        
        private void OnDestroy()
        {
            Button.onClick.RemoveListener(OnButtonClick);
        }

        public void OnButtonClick()
        {
            if (_audioResourceID != AudioResourceID.NONE)
            {
                _audioEventBus.OnPlaySound?.Invoke(_audioResourceID);
            }
        }
    }
}