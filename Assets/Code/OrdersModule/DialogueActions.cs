using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using MainGUI;
using Progression;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Orders
{
    public class DialogueActions : IPreInitialisation, IInitialisation, ICleanUp, IFixedExecute
    {
        private const string ACCEPT_TEXT = "Принять";
        private const string NOT_ENOUTH_TEXT = "Нет места";
        private const float DIALOGUE_END_DELAY = 1f;
        private const float WARNING_DELAY = 2f;
        private const float DEFAULT_ORDER_TIME = 8f;
        private const float MINUTE_MULTIPLER = 60f;

        private DialogueView _dialogueView;
        private ProgressionData _progressionData;
        private OrdersEventBus _ordersEvents;
        private GameEventBus _gameEventBus;
        private GUIView _guiView;
        private GameConfig _gameConfig;
        private ClientsSpritesContainer _clientsSpritesContainer;
        private UIEventBus _uiEventBus;
        private TutorialEventBus _tutorialEventBus;
        private EconomyEventBus _economyEventBus;

        private OrdersMetaData _ordersMetaData;
        private PlayerMetaData _playerMetaData;
        private ActiveOrder _currentActiveOrder;
        private System.Random _random;
        private List<GameObject> _clientIconObjects;
        private float _generationDelay;
        private int _generationLength;
        private Timer _timer;
        private ClientConfig _currentClientConfig;
        private Timer _dialogueEndTimer;
        private UpgradesMetaData _upgradesMetaData;
        private Timer _warningTimer;
        private int _maxOrdersCount;

        [Inject]
        public void Construct(DialogueView dialogueView, ProgressionData progressionData,
            OrdersEventBus ordersEvents, GameEventBus gameEventBus, GUIView guiView,
            GameConfig gameConfig, ClientsSpritesContainer clientsSpritesContainer,
            UIEventBus uiEventBus, TutorialEventBus tutorialEventBus, EconomyEventBus economyEventBus)
        {
            _dialogueView = dialogueView;
            _progressionData = progressionData;
            _ordersEvents = ordersEvents;
            _gameEventBus = gameEventBus;
            _guiView = guiView;
            _gameConfig = gameConfig;
            _clientsSpritesContainer = clientsSpritesContainer;
            _uiEventBus = uiEventBus;
            _tutorialEventBus = tutorialEventBus;
            _economyEventBus = economyEventBus;
        }
        
        public void PreInitialisation()
        {
            _ordersEvents.OnClientAdded += AddClientIconToQueue;
            _ordersEvents.OnClientRemoved += RemoveClientIconFromQueue;
            _ordersEvents.OnClientActivated += SetClientDialogue;
            _ordersEvents.OnOrderRemoved += CheckOrdersCount;
            _ordersEvents.OnOrdersBoardSpaceChanged += ChangeApplyButtonState;
            _dialogueView.AcceptButton.onClick.AddListener(() => AcceptOrder());
            _dialogueView.RejectButton.onClick.AddListener(() => RejectOrder());
            _dialogueView.DialogueStartButton.onClick.AddListener(() => StartDialogue());
            _dialogueView.NextButton.onClick.AddListener(() => ShowDescriptionText());
            _dialogueView.PrevButton.onClick.AddListener(() => ShowDefaultText());
            
            
            _random = new System.Random();
            _clientIconObjects = new List<GameObject>();
            _generationLength = 20;
            _generationDelay = 1f;
            _maxOrdersCount = 1;
        }

        public void Initialisation()
        {
            _ordersMetaData = _progressionData.OrdersMeta;
            _playerMetaData = _progressionData.PlayerMetaData;
            _upgradesMetaData = _progressionData.UpgradesMeta;
            _economyEventBus.OnUpgradeApplied += CheckIsUpgradesChanged;
            _timer = new Timer(_generationDelay);
            LoadDialogue();
            SetupOrdersCount();
        }

        public void Cleanup()
        {
            _ordersEvents.OnClientAdded -= AddClientIconToQueue;
            _ordersEvents.OnClientRemoved -= RemoveClientIconFromQueue;
            _ordersEvents.OnClientActivated -= SetClientDialogue;
            _ordersEvents.OnOrderRemoved -= CheckOrdersCount;
            _ordersEvents.OnOrdersBoardSpaceChanged -= ChangeApplyButtonState;
            _dialogueView.AcceptButton.onClick.RemoveListener(() => AcceptOrder());
            _dialogueView.RejectButton.onClick.RemoveListener(() => RejectOrder());
            _dialogueView.DialogueStartButton.onClick.RemoveAllListeners();
            _dialogueView.NextButton.onClick.RemoveListener(() => ShowDescriptionText());
            _dialogueView.PrevButton.onClick.RemoveListener(() => ShowDefaultText());
        }

        public void FixedExecute(float fixedDeltaTime)
        {
            if (_timer.Wait())
            {
                GenerateText();
            }

            if (_dialogueEndTimer != null)
            {
                if (_dialogueEndTimer.Wait())
                {
                    DialogueEndActions();
                    _dialogueEndTimer = null;
                }
            }

            if (_warningTimer != null)
            {
                if (_warningTimer.Wait())
                {
                    _warningTimer = null;
                    _dialogueView.OrdersFullWarningPanel.SetActive(false);
                }
            }
        }
        
        private void CheckIsUpgradesChanged(UpgradeName upgradeName)
        {
            if (upgradeName == UpgradeName.MultiTask)
            {
                SetupOrdersCount();
            }
        }

        private void SetupOrdersCount()
        {
            _maxOrdersCount = 1;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.MultiTask))
            {
                _maxOrdersCount +=
                    (int)_upgradesMetaData.Upgrades[UpgradeName.MultiTask].GetUpgradeData();
            }
        }

        private void ChangeApplyButtonState(bool isInteractable)
        {
            //_dialogueView.AcceptButton.interactable = isInteractable;
            _dialogueView.DialogueStartButton.onClick.RemoveAllListeners();
            if (isInteractable)
            {
                _dialogueView.DialogueStartButton.onClick.AddListener(() => StartDialogue());
                _dialogueView.OrdersFullWarningPanel.gameObject.SetActive(false);
                _warningTimer = null;
            }
            else
            {
                _dialogueView.DialogueStartButton.onClick.AddListener(() => ShowOrdersFullWarning());
            }
        }

        private void ShowOrdersFullWarning()
        {
            if (_warningTimer == null)
            {
                _dialogueView.OrdersFullWarningPanel.gameObject.SetActive(true);
                _warningTimer = new Timer(WARNING_DELAY);
                //Debug.LogWarning("Orders full warning");
            }
        }

        private void LoadDialogue()
        {
            if (_ordersMetaData.ActiveClient != null)
            {
                SetClientDialogue();
                GenerateText();
            }
            else
            {
                _dialogueView.ClientImage.gameObject.SetActive(false);
                _dialogueView.DialoguePanel.SetActive(false);
            }
        }

        private void RemoveClientIconFromQueue()
        {
            if (_clientIconObjects.Count > 0)
            {
                GameObject clientIcon = _clientIconObjects[0];
                clientIcon.GetComponent<Button>().onClick.RemoveAllListeners();
                _clientIconObjects.Remove(clientIcon);
                GameObject.Destroy(clientIcon);
            }
        }

        private void AddClientIconToQueue(ClientConfig config)
        {
            _currentClientConfig = config;
            GameObjectSpawnCallback callback = new GameObjectSpawnCallback();
            _gameEventBus.OnSpawnObject?.Invoke(PrefabID.ClientIcon,
                Vector3.zero, _guiView.ClientsQueueTransform, callback);
            AddClientIconObject(callback.SpawnedObject);
        }

        private void AddClientIconObject(GameObject clientIconObject)
        {
            clientIconObject.transform.SetSiblingIndex(0);
            Image clientImage = clientIconObject.GetComponent<Image>();
            Button clientButton = clientIconObject.GetComponent<Button>();
            clientButton.onClick.AddListener(ShowDialogueScreen);
            clientImage.sprite = _clientsSpritesContainer.ClientSprites[_currentClientConfig.ClientType];
            _currentClientConfig = null;
            _clientIconObjects.Add(clientIconObject);
        }

        private void ShowDialogueScreen()
        {
            _gameEventBus.OnSetDialogueState?.Invoke();
        }
        
        private void DialogueStartActions(ClientConfig client)
        {
            _dialogueView.ClientImage.gameObject.SetActive(true);
            _dialogueView.DialogueIcon.gameObject.SetActive(true);
            _dialogueView.ClientImage.sprite = client.ClientSprite;
            _dialogueView.TitleText.text = client.Name;

            SetCurrentOrder(client);
            CheckOrdersCount();
        }

        private void CheckOrdersCount()
        {
            if (_ordersMetaData.ActiveOrders.Count >= _maxOrdersCount)
            {
                _dialogueView.AcceptButton.interactable = false;
                _dialogueView.AcceptButtonText.text = NOT_ENOUTH_TEXT;
            }
            else
            {
                _dialogueView.AcceptButton.interactable = true;
                _dialogueView.AcceptButtonText.text = ACCEPT_TEXT;
            }
        }

        private void SetCurrentOrder(ClientConfig client)
        {
            OrdersPoolConfig orders = client.Orders;
            List<OrderConfig> selectedOrders = new List<OrderConfig>();
            foreach (OrderConfig selectedOrderConfig in orders.GetOrders())
            {
                if (selectedOrderConfig.Materials[0].Config.MaterialName <= _playerMetaData.CurrentMaximumMaterial)
                {
                    selectedOrders.Add(selectedOrderConfig);
                }
            }
            int orderIndex = _random.Next(0, selectedOrders.Count);
            int probabilityOfFirstMaterial = 50;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.ClientsHolydays))
            {
                float probabilityOfFirst = _upgradesMetaData.Upgrades[UpgradeName.ClientsHolydays].GetUpgradeData();
                probabilityOfFirstMaterial = (int)probabilityOfFirst * 100;
            }
            int materialIndex = _random.Next(0, 100) < probabilityOfFirstMaterial ? 0 : 1;
            MaterialName firstMaterial = MaterialName.NONE;
            MaterialName secondMaterial = MaterialName.NONE;
            foreach (OrderConfig selectedOrder in selectedOrders)
            {
                if (firstMaterial == MaterialName.NONE)
                {
                    firstMaterial = selectedOrder.Materials[0].Config.MaterialName;
                }
                else if(firstMaterial != selectedOrder.Materials[0].Config.MaterialName)
                {
                    secondMaterial = selectedOrder.Materials[0].Config.MaterialName;
                    break;
                }
            }
            OrderConfig orderConfig = selectedOrders[orderIndex];
            if (materialIndex == 0)
            {
                int attempts = 0;
                while (orderConfig.Materials[0].Config.MaterialName != firstMaterial && attempts < 10)
                {
                    attempts++;
                    orderIndex = _random.Next(0, selectedOrders.Count);
                    orderConfig = selectedOrders[orderIndex];
                }
            }
            if (materialIndex == 1)
            {
                int attempts = 0;
                while (orderConfig.Materials[0].Config.MaterialName != secondMaterial && attempts < 10)
                {
                    attempts++;
                    orderIndex = _random.Next(0, selectedOrders.Count);
                    orderConfig = selectedOrders[orderIndex];
                }
            }
            int descriptionID = _random.Next(0, orderConfig.Descriptions.Count);
            _currentActiveOrder = new ActiveOrder(client, orderConfig, descriptionID);
            _currentActiveOrder.BasicCost = orderConfig.BasicCost;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.Cost))
            {
                _currentActiveOrder.BasicCost += 
                    (int)Mathf.Ceil(orderConfig.BasicCost *
                                    _upgradesMetaData.Upgrades[UpgradeName.Cost].GetUpgradeData());
            }
            ShowDefaultText();
        }

        private void SetClientDialogue()
        {
            if (_ordersMetaData.ActiveClient != null)
            {
                ClientConfig client = _ordersMetaData.ActiveClient;
                ResizeQueueIcon(client);
                DialogueStartActions(client);
            }
        }

        private void ResizeQueueIcon(ClientConfig client)
        {
            Vector3 scale = _clientIconObjects[0].gameObject.transform.localScale;
            _clientIconObjects[0].gameObject.transform.localScale =
                new Vector3(scale.x * 1.5f, scale.y * 1.5f, scale.z * 1.5f);
        }

        private void StartDialogue()
        {
            CheckOrdersCount();
            _dialogueView.DialoguePanel.SetActive(true);
            _dialogueView.DialogueIcon.gameObject.SetActive(false);
            _guiView.NavigationPanel.SetActive(false);
            _tutorialEventBus.OnDialogueStarted?.Invoke(_currentActiveOrder);
        }

        private void StartDialogueEndDelay()
        {
            _uiEventBus.OnFreezeUI?.Invoke();
            _dialogueEndTimer = new Timer(DIALOGUE_END_DELAY);
        }

        private void RejectOrder()
        {
            _dialogueView.HighlightRejectObject.gameObject.SetActive(true);
            StartDialogueEndDelay();
        }
        
        private void AcceptOrder()
        {
            _dialogueView.HighlightComfirmObject.SetActive(true);
            _currentActiveOrder.OrderTimer = new Timer(DEFAULT_ORDER_TIME * MINUTE_MULTIPLER);
            _ordersMetaData.SaveActiveOrder(_currentActiveOrder);
            _ordersEvents.OnOrderAdded?.Invoke();
            _tutorialEventBus.OnOrderApplied?.Invoke();
            StartDialogueEndDelay();
        }

        private void DialogueEndActions()
        {
            _uiEventBus.OnUnfreezeUI?.Invoke();
            _dialogueView.HighlightComfirmObject.SetActive(false);
            _dialogueView.HighlightRejectObject.gameObject.SetActive(false);
            _dialogueView.ClientImage.gameObject.SetActive(false);
            _dialogueView.DialoguePanel.SetActive(false);
            _guiView.NavigationPanel.SetActive(true);
            _currentActiveOrder = null;
            _ordersEvents.OnClientRemoved?.Invoke();
            _ordersEvents.OnClientDeactivated?.Invoke();
        }

        private void ShowDefaultText()
        {
            _dialogueView.DescriptionText.text = _currentActiveOrder.Description.Text;
            _dialogueView.NextButton.interactable = true;
            _dialogueView.PrevButton.interactable = false;
        }

        private void ShowDescriptionText()
        {
            _dialogueView.DescriptionText.text = _currentActiveOrder.Description.Description;
            _dialogueView.NextButton.interactable = false;
            _dialogueView.PrevButton.interactable = true;
        }

        private void GenerateText()
        {
            string[] symb = new string[]
            {
            "a", "b", "c", "d", "e",
            "f", "g", "x", "y", "z",
            " ", "h", "i", "ά", "#",
            "@", "$", "&", "%", "^",
            "β", "∂", "Ē", "€", "ū"
            };

            string generated = "";
            int charIndex = 0;
            for (int i = 0; i < _generationLength; i++)
            {
                charIndex = _random.Next(0, symb.Length);
                generated += symb[charIndex];
            }
            _dialogueView.DialogueIconText.text = generated;
        }
    }
}

