using Orders;
using Progression;
using UnityEngine;

[CreateAssetMenu(fileName = "ProgressionContainer", menuName = "SO/ProgressionContainer", order = 1)]
public class ProgressionContainer : ScriptableObject
{
    [SerializeField] private PlayerMetaData _playerMetaData;
    private OrdersMetaData _ordersMeta;

    public PlayerMetaData PlayerMetaData { get => _playerMetaData; set => _playerMetaData = value; }
    public OrdersMetaData OrdersMeta { get => _ordersMeta; set => _ordersMeta = value; }

    public void SaveData(ProgressionData progressionData)
    {
        _playerMetaData = progressionData.PlayerMetaData;
        _ordersMeta = progressionData.OrdersMeta;
    }

    public ProgressionData LoadProgress()
    {
        ProgressionData data = new ProgressionData();
        data.LoadData(_playerMetaData, _ordersMeta);
        return data;
    }
}