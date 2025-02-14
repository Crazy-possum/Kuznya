using GameCoreModule;
using MAEngine;
using Orders;
using System;
using UnityEngine;
using Zenject;

namespace Progression
{
    public class ProgressionOperator : IAction, IInitialisation, ICleanUp
    {
        private ProgressionData _data;
        private ProgressionContainer _container;
        private ProgressionEvents _progressionEvents;

        [Inject]
        public void Construct(ProgressionData data, ProgressionContainer container,
            ProgressionEvents progressionEvents)
        {
            _data = data;
            _container = container;
            _progressionEvents = progressionEvents;
        }

        public void Initialisation()
        {
            _progressionEvents.OnProgressionDataChanged += UpdateProgressData;
            LoadData();

        }

        public void Cleanup()
        {
            _progressionEvents.OnProgressionDataChanged -= UpdateProgressData;
        }


        private void UpdateProgressData(ProgressionData data)
        {
            Debug.Log(_data.OrdersMeta.GetClientsCount());
            _data = data;
            Debug.Log(_data.OrdersMeta.GetClientsCount());
        }

        private void LoadData()
        {
            _data = _container.LoadProgress();
            //Debug.Log($"Data loaded : {_data.PlayerMetaData.CurrentMaximumMaterial}");
            if (_data.OrdersMeta == null)
            {
                OrdersMetaData ordersMetaData = new OrdersMetaData();
                ordersMetaData.Initialize();
                _data.OrdersMeta = ordersMetaData;
            }
            _progressionEvents.OnProgressionDataLoaded?.Invoke(_data);
        }
    }
}
