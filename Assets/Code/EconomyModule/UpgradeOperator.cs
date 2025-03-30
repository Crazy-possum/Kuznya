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
        private ShopView _shopView;
        private UpgradesPool _upgradesPool;
        
        private UpgradesMetaData _upgradesMetaData;
        
        [Inject]
        public void Construct(EconomyEventBus economyEventBus, ProgressionData progressionData,
            ProgressionEvents progressionEvents, ShopView shopView, UpgradesPool upgradesPool)
        {
            _economyEventBus = economyEventBus;
            _progressionData = progressionData;
            _progressionEvents = progressionEvents;
            _shopView = shopView;
            _upgradesPool = upgradesPool;
        }
        
        public void Initialisation()
        {
            _upgradesMetaData = _progressionData.UpgradesMeta;
            _economyEventBus.OnUpgradeBought += AddUpgrade;
            LoadUpgrades();
        }

        private void LoadUpgrades()
        {
            foreach (UpgradeConfig config in _upgradesPool.UpgradesList)
            {
                UpgradeName upgradeName = config.Name;
                UpgradeView view = _shopView.UpgradeViews.GetValue(upgradeName);
                view.SetLevel(_upgradesMetaData.GetUpgradeLevel(upgradeName));
            }
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
            _upgradesMetaData.SetUpgradeLevel(upgradeName, upgradeView.CurrentLevel);
            _progressionEvents.OnProgressionDataChanged?.Invoke(_progressionData);
        }

        private void SetProgressionMaterial(UpgradeView upgradeView)
        {
            MaterialName material = MaterialName.NONE;
            switch (upgradeView.CurrentLevel)
            {
                case 0 :
                    material = MaterialName.Metal;
                    break;
                case 1 :
                    material = MaterialName.Tin;
                    break;
                case 2 :
                    material = MaterialName.Copper;
                    break;
                case 3 :
                    material = MaterialName.Iron;
                    break;
                case 4 :
                    material = MaterialName.Steel;
                    break;
                case 5 :
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