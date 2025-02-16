using GameCoreModule;
using MAEngine;
using Progression;
using UnityEngine;
using Zenject;

namespace Orders
{
    public class OrdersOperator : IAction, IInitialisation, ICleanUp
    {
        private ProgressionData _progressionData;
        private ProgressionEvents _progressionEvents;

        private OrdersMetaData _ordersMetaData;
        private System.Random _random;

        [Inject]
        public void Construct(ClientsPoolConfig clientsPoolConfig,
            ProgressionEvents progressionEvents, ProgressionData progressionData)
        {
            _progressionEvents = progressionEvents;
            _progressionData = progressionData;
        }


        public void Initialisation()
        {
            _random = new System.Random();
            _ordersMetaData = _progressionData.OrdersMeta;

        }

        public void Cleanup()
        {
            
        }
    }
}

