using MAEngine.Extention;
using UnityEngine;

[CreateAssetMenu(fileName = "ClientsSpritesContainer", menuName = "SO/ClientsSpritesContainer", order = 0)]
public class ClientsSpritesContainer : ScriptableObject
{
    [SerializeField] private SerializableDictionary<ClientType, Sprite> _clientSprites;
    
    public SerializableDictionary<ClientType, Sprite> ClientSprites { get => _clientSprites; }
}