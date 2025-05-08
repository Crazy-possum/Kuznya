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
        private PlayerMetaData _playerMetaData;
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
            _playerMetaData = _progressionData.PlayerMetaData;
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
                int maxClients = _gameConfig.MaxClients;
                if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.LargeRoom))
                {
                    maxClients += (int)_upgradesMetaData.Upgrades[UpgradeName.LargeRoom].GetUpgradeData();
                }
                if (_ordersMetaData.GetClientsCount() < maxClients)
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
                int clientTier = 0;
                clientTier = GetClientType();
                ClientType clientType = (ClientType)clientTier;
                if (clients.Count > 0)
                {
                    int attempts = 0;
                    clientIndex = _random.Next(0, clients.Count);
                    while (
                        !_ordersMetaData.CheckClientIsFree(clients[clientIndex],
                        _ordersMetaData.ActiveClient)
                        && attempts < 10 && clients[clientIndex].ClientType != clientType)
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

        private int GetClientType()
        {
            int tierIndex = 0;
            float tier1Probability = 0.2f;
            float tier2Probability = 0.3f;
            float tier3Probability = 0.2f;
            float tier4Probability = 0.2f;
            float tier5Probability = 0.1f;

            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.ClientsPrestige))
            {
                switch (_upgradesMetaData.Upgrades[UpgradeName.ClientsPrestige].GetUpgradeData())
                {
                    case 1f:
                        tier1Probability = 0.185f;
                        tier2Probability = 0.285f;
                        tier3Probability = 0.2f;
                        tier4Probability = 0.215f;
                        tier5Probability = 0.115f;
                        break;
                    case 2f:
                        tier1Probability = 0.17f;
                        tier2Probability = 0.27f;
                        tier3Probability = 0.2f;
                        tier4Probability = 0.23f;
                        tier5Probability = 0.13f;
                        break;
                    case 3f:
                        tier1Probability = 0.155f;
                        tier2Probability = 0.255f;
                        tier3Probability = 0.2f;
                        tier4Probability = 0.245f;
                        tier5Probability = 0.145f;
                        break;
                    case 4f:
                        tier1Probability = 0.14f;
                        tier2Probability = 0.24f;
                        tier3Probability = 0.2f;
                        tier4Probability = 0.27f;
                        tier5Probability = 0.17f;
                        break;
                    case 5f:
                        tier1Probability = 0.125f;
                        tier2Probability = 0.225f;
                        tier3Probability = 0.2f;
                        tier4Probability = 0.285f;
                        tier5Probability = 0.185f;
                        break;
                }
            }
            
            int probabilityNumberLimit = 100;
            if (_playerMetaData.CurrentMaximumMaterial == MaterialName.Metal)
            {
                probabilityNumberLimit = (int)(tier1Probability * 100);
            }
            else if (_playerMetaData.CurrentMaximumMaterial == MaterialName.Tin)
            {
                probabilityNumberLimit = (int)((tier1Probability + tier2Probability) * 100);
            }
            else if (_playerMetaData.CurrentMaximumMaterial == MaterialName.Copper)
            {
                probabilityNumberLimit = (int)((tier1Probability + tier2Probability) * 100);
            }
            else if (_playerMetaData.CurrentMaximumMaterial == MaterialName.Iron)
            {
                probabilityNumberLimit = (int)((tier1Probability + tier2Probability + tier3Probability) * 100);
            }
            else if (_playerMetaData.CurrentMaximumMaterial == MaterialName.Steel)
            {
                probabilityNumberLimit =
                    (int)((tier1Probability + tier2Probability + tier3Probability + tier4Probability) * 100);
            }
            else if (_playerMetaData.CurrentMaximumMaterial == MaterialName.Silver)
            {
                probabilityNumberLimit = 100;
            }

            int probabilityNumber = _random.Next(0, probabilityNumberLimit);
            if (probabilityNumber < tier1Probability * 100)
            {
                tierIndex = 1;
            }
            else if (probabilityNumber < (tier1Probability + tier2Probability) * 100)
            {
                tierIndex = 2;
            }
            else if (probabilityNumber < (tier1Probability + tier2Probability + tier3Probability) * 100)
            {
                tierIndex = 3;
            }
            else if (probabilityNumber < 
                     (tier1Probability + tier2Probability + tier3Probability + tier4Probability) * 100)
            {
                tierIndex = 4;
            }
            else if (probabilityNumber < 100)
            {
                tierIndex = 5;
            }
            else
            {
                Debug.Log("Client probability error");
            }
            return tierIndex;
        }

        private void UpdateTimer()
        {
            int minTimeBetweenClients = _gameConfig.MinTimeBetweenClients;
            int maxTimeBetweenClients = _gameConfig.MaxTimeBetweenClients;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.ClientsSign))
            {
                minTimeBetweenClients = _gameConfig.MinTimeBetweenClients;
                maxTimeBetweenClients = _gameConfig.MaxTimeBetweenClients - (int)Mathf.Ceil(_upgradesMetaData.
                                                Upgrades[UpgradeName.ClientsSign].GetUpgradeData());
            }
            _currentTimeBetweenClients = _random.Next(minTimeBetweenClients,
                maxTimeBetweenClients);
            _timer = new Timer(_currentTimeBetweenClients);
        }

    }
}

