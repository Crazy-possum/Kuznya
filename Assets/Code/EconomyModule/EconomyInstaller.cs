using UnityEngine;
using Zenject;

namespace Economy
{
    public class EconomyInstaller : MonoInstaller
    {
        [SerializeField] private ShopView _shopView;
        [SerializeField] private TradeView _tradeView;
        [SerializeField] private UpgradesPool _upgradesPool;
        

        public override void InstallBindings()
        {
            Container.Bind<ShopView>().FromInstance(_shopView).AsSingle();
            Container.Bind<TradeView>().FromInstance(_tradeView).AsSingle();
            Container.Bind<UpgradesPool>().FromInstance(_upgradesPool).AsSingle();
            Container.Bind<UpgradesUIActions>().AsSingle();
            Container.Bind<EconomyOperator>().AsSingle();
            Container.Bind<TradeController>().AsSingle();
        }
    }
}