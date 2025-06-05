using GameCoreModule;
using MAEngine;
using Progression;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Audio
{
    public class GameAudioActions : IAction, IInitialisation
    {
        private AudioEventBus _audioEventBus;
        private ProgressionData _progressionData;
        private EconomyEventBus _economyEventBus;

        [Inject]
        public void Construct(AudioEventBus audioEventBus, ProgressionData progressionData, EconomyEventBus economyEventBus)
        {
            _audioEventBus = audioEventBus;
            _progressionData = progressionData;
            _economyEventBus = economyEventBus;
        }
        public void Initialisation()
        {
            _economyEventBus.OnUpgradeApplied += CheckSecretUpgradeApplied;
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                _audioEventBus.OnPlayMusic?.Invoke(AudioResourceID.Music_Menu);
            }
            else
            {
                if (!_progressionData.UpgradesMeta.Upgrades.IsContainsKey(UpgradeName.SecretUpgrade))
                {
                    _audioEventBus.OnPlayMusic?.Invoke(AudioResourceID.Music_InGame_1);
                }
                else
                {
                    _audioEventBus.OnPlayMusic?.Invoke(AudioResourceID.Music_Secret);
                }
                
            } 
        }

        private void CheckSecretUpgradeApplied(UpgradeName upgradeName)
        {
            if (upgradeName == UpgradeName.SecretUpgrade)
            {
                _audioEventBus.OnPlayMusic?.Invoke(AudioResourceID.Music_Secret);
            }
        }
    }
}