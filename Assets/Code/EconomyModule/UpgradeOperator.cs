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
                AddUpgrade(upgradeName, view, 0);
            }
        }

        public void Cleanup()
        {
            _economyEventBus.OnUpgradeBought -= AddUpgrade;
        }
        
        private void AddUpgrade(UpgradeName upgradeName, UpgradeView upgradeView, int upgradePrice)
        {
            if (upgradeView.CurrentLevel == 0)
            {
                return;
            }
            switch (upgradeName)
            {
                case UpgradeName.Ore :
                    SetProgressionMaterial(upgradeView);
                    break;
                case UpgradeName.Economy :
                    SetEconomyUpgrade(upgradeView);
                    break;
                case UpgradeName.Cost :
                    SetCostUpgrade(upgradeView);
                    break;
                case UpgradeName.FastMaterialIncome :
                    SetFastMaterialIncomeUpgrade(upgradeView);
                    break;
                case UpgradeName.ExtraDelivery :
                    SetExtraDeliveryUpgrade(upgradeView);
                    break;
                case UpgradeName.TradeSpeechcraft :
                    break;
                case UpgradeName.TradeDiplomacy :
                    break;
                case UpgradeName.ForgingEfficiency :
                    break;
                case UpgradeName.ForgingMastersExp :
                    break;
                case UpgradeName.ForgingScore :
                    break;
                case UpgradeName.ForgingMistakeScore :
                    break;
                case UpgradeName.MultiTask :
                    break;
                case UpgradeName.LargeRoom :
                    break;
                case UpgradeName.ClientsHolydays :
                    break;
                case UpgradeName.ClientsSign :
                    break;
                case UpgradeName.ClientsPrestige :
                    break;
                case UpgradeName.AutomaticWholesale :
                    break;
                case UpgradeName.WholesaleFrequency :
                    break;
                default :
                    Debug.Log("Upgrade in development");
                    break;
            }
            _upgradesMetaData.SetUpgradeLevel(upgradeName, upgradeView.CurrentLevel);
            _progressionEvents.OnProgressionDataChanged?.Invoke(_progressionData);
        }

        private void SetUpgradeValue(UpgradeView upgradeView, float value)
        {
            //Debug.Log($"level {upgradeView.CurrentLevel}");
            UpgradeName upgradeName = upgradeView.UpgradeConfig.Name;
            if (value == 0)
            {
                if (_upgradesMetaData.Upgrades.IsContainsKey(upgradeName))
                {
                    _upgradesMetaData.Upgrades[upgradeName].ActivateUpgrade();
                }
                else
                {
                    Upgrade upgrade = new Upgrade();
                    upgrade.ActivateUpgrade();
                    _upgradesMetaData.Upgrades.Add(upgradeName, upgrade);
                }
            }
            else
            {
                if (_upgradesMetaData.Upgrades.IsContainsKey(upgradeName))
                {
                    _upgradesMetaData.Upgrades[upgradeName].SetUpgradeModifier(value);
                }
                else
                {
                    Upgrade upgrade = new Upgrade();
                    upgrade.SetUpgradeModifier(value);
                    _upgradesMetaData.Upgrades.Add(upgradeName, upgrade);
                }
            }
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
                default:
                    Debug.Log($"Unexpected level {upgradeView.CurrentLevel}");
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

            float value = 0;
            SetUpgradeValue(upgradeView, value);
        }

        private void SetEconomyUpgrade(UpgradeView upgradeView)
        {
            float value = 0;
            switch (upgradeView.CurrentLevel)
            {
                case 1:
                    value = 0.1f;
                    break;
                case 2:
                    value = 0.2f;
                    break;
                case 3:
                    value = 0.3f;
                    break;
                case 4:
                    value = 0.4f;
                    break;
                default:
                    Debug.Log($"Unexpected level {upgradeView.CurrentLevel}");
                    break;
            }
            SetUpgradeValue(upgradeView, value);
        }
        
        private void SetCostUpgrade(UpgradeView upgradeView)
        {
            float value = 0;
            switch (upgradeView.CurrentLevel)
            {
                case 1:
                    value = 0.1f;
                    break;
                case 2:
                    value = 0.2f;
                    break;
                case 3:
                    value = 0.3f;
                    break;
                case 4:
                    value = 0.4f;
                    break;
                default:
                    Debug.Log($"Unexpected level {upgradeView.CurrentLevel}");
                    break;
            }
            SetUpgradeValue(upgradeView, value);
        }
        private void SetFastMaterialIncomeUpgrade(UpgradeView upgradeView)
        {
            float value = 0;
            SetUpgradeValue(upgradeView, value);
            PlayerMetaData playerMetaData = _progressionData.PlayerMetaData;
            int level = upgradeView.CurrentLevel;
            if (level < playerMetaData.StorageMaterialsConfig.MaterialAddingDeltaTime.Count)
            {
                playerMetaData.MaterialAddingDeltaTime =
                    playerMetaData.StorageMaterialsConfig.MaterialAddingDeltaTime[level];
            }
            else
            {
                level = playerMetaData.StorageMaterialsConfig.MaterialAddingDeltaTime.Count - 1;
                playerMetaData.MaterialAddingDeltaTime =
                    playerMetaData.StorageMaterialsConfig.MaterialAddingDeltaTime[level];
            }
        }
        private void SetExtraDeliveryUpgrade(UpgradeView upgradeView)
        {
            float value = 0;
            switch (upgradeView.CurrentLevel)
            {
                case 1:
                    value = 5;
                    break;
                case 2:
                    value = 3f;
                    break;
                case 3:
                    value = 2;
                    break;
                case 4:
                    value = 1.5f;
                    break;
                case 5:
                    value = 1;
                    break;
                default:
                    Debug.Log($"Unexpected level {upgradeView.CurrentLevel}");
                    break;
            }
            SetUpgradeValue(upgradeView, value);
        }
    }
}