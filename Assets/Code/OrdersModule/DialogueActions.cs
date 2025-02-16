using GameCoreModule;
using MAEngine;
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

        private OrdersMetaData _ordersMetaData;
        private ActiveOrder _currentActiveOrder;
        private System.Random _random;
        private List<GameObject> _clientIconObjects;

        [Inject]
        public void Construct(DialogueView dialogueView, ProgressionData progressionData,
            OrdersEventBus ordersEvents, GameEventBus gameEventBus)
        {
            _dialogueView = dialogueView;
            _progressionData = progressionData;
            _ordersEvents = ordersEvents;
            _gameEventBus = gameEventBus;
        }

        public void Initialisation()
        {
            _ordersMetaData = _progressionData.OrdersMeta;
            _random = new System.Random();
            _clientIconObjects = new List<GameObject>();
            _ordersEvents.OnClientAdded += AddClientIconToQueue;
            _ordersEvents.OnClientRemoved += RemoveClientIconFromQueue;
            _ordersEvents.OnClientActivated += ShowClientDialogue;
            _dialogueView.AcceptButton.onClick.AddListener(() => AcceptOrder());
            _dialogueView.RejectButton.onClick.AddListener(() => RejectOrder());
            if (_ordersMetaData.ActiveClient != null)
            {
                ShowClientDialogue();
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
                Vector3.zero, _dialogueView.ClientsQueueTransform);
        }

        private void AddClientIconObject(GameObject clientIconObject)
        {
            _gameEventBus.OnObjectSpawned -= AddClientIconObject;
            _clientIconObjects.Add(clientIconObject);
        }

        public void Cleanup()
        {
            _ordersEvents.OnClientActivated -= ShowClientDialogue;
        }

        private void DialogueStartActions(ClientConfig client)
        {
            _dialogueView.DialoguePanel.SetActive(true);
            _dialogueView.ClientImage.sprite = client.ClientSprite;
            _dialogueView.TitleText.text = client.Name;
            SetCurrentOrder(client.Orders);
        }

        private void SetCurrentOrder(OrdersPoolConfig orders)
        {
            int orderIndex = _random.Next(0, orders.GetOrdersPoolCount());
            OrderConfig orderConfig = orders.GetConfig(orderIndex);
            int descriptionID = _random.Next(0, orderConfig.Descriptions.Count);
            _dialogueView.DescriptionText.text =
                orderConfig.Descriptions[descriptionID].Text;
            _currentActiveOrder = new ActiveOrder(orderConfig, descriptionID);
        }

        private void DialogueEndActions()
        {
            _dialogueView.DialoguePanel.SetActive(false);
            _currentActiveOrder = null;
            _ordersEvents.OnClientDeactivated?.Invoke();
        }

        private void ShowClientDialogue()
        {
            ClientConfig client = _ordersMetaData.ActiveClient;
            DialogueStartActions(client);
        }

        private void RejectOrder()
        {
            DialogueEndActions();
        }

        private void AcceptOrder()
        {
            _ordersMetaData.ActiveOrders.Add(_currentActiveOrder);
            Debug.Log(_currentActiveOrder.Name);
            _ordersEvents.OnOrderAdded?.Invoke();
            DialogueEndActions();
        }
    }
}

