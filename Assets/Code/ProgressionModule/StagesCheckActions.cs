using GameCoreModule;
using MAEngine;
using UnityEngine;

namespace Progression
{
    public class StagesCheckActions : IAction, IInitialisation, ICleanUp
    {
        private ProgressionData _data;
        private EconomyEventBus _economyEventBus;

        private PlayerMetaData _playerMetaData;
        private UpgradesMetaData _upgradesMetaData;

        public StagesCheckActions(ProgressionData data, EconomyEventBus economyEventBus)
        {
            _data = data;
            _economyEventBus = economyEventBus;
        }

        public void Initialisation()
        {
            _playerMetaData = _data.PlayerMetaData;
            _upgradesMetaData = _data.UpgradesMeta;
            CheckUnlockedStages();
            _economyEventBus.OnUpgradeApplied += CheckNewUpgrade;
        }

        public void Cleanup()
        {
            _economyEventBus.OnUpgradeApplied -= CheckNewUpgrade;
        }

        private void CheckNewUpgrade(UpgradeName upgradeName)
        {
            if (upgradeName == UpgradeName.AdditionalSteps)
            {
                CheckUnlockedStages();
            }
        }

        private void CheckUnlockedStages()
        {
            _playerMetaData.Unlocks = new Unlocks();
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.AdditionalSteps))
            {
                int stagesNumber = (int)_upgradesMetaData.Upgrades[UpgradeName.AdditionalSteps].GetUpgradeData();
                if (stagesNumber == 1)
                {
                    _playerMetaData.Unlocks.IsSmeltingUnlocked = true;
                    _playerMetaData.Unlocks.IsHardeningUnlocked = false;
                    _playerMetaData.Unlocks.IsSharpeningUnlocked = false;
                    _playerMetaData.Unlocks.IsTradeUnlocked = false;
                }
                else if (stagesNumber == 2)
                {
                    _playerMetaData.Unlocks.IsSmeltingUnlocked = true;
                    _playerMetaData.Unlocks.IsHardeningUnlocked = true;
                    _playerMetaData.Unlocks.IsSharpeningUnlocked = false;
                    _playerMetaData.Unlocks.IsTradeUnlocked = false;
                }
                else if (stagesNumber == 3)
                {
                    _playerMetaData.Unlocks.IsSmeltingUnlocked = true;
                    _playerMetaData.Unlocks.IsHardeningUnlocked = true;
                    _playerMetaData.Unlocks.IsSharpeningUnlocked = true;
                    _playerMetaData.Unlocks.IsTradeUnlocked = false;
                }
                else if (stagesNumber == 4)
                {
                    _playerMetaData.Unlocks.IsSmeltingUnlocked = true;
                    _playerMetaData.Unlocks.IsHardeningUnlocked = true;
                    _playerMetaData.Unlocks.IsSharpeningUnlocked = true;
                    _playerMetaData.Unlocks.IsTradeUnlocked = true;
                }
            }
        }
    }
}