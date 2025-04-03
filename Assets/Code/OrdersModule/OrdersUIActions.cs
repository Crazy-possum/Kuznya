using Economy;
using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using UnityEngine;
using Zenject;

namespace Orders
{
    public class OrdersUIActions : IAction, IInitialisation, ICleanUp, IFixedExecute
    {
        private const float HIDDEN_POSITION_Y = -70;
        private const float VISIBLE_POSITION_Y = 130;
        private const float REMOVING_MATERIAL_LIFETIME = 1.5f;
        
        private OrdersView _ordersView;
        private EconomyEventBus _economyEventBus;
        private GameEventBus _gameEventBus;
        
        private bool _isInventoryShowed;
        private Timer _removingMaterialLifetime;
        private Vector2 _targetPosition;
        private StorableMaterialView _currentlySpawnedRemovingMaterial;
        
        [Inject]
        public void Construct(OrdersView orderView, EconomyEventBus economyEventBus,
            GameEventBus gameEventBus)
        {
            _ordersView = orderView;
            _economyEventBus = economyEventBus;
            _gameEventBus = gameEventBus;
        }
        
        public void Initialisation()
        {
            SubscribeView();
            HideInventory();
            _ordersView.ResizeButton.Button.onClick.AddListener(ResizeInventory);
            _economyEventBus.OnMaterialRemoved += ShowMaterialRemoving;
        }

        public void Cleanup()
        {
            UnSubscribeView();
            _ordersView.ResizeButton.Button.onClick.RemoveListener(ResizeInventory);
            _economyEventBus.OnMaterialRemoved -= ShowMaterialRemoving;
        }
        
        public void FixedExecute(float fixedDeltaTime)
        {
            CheckRemovingMaterialActions();
        }

        private void SubscribeView()
        {
            _economyEventBus.OnMaterialInitialization += _ordersView.InitializeMaterial;
            _economyEventBus.OnMaterialUpdateInfo += _ordersView.UpdateMaterialInfo;
        }
        
        private void UnSubscribeView()
        {
            _economyEventBus.OnMaterialInitialization -= _ordersView.InitializeMaterial;
            _economyEventBus.OnMaterialUpdateInfo -= _ordersView.UpdateMaterialInfo;
        }
        
        private void ResizeInventory()
        {
            RectTransform resizeButtonImageRect =
                _ordersView.ResizeButton.ButtonImage.gameObject.GetComponent<RectTransform>();
            if (_isInventoryShowed)
            {
                resizeButtonImageRect.rotation = Quaternion.Euler(0f, 0f, 0f);
                HideInventory();
            }
            else
            {
                resizeButtonImageRect.rotation = Quaternion.Euler(0f, 0f, 180f);
                ShowInventory();
            }
        }
        
        private void ShowInventory()
        {
            _ordersView.MaterialsPanelRect.anchoredPosition = new Vector2(0, VISIBLE_POSITION_Y);
            _isInventoryShowed = true;
        }

        private void HideInventory()
        {
            _ordersView.MaterialsPanelRect.anchoredPosition = new Vector2(0, HIDDEN_POSITION_Y);
            _isInventoryShowed = false;
        }
        
        private void ShowMaterialRemoving(MaterialName materialName, int value)
        {
            string materialRemovingText = $"- {value} шт.";
            StorableMaterialView inventoryMaterialView = _ordersView.StorableMaterials[materialName];
            Sprite materialIcon = inventoryMaterialView.MaterialImage.sprite;
            Transform materialsLayout = inventoryMaterialView.MaterialRectTransform.parent;
            Vector2 startAnchoredPos = inventoryMaterialView.MaterialRectTransform.anchoredPosition;
            
            GameObjectSpawnCallback callback = new GameObjectSpawnCallback();
            _gameEventBus.OnSpawnObject?.Invoke(PrefabID.UIRemovingMaterialIcon,
                Vector3.zero, materialsLayout, callback);
            StorableMaterialView removingMaterialView = callback.SpawnedObject.GetComponent<StorableMaterialView>();
            removingMaterialView.CountText.text = materialRemovingText;
            removingMaterialView.MaterialImage.sprite = materialIcon;
            removingMaterialView.MaterialRectTransform.anchoredPosition = startAnchoredPos;
            _currentlySpawnedRemovingMaterial = removingMaterialView;
            _targetPosition = new Vector2(startAnchoredPos.x, startAnchoredPos.y + 200);
            _removingMaterialLifetime = new Timer(REMOVING_MATERIAL_LIFETIME);

        }
        
        private void CheckRemovingMaterialActions()
        {
            if (_removingMaterialLifetime != null)
            {
                MoveRemovingMaterialObject();
                if (_removingMaterialLifetime.Wait())
                {
                    GameObject.Destroy(_currentlySpawnedRemovingMaterial);
                }
            }
        }

        private void MoveRemovingMaterialObject()
        {
            float elapsedTime = _removingMaterialLifetime.Duration - _removingMaterialLifetime.GetRemainingTime();
            float totalTime = _removingMaterialLifetime.Duration;
            
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            
            t = Mathf.SmoothStep(0, 1, t);

            Vector2 startPosition = _currentlySpawnedRemovingMaterial.MaterialRectTransform.anchoredPosition;
            Vector2 currentPosition = Vector2.Lerp(startPosition, _targetPosition, t);
            
            _currentlySpawnedRemovingMaterial.MaterialRectTransform.anchoredPosition = currentPosition;
        }
    }
}