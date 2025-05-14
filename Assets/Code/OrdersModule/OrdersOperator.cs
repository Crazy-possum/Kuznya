using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Progression;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Orders
{
    public class OrdersOperator : IAction, IInitialisation, ICleanUp, IFixedExecute
    {
        private const float REMOVE_TIME = 2f;
        private const float ORDER_START_DELAY = 2f;
        private const float WHORESALE_ORDER_DELAY = 11f;
        private const float MINUTE_MULTIPLER = 60f;
        private const int WHORESALE_COUNT = 10;
        private const int OPENED_STAGES = 4;

        private ProgressionData _progressionData;
        private OrdersView _ordersView;
        private OrdersEventBus _ordersEventBus;
        private ResultsEventBus _resultsEventBus;
        private EconomyEventBus _economyEventBus;
        private UIEventBus _uiEventBus;
        private ClientsPoolConfig _clientsPoolConfig;
        private OrderSpritesContainer _orderSpritesContainer;

        private OrdersMetaData _ordersMetaData;
        private PlayerMetaData _playerMetaData;
        private List<OrderPanelView> _emptyOrderPanels;
        private Dictionary<OrderPanelView, ActiveOrder> _activeOrders;
        private Dictionary<OrderPanelView, Timer> _ordersToRemove;
        private OrderPanelView _currentActiveOrder;
        private Timer _orderStartDelayTimer;
        private UpgradesMetaData _upgradesMetaData;
        private int _activeOrdersCount;
        private int _maxOrdersCount;

        private ClientConfig _whoresaleClient;
        private OrdersPoolConfig _whoresaleConfig;
        private Timer _whoresaleTimer;
        private Timer _whoresaleAutomationTimer;
        private OrderPanelView _whoresalePanel;
        private float _whoresaleDelay;
        private float _whoresaleAutomationDelay;
        private bool _isWhoresaleOrderReadyToRecive;
        private System.Random _random;
        private ActiveOrder _whoresaleActiveOrder;
        
        

        [Inject]
        public void Construct(ProgressionData progressionData,
            OrdersView ordersView, OrdersEventBus ordersEventBus,
            ResultsEventBus resultsEventBus, EconomyEventBus economyEventBus,
            UIEventBus uiEventBus, ClientsPoolConfig clientsPoolConfig,
            OrderSpritesContainer orderSpritesContainer)
        {
            _progressionData = progressionData;
            _ordersView = ordersView;
            _ordersEventBus = ordersEventBus;
            _resultsEventBus = resultsEventBus;
            _economyEventBus = economyEventBus;
            _uiEventBus = uiEventBus;
            _clientsPoolConfig = clientsPoolConfig;
            _orderSpritesContainer = orderSpritesContainer;
        }


        public void Initialisation()
        {
            _ordersMetaData = _progressionData.OrdersMeta;
            _playerMetaData = _progressionData.PlayerMetaData;
            _upgradesMetaData = _progressionData.UpgradesMeta;
            _ordersEventBus.OnOrderAdded += AddOrder;
            _resultsEventBus.OnResultsFinished += ActiveOrderFinished;
            _ordersToRemove = new Dictionary<OrderPanelView, Timer>();
            _activeOrders = new Dictionary<OrderPanelView, ActiveOrder>();
            _economyEventBus.OnUpgradeApplied += CheckIsWhoresaleUpgradesChanged;
            _activeOrdersCount = 0;
            _random = new Random();
            InitializeOrderPanels();
            SetOrdersCount();
            AddInitialOrders();
            InitializeWhoresale();
        }

        private void CheckIsWhoresaleUpgradesChanged(UpgradeName upgradeName)
        {
            if (upgradeName == UpgradeName.WholesaleFrequency || upgradeName == UpgradeName.AutomaticWholesale)
            {
                SetupWhoresaleTimer();
            }
        }

        private void InitializeWhoresale()
        {
            foreach (ClientConfig clientConfig in _clientsPoolConfig.Clients)
            {
                if (clientConfig.ClientType == ClientType.Wholesale)
                {
                    _whoresaleClient = clientConfig;
                    _whoresaleConfig = clientConfig.Orders;
                    break;
                }
            }
            _whoresalePanel = _emptyOrderPanels[_emptyOrderPanels.Count - 1];
            _emptyOrderPanels.Remove(_whoresalePanel);
            SetupWhoresaleTimer();
            if (_whoresaleTimer != null)
            {
                _whoresaleTimer.SetRemainigTime(_ordersMetaData.WhoresaleOrderTime);
                if (_ordersMetaData.WhoresaleOrder != null)
                {
                    if (_ordersMetaData.WhoresaleOrder.Name != null)
                    {
                        Debug.Log(_ordersMetaData.WhoresaleOrder.Name);
                        SetupOrderView(_whoresalePanel, _ordersMetaData.WhoresaleOrder);
                        _whoresaleActiveOrder = _ordersMetaData.WhoresaleOrder;
                        _whoresalePanel.SetOrderPanelState(true);
                    }
                }
            }
        }

        private void SetupWhoresaleTimer()
        {
            _whoresaleDelay = 0;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.WholesaleFrequency))
            {
                _whoresaleDelay = _upgradesMetaData.Upgrades[UpgradeName.WholesaleFrequency].GetUpgradeData();
            }
            _whoresaleDelay *= MINUTE_MULTIPLER;
            if (_whoresaleDelay > 0)
            {
                _whoresaleTimer = new Timer(_whoresaleDelay);
            }

            _whoresaleAutomationDelay = 0;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.AutomaticWholesale))
            {
                _whoresaleAutomationDelay = _upgradesMetaData.Upgrades[UpgradeName.AutomaticWholesale].GetUpgradeData();
            }
            _whoresaleAutomationDelay *= MINUTE_MULTIPLER;
            if (_whoresaleAutomationDelay > 0)
            {
                _whoresaleAutomationTimer = new Timer(_whoresaleAutomationDelay);
            }
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
                    if (_activeOrders.ContainsKey(_currentActiveOrder))
                    {
                        _ordersEventBus.OnOrderStarted?.Invoke(_activeOrders[_currentActiveOrder]);
                    }
                    else
                    {
                        _ordersEventBus.OnOrderStarted?.Invoke(_whoresaleActiveOrder);
                    }
                    
                    _orderStartDelayTimer = null;
                }
                
            }

            if (_whoresaleTimer != null)
            {
                if (_whoresaleTimer.Wait())
                {
                    _isWhoresaleOrderReadyToRecive = true;
                }
                _ordersMetaData.WhoresaleOrderTime = _whoresaleTimer.GetRemainingTime();
            }

            if (_isWhoresaleOrderReadyToRecive)
            {
                if (_whoresalePanel.ActiveOrder == null)
                {
                    SetupNewWhoresaleOrder();
                    _whoresaleTimer = new Timer(_whoresaleDelay);
                    _isWhoresaleOrderReadyToRecive = false;
                }
            }

            if (_whoresaleAutomationTimer != null)
            {
                if (_whoresaleAutomationTimer.Wait())
                {
                    if (_whoresaleActiveOrder != null)
                    {
                        if (_whoresaleActiveOrder.CurrentOrderCount < _whoresaleActiveOrder.OrderCount)
                        {
                            _whoresaleActiveOrder.CurrentOrderCount++;
                            _whoresalePanel.SetupOrderCount(_whoresaleActiveOrder.OrderCount,
                                _whoresaleActiveOrder.CurrentOrderCount);
                            _whoresaleActiveOrder.Reward += _whoresaleActiveOrder.BasicCost * 50 * OPENED_STAGES;
                            if (_whoresaleActiveOrder.CurrentOrderCount == _whoresaleActiveOrder.OrderCount)
                            {
                                _whoresaleActiveOrder.IsCompleted = true;
                                _whoresalePanel.SetCompletionState(true);
                            }
                        }
                    }
                }
                
            }
        }

        private void SetupNewWhoresaleOrder()
        {
            List<OrderConfig> whoresaleOrders = new List<OrderConfig>();
            foreach (OrderConfig order in _whoresaleConfig.GetOrders())
            {
                if (order.Materials[0].Config.MaterialName <= _playerMetaData.CurrentMaximumMaterial)
                {
                    whoresaleOrders.Add(order);
                }
            }
            
            int orderIndex = _random.Next(whoresaleOrders.Count);
            ActiveOrder activeWhoresaleOrder = new ActiveOrder(_whoresaleClient,
                whoresaleOrders[orderIndex], 0);
            activeWhoresaleOrder.OrderCount = WHORESALE_COUNT;
            SetupOrderView(_whoresalePanel, activeWhoresaleOrder);
            _whoresalePanel.SetOrderPanelState(true);
            _whoresaleActiveOrder = activeWhoresaleOrder;
            _ordersMetaData.SaveWhoresaleOrder(_whoresaleActiveOrder);

        }

        private void SetOrdersCount()
        {
            _maxOrdersCount = 1;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.MultiTask))
            {
                _maxOrdersCount += (int)_upgradesMetaData.Upgrades[UpgradeName.MultiTask].GetUpgradeData();
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

            if (_whoresaleActiveOrder != null)
            {
                UpdateOrderSlider(_whoresalePanel, _whoresaleActiveOrder);
                if (_whoresaleActiveOrder.OrderTimer.Wait())
                {
                    if (_currentActiveOrder != null)
                    {
                        if (_currentActiveOrder == _whoresalePanel)
                        {
                            _ordersEventBus.OnOrderEnded?.Invoke(_whoresaleActiveOrder);
                        }
                    }
                    _whoresaleActiveOrder.OrderTimer = null;
                    _whoresalePanel.ActiveOrder = null;
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
            if (_emptyOrderPanels.Count > 0 && _maxOrdersCount > _activeOrdersCount)
            {
                orderPanelView = _emptyOrderPanels[0];
                _emptyOrderPanels.Remove(orderPanelView);
                _activeOrdersCount++;
                if (_maxOrdersCount == _activeOrdersCount)
                {
                    _ordersEventBus.OnOrdersBoardSpaceChanged?.Invoke(false);
                }
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
            ActiveOrder activeOrder;
            if (_activeOrders.ContainsKey(orderPanelView))
            {
                activeOrder = _activeOrders[orderPanelView];
                _ordersMetaData.ActiveOrders.Remove(activeOrder);
                _activeOrders.Remove(orderPanelView);
                _emptyOrderPanels.Insert(0, orderPanelView);
                _activeOrdersCount--;
            }
            else
            {
                activeOrder = _whoresaleActiveOrder;
            }
            _ordersEventBus.OnOrdersBoardSpaceChanged?.Invoke(true);
            _ordersEventBus.OnOrderRemoved?.Invoke();
        }

        private void SetupOrderView(OrderPanelView orderPanelView, ActiveOrder activeOrder)
        {
            //PlayerPrefs.DeleteAll();
            orderPanelView.OrderName.text = activeOrder.Name;
            orderPanelView.OrderDesc.text = activeOrder.Description.Description;
            orderPanelView.OrderIcon.sprite = _orderSpritesContainer.OrderSpritesDict[activeOrder.OrderType].
                ItemSpritesDict[activeOrder.Materials[0].Config.MaterialName].OrderSprite;
            orderPanelView.OrderMaterial1View.MaterialName.text =
                activeOrder.Materials[0].Config.Name;
            orderPanelView.OrderMaterial1View.MaterialImage.sprite =
                activeOrder.Materials[0].Config.Sprite;
            orderPanelView.OrderMaterial1View.MaterialCount.text =
                $"{activeOrder.Materials[0].Count} шт.";
            orderPanelView.OrderTimeSlider.value = 1f;
            orderPanelView.ActiveOrder = activeOrder;
            orderPanelView.SetInitialCompletionState(activeOrder.IsCompleted);
            
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
                int materialEconomy = 0;
                if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.Economy))
                {
                    materialEconomy = (int)Mathf.Ceil(material.Count *
                                                      _upgradesMetaData.Upgrades[UpgradeName.Economy].GetUpgradeData());
                }
                int requiredMaterialCount = material.Count - materialEconomy;
                if (requiredMaterialCount < 1)
                {
                    requiredMaterialCount = 1;
                }
                _economyEventBus.OnMaterialRemoved?.Invoke(material.Config.MaterialName, requiredMaterialCount);
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
                    _currentActiveOrder.ActiveOrder.Reward += score;
                    _currentActiveOrder.SetCompletionState(true);
                }
                _currentActiveOrder = null;
            }
        }
    }
}

