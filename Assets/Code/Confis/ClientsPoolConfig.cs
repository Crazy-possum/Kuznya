using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClientsPoolConfig", menuName = "Configs/ClientsPoolConfig")]
public class ClientsPoolConfig : ScriptableObject
{
    [SerializeField] private List<ClientConfig> clients;

    public List<ClientConfig> Clients { get => clients; }
}
