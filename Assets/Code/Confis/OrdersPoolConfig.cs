using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrdersPoolConfig", menuName = "Configs/OrdersPoolConfig")]
public class OrdersPoolConfig : ScriptableObject
{
    [SerializeField] private List<OrderConfig> _orders;
}
