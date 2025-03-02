namespace Progression
{
    public class ProgressionData
    {
        private PlayerMetaData _playerMetaData;
        private OrdersMetaData _ordersMeta;


        public PlayerMetaData PlayerMetaData { get => _playerMetaData; set => _playerMetaData = value; }
        public OrdersMetaData OrdersMeta { get => _ordersMeta; set => _ordersMeta = value; }

        public void LoadData(PlayerMetaData playerMetaData, OrdersMetaData ordersMeta)
        {
            _playerMetaData = playerMetaData;
            _ordersMeta = ordersMeta;
        }

    }
}
