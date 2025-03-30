using Orders;
using Progression;
using UnityEngine;

[CreateAssetMenu(fileName = "ProgressionContainer", menuName = "SO/ProgressionContainer", order = 1)]
public class ProgressionContainer : ScriptableObject
{
    [SerializeField] private PlayerMetaData _playerMetaData;
    [SerializeField] private OrdersMetaData _ordersMeta;
    [SerializeField] private UpgradesMetaData _upgradesMeta;

    public PlayerMetaData PlayerMetaData { get => _playerMetaData; set => _playerMetaData = value; }
    public OrdersMetaData OrdersMeta { get => _ordersMeta; set => _ordersMeta = value; }
    public UpgradesMetaData UpgradesMeta { get => _upgradesMeta; set => _upgradesMeta = value; }

    public void SaveData(ProgressionData progressionData)
    {
        _playerMetaData = progressionData.PlayerMetaData;
        _ordersMeta = progressionData.OrdersMeta;
        _upgradesMeta = progressionData.UpgradesMeta;
    }

    public ProgressionData LoadProgress()
    {
        ProgressionData data = new ProgressionData();
        data.LoadData(_playerMetaData, _ordersMeta, _upgradesMeta);
        return data;
    }
}