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
        private const float ORDER_START_DELAY = 2f;

        private ProgressionData _progressionData;
        private OrdersView _ordersView;
        private OrdersEventBus _ordersEventBus;
        private ResultsEventBus _resultsEventBus;
        private EconomyEventBus _economyEventBus;
        private UIEventBus _uiEventBus;

        private OrdersMetaData _ordersMetaData;
        private PlayerMetaData _playerMetaData;
        private List<OrderPanelView> _emptyOrderPanels;
        private Dictionary<OrderPanelView, ActiveOrder> _activeOrders;
        private Dictionary<OrderPanelView, Timer> _ordersToRemove;
        private OrderPanelView _currentActiveOrder;
        private Timer _orderStartDelayTimer;

        [Inject]
        public void Construct(ProgressionData progressionData,
            OrdersView ordersView, OrdersEventBus ordersEventBus,
            ResultsEventBus resultsEventBus, EconomyEventBus economyEventBus,
            UIEventBus uiEventBus)
        {
            _progressionData = progressionData;
            _ordersView = ordersView;
            _ordersEventBus = ordersEventBus;
            _resultsEventBus = resultsEventBus;
            _economyEventBus = economyEventBus;
            _uiEventBus = uiEventBus;
        }


        public void Initialisation()
        {
            _ordersMetaData = _progressionData.OrdersMeta;
            _playerMetaData = _progressionData.PlayerMetaData;
            _ordersEventBus.OnOrderAdded += AddOrder;
            _resultsEventBus.OnResultsFinished += ActiveOrderFinished;
            _ordersToRemove = new Dictionary<OrderPanelView, Timer>();
            _activeOrders = new Dictionary<OrderPanelView, ActiveOrder>();
            InitializeOrderPanels();
            AddInitialOrders();
        }

        public void Cleanup()
        {
            _ordersEventBus.OnOrderAdded -= AddOrder;
            _resultsEventBus.OnResultsFinished -= ActiveOrderFinished;
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

            if (_orderStartDelayTimer != null)
            {
                if (_orderStartDelayTimer.Wait())
                {
                    _uiEventBus.OnUnfreezeUI?.Invoke();
                    _ordersEventBus.OnOrderStarted?.Invoke(_activeOrders[_currentActiveOrder]);
                    _orderStartDelayTimer = null;
                }
            }
        }
        
        private void AddInitialOrders()
        {
            if (_ordersMetaData.ActiveOrders.Count > 0)
            {
                for (int i = 0; i < _ordersMetaData.ActiveOrders.Count; i++)
                {
                    ActiveOrder order = _ordersMetaData.GetActiveOrder(i);
                    AddOrder(order);
                }
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
                        if (_currentActiveOrder != null)
                        {
                            if (_currentActiveOrder == keyValuePair.Key)
                            {
                                _ordersEventBus.OnOrderEnded?.Invoke(keyValuePair.Value);
                            }
                        }
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
                () => ConfirmOrSubmitOrder(orderPanelView));
            _emptyOrderPanels.Add(orderPanelView);
            orderPanelView.SetOrderPanelState(false);
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
                () => ConfirmOrSubmitOrder(orderPanelView));
        }

        private void AddOrder()
        {
            ActiveOrder activeOrder =
                _ordersMetaData.GetActiveOrder(_ordersMetaData.ActiveOrders.Count - 1);
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
            orderPanelView.SetOrderPanelState(true);
        }

        private void RemoveOrder(OrderPanelView orderPanelView, float time)
        {
            _ordersToRemove.Add(orderPanelView, new Timer(time));
        }

        private void RemoveOrder(OrderPanelView orderPanelView)
        {
            orderPanelView.SetOrderPanelState(false);
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
            orderPanelView.OrderIcon.sprite = activeOrder.OrderIcon;
            orderPanelView.OrderMaterial1View.MaterialName.text =
                activeOrder.Materials[0].Config.Name;
            orderPanelView.OrderMaterial1View.MaterialImage.sprite =
                activeOrder.Materials[0].Config.Sprite;
            orderPanelView.OrderMaterial1View.MaterialCount.text =
                $"{activeOrder.Materials[0].Count} шт.";
            orderPanelView.OrderTimeSlider.value = 1f;
            orderPanelView.ActiveOrder = activeOrder;
        }

        private void UpdateOrderSlider(OrderPanelView orderPanelView, ActiveOrder activeOrder)
        {
            orderPanelView.OrderTimeSlider.value =
                activeOrder.OrderTimer.GetRemainingTime() /
                activeOrder.OrderTime;
        }

        private void ConfirmOrSubmitOrder(OrderPanelView orderPanelView)
        {
            if (orderPanelView.ActiveOrder.IsCompleted)
            {
                SubmitOrder(orderPanelView);
            }
            else
            {
                ShowConfirmPanel(orderPanelView);
            }
        }
        
        private bool SetConfirmButtonState(OrderPanelView orderPanelView)
        {
            bool haveEnoughtMaterials = true;
            foreach (ForgingMaterial material in orderPanelView.ActiveOrder.Materials)
            {
                if (_playerMetaData.Materials.IsContainsKey(material.Config.MaterialName))
                {
                    if (_playerMetaData.Materials[material.Config.MaterialName] < material.Count)
                    {
                        haveEnoughtMaterials = false;
                    }
                }
            }

            if (haveEnoughtMaterials)
            {
                _ordersView.ConfirmButton.interactable = true;
            }
            else
            {
                _ordersView.ConfirmButton.interactable = false;
            }
            
            return haveEnoughtMaterials;
        }

        private void ShowConfirmPanel(OrderPanelView orderPanelView)
        {
            if (SetConfirmButtonState(orderPanelView))
            {
                _ordersView.ConfirmPanelText.text = "Начать ковку?";
            }
            else
            {
                _ordersView.ConfirmPanelText.text = "Недостаточно материала";
            }
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
            _uiEventBus.OnFreezeUI?.Invoke();
            _currentActiveOrder = orderPanelView;
            foreach (ForgingMaterial material in _currentActiveOrder.ActiveOrder.Materials)
            {
                _economyEventBus.OnMaterialRemoved?.Invoke(material.Config.MaterialName, material.Count);
            }
            _orderStartDelayTimer = new Timer(ORDER_START_DELAY);
            HideConfirmPanel();
        }

        private void DenyOrder()
        {
            HideConfirmPanel();
        }
        
        private void SubmitOrder(OrderPanelView orderPanelView)
        {
            _ordersEventBus.OnOrderFinished?.Invoke(orderPanelView.ActiveOrder);
            RemoveOrder(orderPanelView);
        }
        
        
        private void ActiveOrderFinished(int score)
        {
            if (_currentActiveOrder != null)
            {
                if (score != 0)
                {
                    _currentActiveOrder.ActiveOrder.Reward = score;
                    _currentActiveOrder.SetCompletionState(true);
                }
                _currentActiveOrder = null;
            }
        }
    }
}

