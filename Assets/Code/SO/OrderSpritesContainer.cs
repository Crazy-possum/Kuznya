using MAEngine.Extention;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderSpritesContainer", menuName = "SO/OrderSpritesContainer")]
public class OrderSpritesContainer : ScriptableObject
{
    [SerializeField] private SerializableDictionary<OrderType, ItemMaterialsContainer> _orderSpritesDict;

    public SerializableDictionary<OrderType, ItemMaterialsContainer> OrderSpritesDict => _orderSpritesDict;
}