using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrdersPoolConfig", menuName = "Configs/OrdersPoolConfig")]
public class OrdersPoolConfig : ScriptableObject
{
    [SerializeField] private List<OrderConfig> _orders;
    
    public List<OrderConfig> GetOrders()
    {
        return _orders;
    }

    public int GetOrdersPoolCount()
    {
        return _orders.Count;
    }

    public OrderConfig GetConfig(int index)
    {
        return _orders[index];
    }
}
