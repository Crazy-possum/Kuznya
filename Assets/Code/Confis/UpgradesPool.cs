using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="UpgradesPool", menuName = "Configs/UpgradesPool")]
public class UpgradesPool : ScriptableObject
{
    [SerializeField] private List<UpgradeConfig> _upgradesList;

    public List<UpgradeConfig> UpgradesList => _upgradesList;
}