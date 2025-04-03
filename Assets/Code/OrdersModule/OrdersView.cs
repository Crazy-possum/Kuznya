using Economy;
using MAEngine.Extention;
using MainGUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrdersView : MonoBehaviour
{
    [SerializeField] private OrderPanelView _order1PanelView;
    [SerializeField] private OrderPanelView _order2PanelView;
    [SerializeField] private OrderPanelView _order3PanelView;
    [SerializeField] private OrderPanelView _order4PanelView;
    [SerializeField] private OrderPanelView _order5PanelView;
    [SerializeField] private OrderPanelView _order6PanelView;
    [SerializeField] private OrderPanelView _order7PanelView;
    [SerializeField] private OrderPanelView _order8PanelView;
    [SerializeField] private GameObject _confirmPanel;
    [SerializeField] private TMP_Text _confirmPanelText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _denyButton;
    [SerializeField] private SerializableDictionary<MaterialName, StorableMaterialView> _storableMaterials;
    [SerializeField] private ButtonView _resizeButton;
    [SerializeField] private RectTransform _materialsPanelRect;

    public OrderPanelView Order1PanelView { get => _order1PanelView; }
    public OrderPanelView Order2PanelView { get => _order2PanelView; }
    public OrderPanelView Order3PanelView { get => _order3PanelView; }
    public OrderPanelView Order4PanelView { get => _order4PanelView; }
    public OrderPanelView Order5PanelView { get => _order5PanelView; }
    public OrderPanelView Order6PanelView { get => _order6PanelView; }
    public OrderPanelView Order7PanelView { get => _order7PanelView; }
    public OrderPanelView Order8PanelView { get => _order8PanelView; }
    public GameObject ConfirmPanel { get => _confirmPanel; }
    public TMP_Text ConfirmPanelText { get => _confirmPanelText; }
    public Button ConfirmButton { get => _confirmButton; }
    public Button DenyButton { get => _denyButton; }
    public SerializableDictionary<MaterialName, StorableMaterialView> StorableMaterials { get => _storableMaterials; }
    public ButtonView ResizeButton { get => _resizeButton; }
    public RectTransform MaterialsPanelRect { get => _materialsPanelRect; }
    
    
    public void InitializeMaterial(MaterialConfig materialConfig, int materialCount)
    {
        if (_storableMaterials.IsContainsKey(materialConfig.MaterialName))
        {
            StorableMaterialView storableMaterialView = _storableMaterials[materialConfig.MaterialName];
            storableMaterialView.MaterialImage.sprite = materialConfig.Sprite;
            storableMaterialView.UpdateMaterialCount(materialCount);
        }
    }

    public void UpdateMaterialInfo(MaterialName materialName, int materialCount, MaterialName currentlyCollectable)
    {
        if (_storableMaterials.IsContainsKey(materialName))
        {
            StorableMaterialView storableMaterialView = _storableMaterials[materialName];
            storableMaterialView.UpdateMaterialCount(materialCount);
        }
    }
    
}
