using GameCoreModule;
using MAEngine;
using MainGUI;
using Progression;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Orders
{
    public class DialogueActions : IInitialisation, ICleanUp
    {
        private DialogueView _dialogueView;
        private ProgressionData _progressionData;
        private OrdersEventBus _ordersEvents;
        private GameEventBus _gameEventBus;
        private GUIView _guiView;

        private OrdersMetaData _ordersMetaData;
        private ActiveOrder _currentActiveOrder;
        private System.Random _random;
        private List<GameObject> _clientIconObjects;

        [Inject]
        public void Construct(DialogueView dialogueView, ProgressionData progressionData,
            OrdersEventBus ordersEvents, GameEventBus gameEventBus, GUIView guiView)
        {
            _dialogueView = dialogueView;
            _progressionData = progressionData;
            _ordersEvents = ordersEvents;
            _gameEventBus = gameEventBus;
            _guiView = guiView;
        }

        public void Initialisation()
        {
            _ordersMetaData = _progressionData.OrdersMeta;
            _random = new System.Random();
            _clientIconObjects = new List<GameObject>();
            _ordersEvents.OnClientAdded += AddClientIconToQueue;
            _ordersEvents.OnClientRemoved += RemoveClientIconFromQueue;
            _ordersEvents.OnClientActivated += SetClientDialogue;
            _dialogueView.AcceptButton.onClick.AddListener(() => AcceptOrder());
            _dialogueView.RejectButton.onClick.AddListener(() => RejectOrder());
            _dialogueView.DialogueStartButton.onClick.AddListener(() => StartDialogue());
            _dialogueView.NextButton.onClick.AddListener(() => ShowDescriptionText());
            _dialogueView.PrevButton.onClick.AddListener(() => ShowDefaultText());
            if (_ordersMetaData.ActiveClient != null)
            {
                SetClientDialogue();
            }
            else
            {
                _dialogueView.DialoguePanel.SetActive(false);
            }
        }

        private void RemoveClientIconFromQueue()
        {
            if (_clientIconObjects.Count > 0)
            {
                GameObject clientIcon = _clientIconObjects[0];
                _clientIconObjects.Remove(clientIcon);
                GameObject.Destroy(clientIcon);
            }
        }

        private void AddClientIconToQueue()
        {
            _gameEventBus.OnObjectSpawned += AddClientIconObject;
            _gameEventBus.OnSpawnObject?.Invoke(PrefabID.ClientIcon,
                Vector3.zero, _guiView.ClientsQueueTransform);
        }

        private void AddClientIconObject(GameObject clientIconObject)
        {
            _gameEventBus.OnObjectSpawned -= AddClientIconObject;
            _clientIconObjects.Add(clientIconObject);
        }

        public void Cleanup()
        {
            _ordersEvents.OnClientActivated -= SetClientDialogue;
        }

        private void DialogueStartActions(ClientConfig client)
        {
            _dialogueView.ClientImage.gameObject.SetActive(true);
            _dialogueView.ClientImage.sprite = client.ClientSprite;
            _dialogueView.TitleText.text = client.Name;
            SetCurrentOrder(client.Orders);
        }

        private void SetCurrentOrder(OrdersPoolConfig orders)
        {
            int orderIndex = _random.Next(0, orders.GetOrdersPoolCount());
            OrderConfig orderConfig = orders.GetConfig(orderIndex);
            int descriptionID = _random.Next(0, orderConfig.Descriptions.Count);
            _currentActiveOrder = new ActiveOrder(orderConfig, descriptionID);
            ShowDefaultText();
        }

        private void SetClientDialogue()
        {
            ClientConfig client = _ordersMetaData.ActiveClient;
            DialogueStartActions(client);
        }

        private void StartDialogue()
        {
            _dialogueView.DialoguePanel.SetActive(true);
            _guiView.NavigationPanel.SetActive(false);
        }

        private void RejectOrder()
        {
            DialogueEndActions();
        }

        private void AcceptOrder()
        {
            _ordersMetaData.ActiveOrders.Add(_currentActiveOrder);
            _ordersEvents.OnOrderAdded?.Invoke();
            DialogueEndActions();
        }

        private void DialogueEndActions()
        {
            _dialogueView.ClientImage.gameObject.SetActive(false);
            _dialogueView.DialoguePanel.SetActive(false);
            _guiView.NavigationPanel.SetActive(true);
            _currentActiveOrder = null;
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
    }
}

