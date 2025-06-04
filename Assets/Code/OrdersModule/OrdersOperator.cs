using GameCoreModule;
using MAEngine;
using Progression;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Random = System.Random;
using Timer = MAEngine.Extention.Timer;

namespace Orders
{
    public class OrdersOperator : IAction, IInitialisation, ICleanUp, IFixedExecute
    {
        private const float REMOVE_TIME = 2f;
        private const float ORDER_START_DELAY = 2f;
        private const float WHOLESALE_ORDER_TIME = 20f;
        private const float DEFAULT_ORDER_TIME = 8f;
        private const float MINUTE_MULTIPLER = 60f;
        private const int WHOLESALE_COUNT = 5;
        private const int OPENED_STAGES = 4;

        private ProgressionData _progressionData;
        private OrdersView _ordersView;
        private OrdersEventBus _ordersEventBus;
        private ResultsEventBus _resultsEventBus;
        private EconomyEventBus _economyEventBus;
        private UIEventBus _uiEventBus;
        private ClientsPoolConfig _clientsPoolConfig;
        private OrderSpritesContainer _orderSpritesContainer;
        private TutorialEventBus _tutorialEventBus;

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

        private ClientConfig _wholesaleClient;
        private OrdersPoolConfig _wholesaleConfig;
        private Timer _wholesaleTimer;
        private Timer _wholesaleAutomationTimer;
        private OrderPanelView _wholesalePanel;
        private float _wholesaleDelay;
        private float _wholesaleAutomationDelay;
        private bool _isWholesaleOrderReadyToRecive;
        private System.Random _random;
        private ActiveOrder _wholesaleActiveOrder;
        private bool _isWholesaleReadyToSetup;
        private bool _isTutorialActive;
        
        

        [Inject]
        public void Construct(ProgressionData progressionData,
            OrdersView ordersView, OrdersEventBus ordersEventBus,
            ResultsEventBus resultsEventBus, EconomyEventBus economyEventBus,
            UIEventBus uiEventBus, ClientsPoolConfig clientsPoolConfig,
            OrderSpritesContainer orderSpritesContainer, TutorialEventBus tutorialEventBus)
        {
            _progressionData = progressionData;
            _ordersView = ordersView;
            _ordersEventBus = ordersEventBus;
            _resultsEventBus = resultsEventBus;
            _economyEventBus = economyEventBus;
            _uiEventBus = uiEventBus;
            _clientsPoolConfig = clientsPoolConfig;
            _orderSpritesContainer = orderSpritesContainer;
            _tutorialEventBus = tutorialEventBus;
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
            _activeOrdersCount = 0;
            _random = new Random();
            _isWholesaleReadyToSetup = false;
            InitializeOrderPanels();
            SetOrdersCount();
            AddInitialOrders();
            InitializeWholesale();
            _economyEventBus.OnUpgradeApplied += CheckIsUpgradesChanged;
            _tutorialEventBus.OnMaterialScreenOpened += HideConfirmPanel;
            _tutorialEventBus.OnOrderSectionStarted += () => { _isTutorialActive = true; };
            _tutorialEventBus.OnOrderSectionFinished += () => { _isTutorialActive = false; };
        }

        private void CheckIsUpgradesChanged(UpgradeName upgradeName)
        {
            if (upgradeName == UpgradeName.WholesaleFrequency || upgradeName == UpgradeName.AutomaticWholesale)
            {
                SetupWholesaleTimer();
            }
            if (upgradeName == UpgradeName.MultiTask)
            {
                SetOrdersCount();
            }
        }

        private void InitializeWholesale()
        {
            foreach (ClientConfig clientConfig in _clientsPoolConfig.Clients)
            {
                if (clientConfig.ClientType == ClientType.Wholesale)
                {
                    _wholesaleClient = clientConfig;
                    _wholesaleConfig = clientConfig.Orders;
                    break;
                }
            }
            _wholesalePanel = _emptyOrderPanels[_emptyOrderPanels.Count - 1];
            _emptyOrderPanels.Remove(_wholesalePanel);
            SetupWholesaleTimer();
            if (_wholesaleTimer != null)
            {
                _wholesaleTimer.SetRemainigTime(_ordersMetaData.WholesaleOrderTime);
                if (_ordersMetaData.WholesaleOrder != null)
                {
                    if (_ordersMetaData.WholesaleOrder.Name != null)
                    {
                        SetupOrderView(_wholesalePanel, _ordersMetaData.WholesaleOrder);
                        _wholesaleActiveOrder = _ordersMetaData.WholesaleOrder;
                        _wholesalePanel.SetOrderPanelState(true);
                    }
                }
            }

            _isWholesaleReadyToSetup = true;
        }

