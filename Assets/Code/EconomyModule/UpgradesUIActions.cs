using System.Collections.Generic;
using GameCoreModule;
using MAEngine;
using UnityEngine;
using Zenject;

namespace Economy
{
    public class UpgradesUIActions : IAction, IInitialisation, ICleanUp
    {
        private ShopView _shopView;
        private UpgradesPool _upgradesPool;
        private EconomyEventBus _economyEventBus;
        
        private List<UpgradeView> _upgradeViews;

        [Inject]
        public void Construct(ShopView shopView, UpgradesPool upgradesPool, EconomyEventBus economyEventBus)
        {
            _shopView = shopView;
            _upgradesPool = upgradesPool;
            _economyEventBus = economyEventBus;
        }

        public void Initialisation()
        {
            _upgradeViews = new List<UpgradeView>();
            InitializeUpgradesUI();

        }

        private void InitializeUpgradesUI()
        {
            foreach (UpgradeConfig upgradeConfig in _upgradesPool.UpgradesList)
            {
                UpgradeView view = _shopView.UpgradeViews.GetValue(upgradeConfig.Name);
                view.UpdateUI(upgradeConfig, 0);
                view.UpgradeButton.onClick.AddListener(() => UpgradeButtonClicked(view));
                view.SetLevel(0);
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
            Debug.Log($"Upgrade {view.UpgradeConfig.Name} clicked");
            view.AddLevel();
            _economyEventBus.OnTryBuyUpgrade?.Invoke(view.UpgradeConfig.Name, view);
        }
    }
}