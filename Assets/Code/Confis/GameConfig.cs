using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private int _maxClients;
    [SerializeField] private int _minTimeBetweenClients;
    [SerializeField] private int _maxTimeBetweenClients;

    public int MaxClients { get => _maxClients; }
    public int MinTimeBetweenClients { get => _minTimeBetweenClients; }
    public int MaxTimeBetweenClients { get => _maxTimeBetweenClients; }
}
