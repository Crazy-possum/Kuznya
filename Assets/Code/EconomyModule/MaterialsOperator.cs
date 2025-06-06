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
        private TutorialEventBus _tutorialEventBus;
        
        private PlayerMetaData _playerMetaData;
        private MaterialConfig _currentlyCollectabeMaterial;
        private Timer _materialAddingTimer;
        private UpgradesMetaData _upgradesMetaData;
        private Timer _clickCooldownTimer;
        private bool _canClickMaterial;
        private float _currentClickDelay;
        
        [Inject]
        public void Construct(ProgressionData progressionData, EconomyEventBus economyEventBus,
            GameEventBus gameEventBus,
            StorageMaterialsConfig storageConfig, MaterialsUIView materialsUIView,
            TutorialEventBus tutorialEventBus)
        {
            _progressionData = progressionData;
            _economyEventBus = economyEventBus;
            _gameEventBus = gameEventBus;
            _storageConfig = storageConfig;
            _materialsUIView = materialsUIView;
            _tutorialEventBus = tutorialEventBus;
        }
        
        public void Initialisation()
        {
            _playerMetaData = _progressionData.PlayerMetaData;
            _upgradesMetaData = _progressionData.UpgradesMeta;
            _playerMetaData.InitializeMaterials(_storageConfig);
            SetInitialCollectableMaterial(_playerMetaData.CurrentMaximumMaterial);
            _materialAddingTimer = new Timer(_playerMetaData.MaterialAddingDeltaTime);
            _economyEventBus.OnMaterialRemoved += RemoveMaterial;
            _economyEventBus.OnCollectableMaterialChanged += ChangeCollectableMaterial;
            _materialsUIView.MaterialsClickerButton.onClick.AddListener(ClickMaterialAction);
            if (!_playerMetaData.TutorialInfo.IsTutorialStarted)
            {
                _tutorialEventBus.OnTutorialStarted += AddStartMaterials;
            }
            _clickCooldownTimer = new Timer(1);
            _canClickMaterial = true;
            InitializeUI();
        }

        public void Cleanup()
        {
            _economyEventBus.OnMaterialRemoved -= RemoveMaterial;
            _economyEventBus.OnCollectableMaterialChanged -= ChangeCollectableMaterial;
            _materialsUIView.MaterialsClickerButton.onClick.RemoveListener(ClickMaterialAction);
            _materialsUIView.CleanView();
        }

        public void FixedExecute(float fixedDeltaTime)
        {
            if (_materialAddingTimer != null)
            {
                if (_materialAddingTimer.Wait())
                {
                    AddMaterial(_currentlyCollectabeMaterial);
                }
            }

            if (_clickCooldownTimer != null)
            {
                if (_clickCooldownTimer.Wait() && !_canClickMaterial)
                {
                    _canClickMaterial = true;
                }
            }

            UpdateUI();
        }
        
        private void AddStartMaterials()
        {
            AddMaterial(_currentlyCollectabeMaterial);
            AddMaterial(_currentlyCollectabeMaterial);
            AddMaterial(_currentlyCollectabeMaterial);
            AddMaterial(_currentlyCollectabeMaterial);
            _tutorialEventBus.OnTutorialStarted -= AddStartMaterials;
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
                _economyEventBus.OnMaterialInitialization?.Invoke(materialConfig, materialCount);

            }
        }

        private void UpdateUI()
        {
            SetUnlockedMaterials();
            
            foreach (MaterialConfig materialConfig in _storageConfig.StorableMaterials)
            {
                int materialCount = _playerMetaData.Materials[materialConfig.MaterialName];
                _materialsUIView.UpdateMaterialInfo(materialConfig.MaterialName, materialCount,
                    _currentlyCollectabeMaterial.MaterialName);
                _economyEventBus.OnMaterialUpdateInfo?.Invoke(materialConfig.MaterialName, materialCount,
                    _currentlyCollectabeMaterial.MaterialName);
                
            }

            UpdateCooldownSlder();
        }

        private void UpdateCooldownSlder()
        {
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.ExtraDelivery))
            {
                _materialsUIView.CooldownSlider.gameObject.SetActive(true);
            }
            if (!_canClickMaterial)
            {
                _materialsUIView.CooldownSlider.maxValue = _currentClickDelay;
                _materialsUIView.CooldownSlider.value = _currentClickDelay - _clickCooldownTimer.GetRemainingTime();
            }
            else
            {
                _materialsUIView.CooldownSlider.value = _materialsUIView.CooldownSlider.maxValue;
            }
            
        }

        private void SetUnlockedMaterials()
        {
            foreach (MaterialConfig materialConfig in _storageConfig.StorableMaterials)
            {
                bool isUnlocked = materialConfig.MaterialName <= _playerMetaData.CurrentMaximumMaterial;
                _materialsUIView.SetMaterialButtonUnlocked(materialConfig.MaterialName, isUnlocked);
            }
        }

        private void ChangeCollectableMaterial(MaterialConfig materialConfig)
        {
            if (_currentlyCollectabeMaterial != materialConfig)
            {
                _tutorialEventBus.OnMaterialChanged?.Invoke();
                _currentlyCollectabeMaterial = materialConfig;
            }
            else
            {
                ClickMaterialAction();
            }
                
        }

        private void AddMaterial(MaterialConfig currentlyCollectabeMaterial)
        {
            if (_playerMetaData.Materials.IsContainsKey(currentlyCollectabeMaterial.MaterialName))
            {
                _playerMetaData.Materials[currentlyCollectabeMaterial.MaterialName]++;
                GameObjectSpawnCallback callback = new GameObjectSpawnCallback();
                _gameEventBus.OnSpawnObjectWithoutRoot?.Invoke(PrefabID.UIAddingMaterialText, Vector3.zero, callback);
                if (callback.SpawnedObject != null)
                {
                    InitializeTextObject(callback.SpawnedObject);
                }
            }
        }

        private void RemoveMaterial(MaterialName materialName, int count)
        {
            _playerMetaData.Materials[materialName] -= count;
        }
        
        private void InitializeTextObject(GameObject textObject)
        {
            textObject.transform.SetParent(_materialsUIView.CountRoot);
            CountTextView textView = textObject.GetComponent<CountTextView>();
            textView.InitializeView(_materialsUIView);
            string materialAddingText = $"+{1} {_currentlyCollectabeMaterial.Name}";
            textView.Text.text = materialAddingText;
            _materialsUIView.AddTextToList(textView);
            textObject.transform.localScale = Vector3.one;
        }
        
        private void ClickMaterialAction()
        {
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.ExtraDelivery))
            {
                if (_upgradesMetaData.Upgrades[UpgradeName.ExtraDelivery].IsUpgradeActive)
                {
                    if (_clickCooldownTimer != null)
                    {
                        if (_canClickMaterial)
                        {
                            _currentClickDelay = _upgradesMetaData.Upgrades[UpgradeName.ExtraDelivery].GetUpgradeData();
                            _clickCooldownTimer =
                                new Timer(_currentClickDelay);
                            AddMaterial(_currentlyCollectabeMaterial);
                            _canClickMaterial = false;
                        }
                    }
                    else
                    {
                        _currentClickDelay = _upgradesMetaData.Upgrades[UpgradeName.ExtraDelivery].GetUpgradeData();
                        _clickCooldownTimer =
                            new Timer(_currentClickDelay);
                        AddMaterial(_currentlyCollectabeMaterial);
                    }
                }
            }
        }
    }
}