        private void SetupWholesaleTimer()
        {
            _wholesaleDelay = 0;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.WholesaleFrequency))
            {
                _wholesaleDelay = _upgradesMetaData.Upgrades[UpgradeName.WholesaleFrequency].GetUpgradeData();
                if (_wholesaleActiveOrder == null && _isWholesaleReadyToSetup)
                {
                    SetupNewWholesaleOrder();
                }
                _wholesalePanel.SetLockedState(false);
                
            }
            _wholesaleDelay *= MINUTE_MULTIPLER;
            if (_wholesaleDelay > 0)
            {
                _wholesaleTimer = new Timer(_wholesaleDelay);
            }

            _wholesaleAutomationDelay = 0;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.AutomaticWholesale))
            {
                _wholesaleAutomationDelay = _upgradesMetaData.Upgrades[UpgradeName.AutomaticWholesale].GetUpgradeData();
            }
            _wholesaleAutomationDelay *= MINUTE_MULTIPLER;
            if (_wholesaleAutomationDelay > 0)
            {
                _wholesalePanel.WholesaleAutomationTimeSlider.gameObject.SetActive(true);
                _wholesalePanel.WholesaleAutomationTimeSlider.maxValue = _wholesaleAutomationDelay;
                _wholesalePanel.WholesaleAutomationTimeSlider.value = _wholesaleAutomationDelay;
                _wholesaleAutomationTimer = new Timer(_wholesaleAutomationDelay);
            }
        }

        public void Cleanup()
        {
            _ordersEventBus.OnOrderAdded -= AddOrder;
            _resultsEventBus.OnResultsFinished -= ActiveOrderFinished;
            _ordersView.ConfirmButton.onClick.RemoveAllListeners();
            _ordersView.DenyButton.onClick.RemoveAllListeners();
            CleanOrderPanels();
            _economyEventBus.OnUpgradeApplied -= CheckIsUpgradesChanged;
            _tutorialEventBus.OnMaterialScreenOpened -= HideConfirmPanel;
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
                        _ordersEventBus.OnOrderStarted?.Invoke(_wholesaleActiveOrder);
                    }
                    
                    _orderStartDelayTimer = null;
                }
                
            }

            if (_wholesaleTimer != null)
            {
                if (_wholesaleTimer.Wait())
                {
                    _isWholesaleOrderReadyToRecive = true;
                }
                _ordersMetaData.WholesaleOrderTime = _wholesaleTimer.GetRemainingTime();
            }

            if (_isWholesaleOrderReadyToRecive)
            {
                if (_wholesalePanel.ActiveOrder == null)
                {
                    SetupNewWholesaleOrder();
                    _wholesaleTimer = new Timer(_wholesaleDelay);
                    _isWholesaleOrderReadyToRecive = false;
                }
            }

            if (_wholesaleAutomationTimer != null)
            {
                UpdateWholesaleUI();
                if (_wholesaleAutomationTimer.Wait())
                {
                    if (_wholesaleActiveOrder != null)
                    {
                        if (_wholesaleActiveOrder.CurrentOrderCount < _wholesaleActiveOrder.OrderCount)
                        {
                            _wholesaleActiveOrder.CurrentOrderCount++;
                            _wholesalePanel.SetupOrderCount(_wholesaleActiveOrder.OrderCount,
                                _wholesaleActiveOrder.CurrentOrderCount);
                            _wholesaleActiveOrder.Reward += _wholesaleActiveOrder.BasicCost * 50 * OPENED_STAGES;
                            if (_wholesaleActiveOrder.CurrentOrderCount == _wholesaleActiveOrder.OrderCount)
                            {
                                _wholesaleActiveOrder.IsCompleted = true;
                                _wholesalePanel.SetCompletionState(true);
                            }
                        }
                        _ordersMetaData.SaveWholesaleOrder(_wholesaleActiveOrder);
                    }
                }
                
            }
        }

        private void UpdateWholesaleUI()
        {
            _wholesalePanel.WholesaleAutomationTimeSlider.value =
                _wholesaleAutomationTimer.GetRoundedRemainingTime(0);
        }

        private void SetupNewWholesaleOrder()
        {
            List<OrderConfig> wholesaleOrders = new List<OrderConfig>();
            foreach (OrderConfig order in _wholesaleConfig.GetOrders())
            {
                if (order.Materials[0].Config.MaterialName <= _playerMetaData.CurrentMaximumMaterial)
                {
                    wholesaleOrders.Add(order);
                }
            }
            
            int orderIndex = _random.Next(wholesaleOrders.Count);
            int tryCount = 0;
            while (wholesaleOrders[orderIndex].Materials[0].Config.MaterialName == MaterialName.NONE && tryCount < 10)
            {
                tryCount++;
                orderIndex = _random.Next(wholesaleOrders.Count);
            }
            ActiveOrder activeWholesaleOrder = new ActiveOrder(_wholesaleClient,
                wholesaleOrders[orderIndex], 0);
            activeWholesaleOrder.OrderTimer = new Timer(MINUTE_MULTIPLER * WHOLESALE_ORDER_TIME);
            activeWholesaleOrder.OrderCount = WHOLESALE_COUNT;
            SetupOrderView(_wholesalePanel, activeWholesaleOrder);
            _wholesalePanel.SetOrderPanelState(true);
            _wholesaleActiveOrder = activeWholesaleOrder;
            _ordersMetaData.SaveWholesaleOrder(_wholesaleActiveOrder);

        }

        private void SetOrdersCount()
        {
            _maxOrdersCount = 1;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.MultiTask))
            {
                _maxOrdersCount += (int)_upgradesMetaData.Upgrades[UpgradeName.MultiTask].GetUpgradeData();
            }

            int startIndex = _activeOrdersCount;
            for (int i = startIndex; i < _maxOrdersCount; i++)
            {
                if (_emptyOrderPanels.Count >= i+1)
                {
                    _emptyOrderPanels[i].SetLockedState(false);
                }
                else
                {
                    _emptyOrderPanels[i].SetLockedState(true);
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
                        if (!_isTutorialActive)
                        {
                            if (_currentActiveOrder != null)
                            {
                                if (_currentActiveOrder == keyValuePair.Key)
                                {
                                    _ordersEventBus.OnOrderEnded?.Invoke(keyValuePair.Value);
                                }
                            }
                            RemoveOrder(keyValuePair.Key, REMOVE_TIME);
                        }
                        keyValuePair.Value.OrderTimer = null;
                    }
                }
            }

            if (_wholesaleActiveOrder != null)
            {
                if (_wholesalePanel != null)
                {
                    UpdateOrderSlider(_wholesalePanel, _wholesaleActiveOrder);
                }

                if (_wholesaleActiveOrder.OrderTimer != null)
                {
                    if (_wholesaleActiveOrder.OrderTimer.Wait())
                    {
                        if (_currentActiveOrder != null)
                        {
                            if (_currentActiveOrder == _wholesalePanel)
                            {
                                _ordersEventBus.OnOrderEnded?.Invoke(_wholesaleActiveOrder);
                            }
                        }
                        _wholesaleActiveOrder.OrderTimer = null;
                        _wholesalePanel.ActiveOrder = null;
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
                activeOrder = _wholesaleActiveOrder;
                _ordersMetaData.WholesaleOrder = null;
            }
            _ordersEventBus.OnOrdersBoardSpaceChanged?.Invoke(true);
            _ordersEventBus.OnOrderRemoved?.Invoke();
        }

        private void SetupOrderView(OrderPanelView orderPanelView, ActiveOrder activeOrder)
        {
            //PlayerPrefs.DeleteAll();
            orderPanelView.OrderName.text = activeOrder.Name;
            orderPanelView.OrderDesc.text = activeOrder.Description.Description;
            Sprite orderIcon = _orderSpritesContainer.OrderSpritesDict[activeOrder.OrderType]
                .ItemSpritesDict[activeOrder.Materials[0].Config.MaterialName].OrderSprite;
            if (orderIcon != null)
            {
                orderPanelView.OrderIcon.sprite = orderIcon;
            }
            orderPanelView.OrderMaterial1View.MaterialName.text =
                activeOrder.Materials[0].Config.Name;
            orderPanelView.OrderMaterial1View.MaterialImage.sprite =
                activeOrder.Materials[0].Config.Sprite;
            int materialEconomy = 0;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.Economy))
            {
                materialEconomy = (int)Mathf.Ceil(activeOrder.Materials[0].Count *
                                                  _upgradesMetaData.Upgrades[UpgradeName.Economy].GetUpgradeData());
            }
            int requiredMaterialCount = activeOrder.Materials[0].Count - materialEconomy;
            if (requiredMaterialCount < 1)
            {
                requiredMaterialCount = 1;
            }
            orderPanelView.OrderMaterial1View.MaterialCount.text =
                $"{requiredMaterialCount} шт.";
            orderPanelView.OrderTimeSlider.value = 1f;
            orderPanelView.ActiveOrder = activeOrder;
            orderPanelView.SetInitialCompletionState(activeOrder.IsCompleted);
            
        }

        private void UpdateOrderSlider(OrderPanelView orderPanelView, ActiveOrder activeOrder)
        {
            if (activeOrder.OrderTimer != null)
            {
                float orderTime;
                if (orderPanelView.ActiveOrder.OrderCount == 0)
                {
                    orderTime = DEFAULT_ORDER_TIME * MINUTE_MULTIPLER;
                }
                else
                {
                    orderTime = WHOLESALE_ORDER_TIME * MINUTE_MULTIPLER;
                }
                orderPanelView.OrderTimeSlider.value =
                    activeOrder.OrderTimer.GetRemainingTime() / orderTime;
            }
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
                        if (material.Config.MaterialName != MaterialName.Metal)
                        {
                            _tutorialEventBus.OnOrderMaterialUnavaliable?.Invoke();
                        }
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
            _tutorialEventBus.OnOrderSelected?.Invoke();
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
            _tutorialEventBus.OnOrderSubmitStarted?.Invoke();
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
                _ordersMetaData.SaveWholesaleOrder(_wholesaleActiveOrder);
                _currentActiveOrder = null;
            }
        }
    }
}

