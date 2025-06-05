using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Progression
{
    public class ProgressionOperator : IAction, IPreInitialisation, ICleanUp, IFixedExecute
    {
        private ProgressionData _data;
        private ProgressionContainer _container;
        private ProgressionEvents _progressionEvents;
        private SaveLoadEventBus _saveLoadEventBus;
        private ClientsPoolConfig _clientsPoolConfig;
        private Timer _saveTimer;
        
        [Inject]
        public void Construct(ProgressionData data, ProgressionContainer container,
            ProgressionEvents progressionEvents, StorageMaterialsConfig storageMaterialsConfig,
            SaveLoadEventBus saveLoadEventBus, ClientsPoolConfig clientsPoolConfig)
        {
            _data = data;
            _container = container;
            _progressionEvents = progressionEvents;
            _saveLoadEventBus = saveLoadEventBus;
            _clientsPoolConfig = clientsPoolConfig;
        }

        public void PreInitialisation()
        {
            _progressionEvents.OnProgressionDataChanged += UpdateProgressData;
            _progressionEvents.OnProgressionDataCleared += ClearProgressData;
            _saveTimer = new Timer(1f);
            LoadData();

        }
        
        public void FixedExecute(float fixedDeltaTime)
        {
            if (_saveTimer != null)
            {
                if (_saveTimer.Wait())
                {
                    SaveData();
                }
            }
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
            //_data.PlayerMetaData = _container.PlayerMetaData;
            if (_data.PlayerMetaData == null)
            {
                PlayerMetaData playerMetaData = new PlayerMetaData();
                playerMetaData.Initialize(_saveLoadEventBus);
                _data.PlayerMetaData = playerMetaData;
            }

            //_data.OrdersMeta = _container.OrdersMeta;
            if (_data.OrdersMeta == null)
            {
                OrdersMetaData ordersMetaData = new OrdersMetaData();
                ordersMetaData.Initialize(_saveLoadEventBus, _clientsPoolConfig);
                _data.OrdersMeta = ordersMetaData;
            }
            
            //_data.UpgradesMeta = _container.UpgradesMeta;
            if (_data.UpgradesMeta == null)
            {
                UpgradesMetaData upgradesMetaData = new UpgradesMetaData();
                upgradesMetaData.Initialize(_saveLoadEventBus);
                _data.UpgradesMeta = upgradesMetaData;
            }
            //_progressionEvents.OnProgressionDataLoaded?.Invoke(_data);
        }

        private void SaveData()
        {
            //_container.SaveData(_data);
            _data.PlayerMetaData.SaveData(_saveLoadEventBus);
            _data.OrdersMeta.SaveData(_saveLoadEventBus);
            _data.UpgradesMeta.SaveData(_saveLoadEventBus);
            _saveLoadEventBus.OnSaveData?.Invoke();
            //Debug.Log("Saved data");
        }
        
        private void ClearProgressData()
        {
            _data.ClearData();
            SaveData();
            _saveLoadEventBus.OnClearData?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
