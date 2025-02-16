using GameCoreModule;
using MAEngine;
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
            _data = data;
        }

        private void LoadData()
        {
            _data.PlayerMetaData = _container.PlayerMetaData;
            _data.OrdersMeta = _container.OrdersMeta;
            if (_data.OrdersMeta == null)
            {
                OrdersMetaData ordersMetaData = new OrdersMetaData();
                ordersMetaData.Initialize();
                _data.OrdersMeta = ordersMetaData;
            }
            //_progressionEvents.OnProgressionDataLoaded?.Invoke(_data);
        }
    }
}
