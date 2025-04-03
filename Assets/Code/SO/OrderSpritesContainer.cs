using MAEngine.Extention;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderSpritesContainer", menuName = "SO/OrderSpritesContainer")]
public class OrderSpritesContainer : ScriptableObject
{
    [SerializeField] private SerializableDictionary<OrderType, ItemSprites> _orderSpritesDict;

    public SerializableDictionary<OrderType, ItemSprites> OrderSpritesDict => _orderSpritesDict;
}