using GameCoreModule;
using MAEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Progression
{
    public class ProgressionOperator : IAction, IPreInitialisation, ICleanUp
    {
        private ProgressionData _data;
        private ProgressionContainer _container;
        private ProgressionEvents _progressionEvents;
        [Inject]
        public void Construct(ProgressionData data, ProgressionContainer container,
            ProgressionEvents progressionEvents, StorageMaterialsConfig storageMaterialsConfig)
        {
            _data = data;
            _container = container;
            _progressionEvents = progressionEvents;
        }

        public void PreInitialisation()
        {
            _progressionEvents.OnProgressionDataChanged += UpdateProgressData;
            _progressionEvents.OnProgressionDataCleared += ClearProgressData;
            LoadData();

        }

        public void Cleanup()
        {
            _progressionEvents.OnProgressionDataChanged -= UpdateProgressData;
            _progressionEvents.OnProgressionDataCleared -= ClearProgressData;
        }

        private void UpdateProgressData(ProgressionData data)
        {
            //_data = data;
            SaveData();
        }

        private void LoadData()
        {
            _data.PlayerMetaData = _container.PlayerMetaData;
            if (_data.PlayerMetaData == null)
            {
                PlayerMetaData playerMetaData = new PlayerMetaData();
                playerMetaData.Initialize();
                _data.PlayerMetaData = playerMetaData;
            }
            else
            {
                _data.PlayerMetaData.Initialize();
            }

            _data.OrdersMeta = _container.OrdersMeta;
            if (_data.OrdersMeta == null)
            {
                OrdersMetaData ordersMetaData = new OrdersMetaData();
                ordersMetaData.Initialize();
                _data.OrdersMeta = ordersMetaData;
            }
            
            _data.UpgradesMeta = _container.UpgradesMeta;
            if (_data.UpgradesMeta == null)
            {
                UpgradesMetaData upgradesMetaData = new UpgradesMetaData();
                upgradesMetaData.Initialize();
                _data.UpgradesMeta = upgradesMetaData;
            }
            //_progressionEvents.OnProgressionDataLoaded?.Invoke(_data);
        }

        private void SaveData()
        {
            _container.SaveData(_data);
        }
        
        private void ClearProgressData()
        {
            _data.ClearData();
            SaveData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
