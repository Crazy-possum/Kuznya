using System.Collections.Generic;
using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using UnityEngine;

namespace Economy
{
    public class MaterialsUIView : MonoBehaviour, IView
    {
        [SerializeField] private GameObject _panelObject;
        [SerializeField] private string _viewID;
        [SerializeField] private Transform _countRoot;
        [SerializeField] private int _maxCountTexts;
        [SerializeField] private SerializableDictionary<MaterialName, StorableMaterialView> _storableMaterials;
        private List<CountTextView> _addingCountTextList = new List<CountTextView>();
        private EconomyEventBus _economyEventBus;
        
        
        public GameObject Object { get => _panelObject; }
        public string ViewID { get => _viewID; set => _viewID = value; }
        public Transform CountRoot { get => _countRoot; }
        public SerializableDictionary<MaterialName, StorableMaterialView> StorableMaterials 
        { get => _storableMaterials; set => _storableMaterials = value; }
        public List<CountTextView> AddingCountTextList { get => _addingCountTextList; }

        public void InitializeView(EconomyEventBus economyEventBus)
        {
            _economyEventBus = economyEventBus;
        }
        
        public void CleanView()
        {
            List<StorableMaterialView> materialViews = StorableMaterials.GetAllValues();
            foreach (StorableMaterialView materialView in materialViews)
            {
                materialView.MaterialButton.onClick.RemoveAllListeners();
            }
        }

        public void AddTextToList(CountTextView text)
        {
            if (_addingCountTextList.Count >= _maxCountTexts)
            {
                GameObject textObject = _addingCountTextList[0].gameObject;
                _addingCountTextList.Remove(_addingCountTextList[0]);
                Destroy(textObject);
            }
            _addingCountTextList.Add(text);
        }

        public void InitializeMaterial(MaterialConfig materialConfig, int materialCount)
        {
            if (_storableMaterials.IsContainsKey(materialConfig.MaterialName))
            {
                StorableMaterialView storableMaterialView = _storableMaterials[materialConfig.MaterialName];
                storableMaterialView.MaterialImage.sprite = materialConfig.Sprite;
                storableMaterialView.UpdateMaterialCount(materialCount);
                storableMaterialView.MaterialButton.onClick.AddListener(() => SetCollectableMaterial(materialConfig));
            }
        }

        private void SetCollectableMaterial(MaterialConfig materialConfig)
        {
            _economyEventBus.OnCollectableMaterialChanged?.Invoke(materialConfig);
        }


        public void UpdateMaterialInfo(MaterialName materialName, int materialCount, MaterialName currentlyCollectable)
        {
            if (_storableMaterials.IsContainsKey(materialName))
            {
                StorableMaterialView storableMaterialView = _storableMaterials[materialName];
                storableMaterialView.UpdateMaterialCount(materialCount);
                if (materialName == currentlyCollectable)
                {
                    storableMaterialView.SetHighlight(true);
                }
                else
                {
                    storableMaterialView.SetHighlight(false);
                }
            }
        }

        public void SetMaterialButtonUnlocked(MaterialName materialName, bool isUnlocked)
        {
            _storableMaterials[materialName].MaterialButton.interactable = isUnlocked;
        }
    }
}