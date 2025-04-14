using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Progression;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Orders
{
    public class ClientsOperator : IAction, IInitialisation, ICleanUp,
        IFixedExecute
    {
        private ClientsPoolConfig _clientsPoolConfig;
        private ProgressionData _progressionData;
        private OrdersEventBus _ordersEventBus;
        private GameConfig _gameConfig;

        private OrdersMetaData _ordersMetaData;
        private UpgradesMetaData _upgradesMetaData;
        private System.Random _random;
        private Timer _timer;
        private int _currentTimeBetweenClients;
        private bool _isClientActive;


        [Inject]
        public void Construct(ClientsPoolConfig clientsPoolConfig, 
            ProgressionData progressionData,
            OrdersEventBus ordersEventBus, GameConfig gameConfig)
        {
            _clientsPoolConfig = clientsPoolConfig;
            _progressionData = progressionData;
            _ordersEventBus = ordersEventBus;
            _gameConfig = gameConfig;
        }

        public void Initialisation()
        {
            _ordersEventBus.OnClientDeactivated += DeactivateClient;
            _random = new System.Random();
            _ordersMetaData = _progressionData.OrdersMeta;
            _upgradesMetaData = _progressionData.UpgradesMeta;
            UpdateTimer();
            LoadClients();
        }

        private void LoadClients()
        {
            if (_ordersMetaData.ActiveClient != null)
            {
                _isClientActive = true;
                _ordersEventBus.OnClientAdded?.Invoke(_ordersMetaData.ActiveClient);
            }
            if (_ordersMetaData.GetClientsCount() != 0)
            {
                foreach (ClientConfig clientConfig in _ordersMetaData.ActiveClients)
                {
                    _ordersEventBus.OnClientAdded?.Invoke(clientConfig);
                }
            }
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
                if (_ordersMetaData.GetClientsCount() < _gameConfig.MaxClients)
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
                        _ordersEventBus.OnClientAdded?.Invoke(clients[clientIndex]);
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
            int minTimeBetweenClients = _gameConfig.MinTimeBetweenClients -
                                        (int)Mathf.Ceil(_gameConfig.MinTimeBetweenClients * _upgradesMetaData.
                                            Upgrades[UpgradeName.ClientsSign].GetUpgradeData());
            int maxTimeBetweenClients = _gameConfig.MaxTimeBetweenClients -
                                        (int)Mathf.Ceil(_gameConfig.MaxTimeBetweenClients * _upgradesMetaData.
                                            Upgrades[UpgradeName.ClientsSign].GetUpgradeData());;
            _currentTimeBetweenClients = _random.Next(minTimeBetweenClients,
                maxTimeBetweenClients);
            _timer = new Timer(_currentTimeBetweenClients);
        }

    }
}

