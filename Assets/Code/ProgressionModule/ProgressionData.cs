namespace Progression
{
    public class ProgressionData
    {
        private PlayerMetaData _playerMetaData;
        private OrdersMetaData _ordersMeta;
        private UpgradesMetaData _upgradesMeta;


        public PlayerMetaData PlayerMetaData { get => _playerMetaData; set => _playerMetaData = value; }
        public OrdersMetaData OrdersMeta { get => _ordersMeta; set => _ordersMeta = value; }
        public UpgradesMetaData UpgradesMeta { get => _upgradesMeta; set => _upgradesMeta = value; }

        public void LoadData(PlayerMetaData playerMetaData, OrdersMetaData ordersMeta, UpgradesMetaData upgradesMeta)
        {
            _playerMetaData = playerMetaData;
            _ordersMeta = ordersMeta;
            _upgradesMeta = upgradesMeta;
        }

        public void ClearData()
        {
            _playerMetaData.Clear();
            _ordersMeta.Clear();
            _upgradesMeta.Clear();
        }

    }
}
