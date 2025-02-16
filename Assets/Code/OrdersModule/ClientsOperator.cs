using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Progression;
using System;
using System.Collections.Generic;
using Zenject;

namespace Orders
{
    public class ClientsOperator : IAction, IInitialisation, ICleanUp,
        IFixedExecute
    {
        private ClientsPoolConfig _clientsPoolConfig;
        private ProgressionData _progressionData;
        private OrdersEventBus _ordersEventBus;

        private OrdersMetaData _ordersMetaData;
        private System.Random _random;
        private Timer _timer;
        private int _maxClients = 5;
        private int _minTimeBetweenClients = 5;
        private int _maxTimeBetweenClients = 10;
        private int _currentTimeBetweenClients;
        private bool _isClientActive;


        [Inject]
        public void Construct(ClientsPoolConfig clientsPoolConfig, 
            ProgressionData progressionData,
            OrdersEventBus ordersEventBus)
        {
            _clientsPoolConfig = clientsPoolConfig;
            _progressionData = progressionData;
            _ordersEventBus = ordersEventBus;
        }

        public void Initialisation()
        {
            _ordersEventBus.OnClientDeactivated += DeactivateClient;
            _random = new System.Random();
            _ordersMetaData = _progressionData.OrdersMeta;

            UpdateTimer();
            if (_ordersMetaData.ActiveClient != null)
            {
                _isClientActive = true;
            }
            if (_ordersMetaData.GetClientsCount() == 0)
            {
                AddNewClient();
            }
            SetActiveClient();
        }

        public void Cleanup()
        {
            _ordersEventBus.OnClientDeactivated -= DeactivateClient;
        }

        public void FixedExecute(float fixedDeltaTime)
        {
            if (!_isClientActive && 
                _ordersMetaData.GetClientsCount() > 0)
            {
                SetActiveClient();
            }
            if (_timer.Wait())
            {
                if (_ordersMetaData.GetClientsCount() < _maxClients)
                {
                    AddNewClient();
                }
                UpdateTimer();
            }
        }

        private void SetActiveClient()
        {
            if (!_isClientActive)
            {
                _ordersMetaData.ActiveClient = _ordersMetaData.GetFirstClient();
                if (_ordersMetaData.ActiveClient != null)
                {
                    _isClientActive = true;
                    _ordersEventBus.OnClientRemoved?.Invoke();
                    _ordersEventBus.OnClientActivated?.Invoke();
                }
            }
        }

        private void DeactivateClient()
        {
            _ordersMetaData.ActiveClient = null;
            _isClientActive = false;
        }

        private void AddNewClient()
        {
            if (_clientsPoolConfig.Clients.Count > 0)
            {
                List<ClientConfig> clients = new List<ClientConfig>();
                foreach (ClientConfig client in _clientsPoolConfig.Clients)
                {
                    if (client.MaterialName <= _progressionData.PlayerMetaData.CurrentMaximumMaterial)
                    {
                        clients.Add(client);
                    }
                }
                int clientIndex = 0;
                if (clients.Count > 0)
                {
                    int attempts = 0;
                    clientIndex = _random.Next(0, clients.Count);
                    while (
                        !_ordersMetaData.CheckClientIsFree(clients[clientIndex],
                        _ordersMetaData.ActiveClient)
                        && attempts < 10)
                    {
                        attempts++;
                        clientIndex = _random.Next(0, clients.Count);
                    }
                    if (attempts < 10)
                    {

                        _ordersMetaData.AddClient(clients[clientIndex]);
                        if (_isClientActive)
                        {
                            _ordersEventBus.OnClientAdded?.Invoke();
                        }
                        //Debug.Log($"{clients[clientIndex]}");
                    }
                    else
                    {
                        //Debug.Log("All clients busy");
                    }
                } 
            }
        }

        private void UpdateTimer()
        {
            _currentTimeBetweenClients = _random.Next(_minTimeBetweenClients,
                _maxTimeBetweenClients);
            _timer = new Timer(_currentTimeBetweenClients);
        }

    }
}

