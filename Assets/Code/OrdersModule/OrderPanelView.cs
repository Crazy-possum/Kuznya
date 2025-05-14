using Orders;
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

    [SerializeField] private TMP_Text _orderCountText;

    [SerializeField] private OrderMaterialView _orderMaterial1View;
    [SerializeField] private OrderMaterialView _orderMaterial2View;
    [SerializeField] private OrderMaterialView _orderMaterial3View;

    [SerializeField] private Sprite _inProcessSprite;
    [SerializeField] private Sprite _comletedSprite;
    [SerializeField] private GameObject _lockedVisualPanel;
    private ActiveOrder _activeOrder;

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
    public ActiveOrder ActiveOrder { get => _activeOrder; set => _activeOrder = value; }
    public GameObject LockedVisualPanel { get => _lockedVisualPanel; }
    
    public void SetOrderPanelState(bool isActive)
    {
        _activeOrderPanel.SetActive(isActive);
        if (_activeOrder == null)
        {
            SetInitialCompletionState(false);
        }
    }

    public void SetInitialCompletionState(bool isCompleted)
    {
        if (_activeOrder != null)
        {
            SetupOrderCount(_activeOrder.OrderCount, _activeOrder.CurrentOrderCount);
            _activeOrder.IsCompleted = isCompleted;
        }
        
        if (isCompleted)
        {
            _completedIndicator.sprite = _comletedSprite;
        }
        else
        {
            _completedIndicator.sprite = _inProcessSprite;
        }
    }

    public void SetCompletionState(bool isCompleted)
    {
        if (_activeOrder != null)
        {
            if (_activeOrder.CheckIsOrderCountCompleted())
            {
                _activeOrder.IsCompleted = isCompleted;
            }
            SetupOrderCount(_activeOrder.OrderCount, _activeOrder.CurrentOrderCount);
            if (_activeOrder.IsCompleted)
            {
                _completedIndicator.sprite = _comletedSprite;
            }
            else
            {
                _completedIndicator.sprite = _inProcessSprite;
            }
        }
    }
    
    public void SetupOrderCount(int count, int currentCount)
    {
        if (count > 0)
        {
            _orderCountText.text = $" {currentCount} / {count} шт.";
            _orderCountText.gameObject.SetActive(true);
        }
        else
        {
            _orderCountText.gameObject.SetActive(false);
        }
    }

    public void SetLockedState(bool isLocked)
    {
        _lockedVisualPanel.SetActive(isLocked);
    }
}
