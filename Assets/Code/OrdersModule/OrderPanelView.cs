using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderPanelView : MonoBehaviour
{
    [SerializeField] private GameObject _activeOrderPanel;
    [SerializeField] private Button _orderButton;
    [SerializeField] private Slider _orderTimeSlider;
    [SerializeField] private TMP_Text _orderName;
    [SerializeField] private TMP_Text _orderDesc;
    [SerializeField] private Image _completedIndicator;
    [SerializeField] private Image _orderIcon;

    [SerializeField] private OrderMaterialView _orderMaterial1View;
    [SerializeField] private OrderMaterialView _orderMaterial2View;
    [SerializeField] private OrderMaterialView _orderMaterial3View;

    public GameObject ActiveOrderPanel { get => _activeOrderPanel; }
    public Button OrderButton { get => _orderButton; }
    public Slider OrderTimeSlider { get => _orderTimeSlider; }
    public TMP_Text OrderName { get => _orderName; }
    public TMP_Text OrderDesc { get => _orderDesc; }
    public Image CompletedIndicator { get => _completedIndicator; }
    public Image OrderIcon { get => _orderIcon; }
    public OrderMaterialView OrderMaterial1View { get => _orderMaterial1View; }
    public OrderMaterialView OrderMaterial2View { get => _orderMaterial2View; }
    public OrderMaterialView OrderMaterial3View { get => _orderMaterial3View; }
}
