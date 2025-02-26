using UnityEngine;

[CreateAssetMenu(fileName ="ClientConfig", menuName = "Configs/ClientConfig")]
public class ClientConfig : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private MaterialName _materialName;
    [SerializeField] private Sprite _clientSprite;
    [SerializeField] private OrdersPoolConfig _orders;

    public string Name { get => _name; }
    public MaterialName MaterialName { get => _materialName; }
    public Sprite ClientSprite { get => _clientSprite; }
    public OrdersPoolConfig Orders { get => _orders; }
}