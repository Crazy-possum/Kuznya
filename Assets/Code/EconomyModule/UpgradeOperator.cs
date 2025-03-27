using GameCoreModule;
using MAEngine;
using Progression;
using UnityEngine;
using Zenject;

namespace Economy
{
    public class UpgradeOperator : IAction, IInitialisation, ICleanUp
    {
        private EconomyEventBus _economyEventBus;
        private ProgressionData _progressionData;
        private ProgressionEvents _progressionEvents;
        
        [Inject]
        public void Construct(EconomyEventBus economyEventBus, ProgressionData progressionData,
            ProgressionEvents progressionEvents)
        {
            _economyEventBus = economyEventBus;
            _progressionData = progressionData;
            _progressionEvents = progressionEvents;
        }
        
        public void Initialisation()
        {
            _economyEventBus.OnUpgradeBought += AddUpgrade;
        }
        
        public void Cleanup()
        {
            _economyEventBus.OnUpgradeBought -= AddUpgrade;
        }
        
        private void AddUpgrade(UpgradeName upgradeName, UpgradeView upgradeView, int upgradePrice)
        {
            switch (upgradeName)
            {
                case UpgradeName.Ore :
                    SetProgressionMaterial(upgradeView);
                    break;
                default :
                    Debug.Log("Upgrade in development");
                    break;
            }
            _progressionEvents.OnProgressionDataChanged?.Invoke(_progressionData);
        }

        private void SetProgressionMaterial(UpgradeView upgradeView)
        {
            MaterialName material = MaterialName.NONE;
            switch (upgradeView.CurrentLevel)
            {
                case 0 :
                    material = MaterialName.Tin;
                    break;
                case 1 :
                    material = MaterialName.Copper;
                    break;
                case 2 :
                    material = MaterialName.Iron;
                    break;
                case 3 :
                    material = MaterialName.Steel;
                    break;
                case 4 :
                    material = MaterialName.Silver;
                    break;
                default :
                    break;
            }

            if (material != MaterialName.NONE)
            {
                _progressionData.PlayerMetaData.CurrentMaximumMaterial = material;
            }
            else
            {
                Debug.Log("Material not found");
            }

        }
    }
}