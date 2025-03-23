using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Progression;
using UnityEngine;
using Zenject;

namespace Economy
{
    public class MaterialsOperator : IAction, IInitialisation, ICleanUp, IFixedExecute
    {
        private ProgressionData _progressionData;
        private EconomyEventBus _economyEventBus;
        private GameEventBus _gameEventBus;
        private StorageMaterialsConfig _storageConfig;
        private MaterialsUIView _materialsUIView;
        
        private PlayerMetaData _playerMetaData;
        private MaterialConfig _currentlyCollectabeMaterial;
        private Timer _materialAddingTimer;
        
        [Inject]
        public void Construct(ProgressionData progressionData, EconomyEventBus economyEventBus,
            GameEventBus gameEventBus,
            StorageMaterialsConfig storageConfig, MaterialsUIView materialsUIView)
        {
            _progressionData = progressionData;
            _economyEventBus = economyEventBus;
            _gameEventBus = gameEventBus;
            _storageConfig = storageConfig;
            _materialsUIView = materialsUIView;
        }
        
        public void Initialisation()
        {
            _playerMetaData = _progressionData.PlayerMetaData;
            _playerMetaData.InitializeMaterials(_storageConfig);
            SetInitialCollectableMaterial(_playerMetaData.CurrentMaximumMaterial);
            _materialAddingTimer = new Timer(_playerMetaData.MaterialAddingDeltaTime);
            _economyEventBus.OnMaterialRemoved += RemoveMaterial;
            _economyEventBus.OnCollectableMaterialChanged += ChangeCollectableMaterial;
            _gameEventBus.OnCreatePool(PrefabID.UIAddingMaterialText);
            InitializeUI();
        }

        public void Cleanup()
        {
            _economyEventBus.OnMaterialRemoved -= RemoveMaterial;
            _economyEventBus.OnCollectableMaterialChanged -= ChangeCollectableMaterial;
            _materialsUIView.CleanView();
        }

        public void FixedExecute(float fixedDeltaTime)
        {
            if (_materialAddingTimer != null)
            {
                if (_materialAddingTimer.Wait())
                {
                    AddMaterial(_currentlyCollectabeMaterial.MaterialName);
                }
            }
            UpdateUI();
        }
        
        private void SetInitialCollectableMaterial(MaterialName currentMaximumMaterial)
        {
            MaterialConfig config = null;
            foreach (MaterialConfig materialConfig in _storageConfig.StorableMaterials)
                
            {
                if (materialConfig.MaterialName == currentMaximumMaterial)
                {
                    config = materialConfig;
                    break;
                }
            }

            if (config != null)
            {
                _currentlyCollectabeMaterial = config;
            }
            else
            {
                Debug.Log("No material in storables");
            }
        }
        
        private void InitializeUI()
        {
            _materialsUIView.InitializeView(_economyEventBus);
            foreach (MaterialConfig materialConfig in _storageConfig.StorableMaterials)
            {
                int materialCount = _playerMetaData.Materials[materialConfig.MaterialName];
                _materialsUIView.InitializeMaterial(materialConfig, materialCount);
            }
        }

        private void UpdateUI()
        {
            foreach (MaterialConfig materialConfig in _storageConfig.StorableMaterials)
            {
                int materialCount = _playerMetaData.Materials[materialConfig.MaterialName];
                _materialsUIView.UpdateMaterialCount(materialConfig.MaterialName, materialCount);
            }
        }
        
        private void ChangeCollectableMaterial(MaterialConfig materialConfig)
        {
            _currentlyCollectabeMaterial = materialConfig;
        }

        private void AddMaterial(MaterialName currentlyCollectabeMaterial)
        {
            if (_playerMetaData.Materials.ContainsKey(currentlyCollectabeMaterial))
            {
                _playerMetaData.Materials[currentlyCollectabeMaterial]++;
                _gameEventBus.OnObjectSpawnedFromPool += InitializeTextObject;
                _gameEventBus.OnSpawnObjectFromPool?.Invoke(PrefabID.UIAddingMaterialText, Vector3.zero);
            }
            
        }

        private void RemoveMaterial(MaterialName materialName, int count)
        {
            _playerMetaData.Materials[materialName] -= count;
        }
        
        private void InitializeTextObject(GameObject textObject, IPool pool)
        {
            textObject.transform.SetParent(_materialsUIView.CountRoot);
            CountTextView textView = textObject.GetComponent<CountTextView>();
            textView.InitializeView(_materialsUIView);
            textView.Text.text = 1.ToString();
            _materialsUIView.AddTextToList(textView);
            _gameEventBus.OnObjectSpawnedFromPool -= InitializeTextObject;
        }
    }
}