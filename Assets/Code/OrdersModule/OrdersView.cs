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
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _denyButton;

    public OrderPanelView Order1PanelView { get => _order1PanelView; }
    public OrderPanelView Order2PanelView { get => _order2PanelView; }
    public OrderPanelView Order3PanelView { get => _order3PanelView; }
    public OrderPanelView Order4PanelView { get => _order4PanelView; }
    public OrderPanelView Order5PanelView { get => _order5PanelView; }
    public OrderPanelView Order6PanelView { get => _order6PanelView; }
    public OrderPanelView Order7PanelView { get => _order7PanelView; }
    public OrderPanelView Order8PanelView { get => _order8PanelView; }
    public GameObject ConfirmPanel { get => _confirmPanel; }
    public Button ConfirmButton { get => _confirmButton; }
    public Button DenyButton { get => _denyButton; }
    
}
