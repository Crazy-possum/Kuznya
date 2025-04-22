using UnityEngine;

[CreateAssetMenu(fileName ="ClientConfig", menuName = "Configs/ClientConfig")]
public class ClientConfig : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private MaterialName _materialName;
    [SerializeField] private Sprite _clientSprite;
    [SerializeField] private OrdersPoolConfig _orders;
    [SerializeField] private ClientType _clientType;
    private string _clientID;

    public string Name { get => _name; }
    public MaterialName MaterialName { get => _materialName; }
    public Sprite ClientSprite { get => _clientSprite; }
    public OrdersPoolConfig Orders { get => _orders; }
    public ClientType ClientType { get => _clientType; }
    public string ClientID { get => _clientID; set => _clientID = value; }

    public override string ToString()
    {
        return $"{this.name}";
    }
}