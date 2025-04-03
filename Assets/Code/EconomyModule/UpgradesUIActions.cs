using System.Collections.Generic;
using GameCoreModule;
using MAEngine;
using Progression;
using UnityEngine;
using Zenject;

namespace Economy
{
    public class UpgradesUIActions : IAction, IInitialisation, ICleanUp
    {
        private ShopView _shopView;
        private UpgradesPool _upgradesPool;
        private EconomyEventBus _economyEventBus;
        private ProgressionData _progressionData;
        
        private UpgradesMetaData _upgradesMetaData;
        private List<UpgradeView> _upgradeViews;

        [Inject]
        public void Construct(ShopView shopView, UpgradesPool upgradesPool, EconomyEventBus economyEventBus,
            ProgressionData data)
        {
            _shopView = shopView;
            _upgradesPool = upgradesPool;
            _economyEventBus = economyEventBus;
            _progressionData = data;
        }

        public void Initialisation()
        {
            _upgradeViews = new List<UpgradeView>();
            _upgradesMetaData = _progressionData.UpgradesMeta;
            InitializeUpgradesUI();

        }

        private void InitializeUpgradesUI()
        {
            foreach (UpgradeConfig upgradeConfig in _upgradesPool.UpgradesList)
            {
                UpgradeView view = _shopView.UpgradeViews.GetValue(upgradeConfig.Name);
                view.UpdateUI(upgradeConfig, 0);
                view.UpgradeButton.onClick.AddListener(() => UpgradeButtonClicked(view));
                _upgradeViews.Add(view);
            }
        }

        public void Cleanup()
        {
            foreach (UpgradeView view in _upgradeViews)
            {
                view.UpgradeButton.onClick.RemoveAllListeners();
            }
        }
        
        
        private void UpgradeButtonClicked(UpgradeView view)
        {
            //Debug.Log($"Upgrade {view.UpgradeConfig.Name} clicked");
            _economyEventBus.OnTryBuyUpgrade?.Invoke(view.UpgradeConfig.Name, view);
        }
    }
}