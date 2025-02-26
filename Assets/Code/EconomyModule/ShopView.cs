using System.Collections;
using System.Collections.Generic;
using MAEngine.Extention;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<UpgradeName, UpgradeView> _upgradeViews;

    public SerializableDictionary<UpgradeName, UpgradeView> UpgradeViews => _upgradeViews;
}
