using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Progression;
using System.Collections.Generic;
using Zenject;

namespace Orders
{
    public class ClientsOperator : IAction, IInitialisation, ICleanUp, IFixedExecute, IPreInitialisation
    {
        private ClientsPoolConfig _clientsPoolConfig;
        private ProgressionData _progressionData;
        private ProgressionEvents _progressionEvents;

        private OrdersMetaData _ordersMetaData;
        private System.Random _random;
        private Timer _timer;
        private int _maxClients = 5;
        private int _minTimeBetweenClients = 5;
        private int _maxTimeBetweenClients = 10;
        private int _currentTimeBetweenClients;
        private ClientConfig _activeClient;
        private bool _isClientActive;


        [Inject]
        public void Construct(ClientsPoolConfig clientsPoolConfig,
            ProgressionEvents progressionEvents)
        {
            _clientsPoolConfig = clientsPoolConfig;
            _progressionEvents = progressionEvents;
        }

        public void PreInitialisation()
        {
            _progressionEvents.OnProgressionDataLoaded += SetProgressionData;
            _random = new System.Random();
            UpdateTimer();

        }

        private void SetProgressionData(ProgressionData data)
        {
            _progressionData = data;
        }

        public void Initialisation()
        {
            _ordersMetaData = _progressionData.OrdersMeta;
            if (_ordersMetaData.GetClientsCount() == 0)
            {
                AddNewClient();
            }
            SetActiveClient();

        }

        private void SetActiveClient()
        {
            if (!_isClientActive)
            {
                _activeClient = _ordersMetaData.GetFirstClient();
                if (_activeClient != null)
                {
                    _isClientActive = true;
                }
            }
            
        }

        public void Cleanup()
        {
            _progressionEvents.OnProgressionDataLoaded -= SetProgressionData;
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
                        !_ordersMetaData.CheckClientIsFree(clients[clientIndex], _activeClient)
                        && attempts < 10)
                    {
                        attempts++;
                        clientIndex = _random.Next(0, clients.Count);
                    }
                    if (attempts < 10)
                    {
                        _ordersMetaData.AddClient(clients[clientIndex]);
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

