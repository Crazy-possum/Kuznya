using UnityEngine;

[CreateAssetMenu(fileName = "MaterialConfig", menuName = "Configs/MaterialConfig")]
public class MaterialConfig : ScriptableObject
{
    [SerializeField] private MaterialName _materialName;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;

    public MaterialName MaterialName { get => _materialName; }
    public string Name { get => _name; }
    public Sprite Sprite { get => _sprite; }
}