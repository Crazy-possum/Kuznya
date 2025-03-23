using Orders;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderConfig", menuName = "Configs/OrderConfig")]
public class OrderConfig : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private List<OrderText> _descriptions;
    [SerializeField] private Sprite _orderIcon;
    [SerializeField] private List<ForgingMaterial> _materials;
    [SerializeField] private int _orderTime;
    [SerializeField] private int _basicCost;

    public string Name { get => _name; }
    public List<OrderText> Descriptions { get => _descriptions; }
    public Sprite OrderIcon { get => _orderIcon; }
    public List<ForgingMaterial> Materials { get => _materials; }
    public int OrderTime { get => _orderTime; }
    public int BasicCost { get => _basicCost; }
}
