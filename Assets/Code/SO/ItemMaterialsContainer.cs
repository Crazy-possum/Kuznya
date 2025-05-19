using MAEngine.Extention;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemMaterialsContainer", menuName = "SO/ItemMaterialsContainer")]
public class ItemMaterialsContainer : ScriptableObject
{
    [SerializeField] private SerializableDictionary<MaterialName, ItemSprites> _itemSpritesDict;
    
    public SerializableDictionary<MaterialName, ItemSprites> ItemSpritesDict => _itemSpritesDict;
}