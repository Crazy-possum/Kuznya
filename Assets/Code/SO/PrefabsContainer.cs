using MAEngine.Extention;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsContainer", menuName = "SO/PrefabsContainer", order = 0)]
public class PrefabsContainer : ScriptableObject
{
    [SerializeField] private SerializableDictionary<PrefabID, GameObject> _prefabsDict;

    public SerializableDictionary<PrefabID, GameObject> PrefabsDict { get => _prefabsDict; }
}