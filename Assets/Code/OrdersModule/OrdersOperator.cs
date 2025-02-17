using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Progression;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Orders
{
    public class OrdersOperator : IAction, IInitialisation, ICleanUp, IFixedExecute
    {
        private const float REMOVE_TIME = 2f;

        private ProgressionData _progressionData;
        private OrdersView _ordersView;
        private OrdersEventBus _ordersEventBus;

        private OrdersMetaData _ordersMetaData;
        private List<OrderPanelView> _emptyOrderPanels;
        private Dictionary<OrderPanelView, ActiveOrder> _activeOrders;
        private Dictionary<OrderPanelView, Timer> _ordersToRemove;

        [Inject]
        public void Construct(ClientsPoolConfig clientsPoolConfig,
            ProgressionData progressionData,
            OrdersView ordersView, OrdersEventBus ordersEventBus)
        {
            _progressionData = progressionData;
            _ordersView = ordersView;
            _ordersEventBus = ordersEventBus;
        }


        public void Initialisation()
        {
            _ordersMetaData = _progressionData.OrdersMeta;
            _ordersEventBus.OnOrderAdded += AddOrder;
            _ordersToRemove = new Dictionary<OrderPanelView, Timer>();
            _activeOrders = new Dictionary<OrderPanelView, ActiveOrder>();
            InitializeOrderPanels();
            AddInitialOrders();
        }

        private void AddInitialOrders()
        {
            if (_ordersMetaData.ActiveOrders.Count > 0)
            {
                foreach (ActiveOrder order in _ordersMetaData.ActiveOrders)
                {
                    AddOrder(order);
                }
            }
        }

        public void Cleanup()
        {
            _ordersEventBus.OnOrderAdded -= AddOrder;
            _ordersView.ConfirmButton.onClick.RemoveAllListeners();
            _ordersView.DenyButton.onClick.RemoveAllListeners();
            CleanOrderPanels();
        }

        public void FixedExecute(float fixedDeltaTime)
        {

            if (_activeOrders.Count > 0)
            {
                CheckPanelsTimers();
            }
            if (_ordersToRemove.Count > 0)
            {
                RemoveDelayedPanels();
            }
        }

        private void CheckPanelsTimers()
        {
            foreach (KeyValuePair<OrderPanelView, ActiveOrder> keyValuePair in _activeOrders)
            {
                if (keyValuePair.Value.OrderTimer != null)
                {
                    UpdateOrderSlider(keyValuePair.Key, keyValuePair.Value);
                    if (keyValuePair.Value.OrderTimer.Wait())
                    {
                        keyValuePair.Value.OrderTimer = null;
                        RemoveOrder(keyValuePair.Key, REMOVE_TIME);
                    }
                }
            }
        }

        private void RemoveDelayedPanels()
        {
            List<OrderPanelView> removedPanels = new List<OrderPanelView>();
            foreach (KeyValuePair<OrderPanelView, Timer> keyValuePair in _ordersToRemove)
            {
                if (keyValuePair.Value != null)
                {
                    if (keyValuePair.Value.Wait())
                    {
                        removedPanels.Add(keyValuePair.Key);
                        RemoveOrder(keyValuePair.Key);
                    }
                }
            }
            if (removedPanels.Count > 0)
            {
                foreach (OrderPanelView removedPanel in removedPanels)
                {
                    _ordersToRemove.Remove(removedPanel);
                }
            }
        }

        private void InitializeOrderPanels()
        {
            _emptyOrderPanels = new List<OrderPanelView>();
            InitializeOrderPanel(_ordersView.Order1PanelView);
            InitializeOrderPanel(_ordersView.Order2PanelView);
            InitializeOrderPanel(_ordersView.Order3PanelView);
            InitializeOrderPanel(_ordersView.Order4PanelView);
            InitializeOrderPanel(_ordersView.Order5PanelView);
            InitializeOrderPanel(_ordersView.Order6PanelView);
            InitializeOrderPanel(_ordersView.Order7PanelView);
            InitializeOrderPanel(_ordersView.Order8PanelView);
        }

        private void InitializeOrderPanel(OrderPanelView orderPanelView)
        {
            orderPanelView.OrderButton.onClick.AddListener(
                () => ShowConfirmPanel(orderPanelView));
            _emptyOrderPanels.Add(orderPanelView);
        }

        private void CleanOrderPanels()
        {
            _emptyOrderPanels = new List<OrderPanelView>();
            CleanOrderPanel(_ordersView.Order1PanelView);
            CleanOrderPanel(_ordersView.Order2PanelView);
            CleanOrderPanel(_ordersView.Order3PanelView);
            CleanOrderPanel(_ordersView.Order4PanelView);
            CleanOrderPanel(_ordersView.Order5PanelView);
            CleanOrderPanel(_ordersView.Order6PanelView);
            CleanOrderPanel(_ordersView.Order7PanelView);
            CleanOrderPanel(_ordersView.Order8PanelView);
        }

        private void CleanOrderPanel(OrderPanelView orderPanelView)
        {
            orderPanelView.OrderButton.onClick.RemoveListener(
                () => ShowConfirmPanel(orderPanelView));
        }

        private void AddOrder()
        {
            ActiveOrder activeOrder =
                _ordersMetaData.ActiveOrders[_ordersMetaData.ActiveOrders.Count - 1];
            AddOrder(activeOrder);
        }

        private void AddOrder(ActiveOrder activeOrder)
        {

            OrderPanelView orderPanelView = null;
            if (_emptyOrderPanels.Count > 0)
            {
                orderPanelView = _emptyOrderPanels[0];
                _emptyOrderPanels.Remove(orderPanelView);
            }
            else
            {
                Debug.Log("No space in orders");
                return;
            }
            _activeOrders.Add(orderPanelView, activeOrder);
            SetupOrderView(orderPanelView, activeOrder);
            orderPanelView.ActiveOrderPanel.SetActive(true);
        }

        private void RemoveOrder(OrderPanelView orderPanelView, float time)
        {
            _ordersToRemove.Add(orderPanelView, new Timer(time));
        }

        private void RemoveOrder(OrderPanelView orderPanelView)
        {
            orderPanelView.ActiveOrderPanel.SetActive(false);
            ActiveOrder activeOrder = _activeOrders[orderPanelView];
            _ordersMetaData.ActiveOrders.Remove(activeOrder);
            _activeOrders.Remove(orderPanelView);
            _emptyOrderPanels.Insert(0, orderPanelView);
            _ordersEventBus.OnOrderRemoved?.Invoke();
        }

        private void SetupOrderView(OrderPanelView orderPanelView, ActiveOrder activeOrder)
        {
            orderPanelView.OrderName.text = activeOrder.Name;
            orderPanelView.OrderDesc.text = activeOrder.Description.Description;
            orderPanelView.OrderMaterial1View.MaterialName.text =
                activeOrder.Materials[0].Config.Name;
            orderPanelView.OrderMaterial1View.MaterialImage.sprite =
                activeOrder.Materials[0].Config.Sprite;
            orderPanelView.OrderMaterial1View.MaterialCount.text =
                $"{activeOrder.Materials[0].Count} шт.";
            orderPanelView.OrderTimeSlider.value = 1f;
        }

        private void UpdateOrderSlider(OrderPanelView orderPanelView, ActiveOrder activeOrder)
        {
            orderPanelView.OrderTimeSlider.value =
                activeOrder.OrderTimer.GetRemainingTime() /
                activeOrder.OrderTime;
        }

        private void ShowConfirmPanel(OrderPanelView orderPanelView)
        {
            _ordersView.ConfirmPanel.SetActive(true);
            _ordersView.ConfirmButton.onClick.AddListener(
                () => AcceptOrder(orderPanelView));
            _ordersView.DenyButton.onClick.AddListener(
                () => DenyOrder());
        }

        private void HideConfirmPanel()
        {
            _ordersView.ConfirmPanel.SetActive(false);
            _ordersView.ConfirmButton.onClick.RemoveAllListeners();
            _ordersView.DenyButton.onClick.RemoveAllListeners();
        }

        private void AcceptOrder(OrderPanelView orderPanelView)
        {
            _ordersEventBus.OnOrderStarted?.Invoke(_activeOrders[orderPanelView]);
            RemoveOrder(orderPanelView);
            HideConfirmPanel();
        }

        private void DenyOrder()
        {
            HideConfirmPanel();
        }
    }
}

