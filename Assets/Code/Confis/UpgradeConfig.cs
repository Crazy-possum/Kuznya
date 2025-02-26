using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Upgrade", menuName = "Configs/UpgradeConfig")]
public class UpgradeConfig : ScriptableObject
{
    [SerializeField] private UpgradeName _name;
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _upgradeSprite;
    [SerializeField] private List<int> _levelsCostList;

    public UpgradeName Name => _name;
    public string Title => _title;
    public string Description => _description;
    public Sprite UpgradeSprite => _upgradeSprite;
    public List<int> LevelsCostList => _levelsCostList;
}