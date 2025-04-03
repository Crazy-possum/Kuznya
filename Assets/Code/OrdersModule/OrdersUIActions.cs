using GameCoreModule;
using MAEngine;
using UnityEngine;
using Zenject;

namespace Orders
{
    public class OrdersUIActions : IAction, IInitialisation, ICleanUp
    {
        private const float HIDDEN_POSITION_Y = -70;
        private const float VISIBLE_POSITION_Y = 130;
        
        private OrdersView _ordersView;
        private EconomyEventBus _economyEventBus;
        
        private bool _isInventoryShowed;
        
        [Inject]
        public void Construct(OrdersView orderView, EconomyEventBus economyEventBus)
        {
            _ordersView = orderView;
            _economyEventBus = economyEventBus;
        }
        
        public void Initialisation()
        {
            SubscribeView();
            HideInventory();
            _ordersView.ResizeButton.Button.onClick.AddListener(ResizeInventory);
        }

        public void Cleanup()
        {
            UnSubscribeView();
            _ordersView.ResizeButton.Button.onClick.RemoveListener(ResizeInventory);
        }
        
        private void SubscribeView()
        {
            _economyEventBus.OnMaterialInitialization += _ordersView.InitializeMaterial;
            _economyEventBus.OnMaterialUpdateInfo += _ordersView.UpdateMaterialInfo;
        }
        
        private void UnSubscribeView()
        {
            _economyEventBus.OnMaterialInitialization -= _ordersView.InitializeMaterial;
            _economyEventBus.OnMaterialUpdateInfo -= _ordersView.UpdateMaterialInfo;
        }
        
        private void ResizeInventory()
        {
            RectTransform resizeButtonImageRect =
                _ordersView.ResizeButton.ButtonImage.gameObject.GetComponent<RectTransform>();
            if (_isInventoryShowed)
            {
                resizeButtonImageRect.rotation = Quaternion.Euler(0f, 0f, 0f);
                HideInventory();
            }
            else
            {
                resizeButtonImageRect.rotation = Quaternion.Euler(0f, 0f, 180f);
                ShowInventory();
            }
        }
        
        private void ShowInventory()
        {
            _ordersView.MaterialsPanelRect.anchoredPosition = new Vector2(0, VISIBLE_POSITION_Y);
            _isInventoryShowed = true;
        }

        private void HideInventory()
        {
            _ordersView.MaterialsPanelRect.anchoredPosition = new Vector2(0, HIDDEN_POSITION_Y);
            _isInventoryShowed = false;
        }
    }
}