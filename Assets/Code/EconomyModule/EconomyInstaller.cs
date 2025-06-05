using Progression;
using UnityEngine;
using Zenject;

namespace Economy
{
    public class EconomyInstaller : MonoInstaller
    {
        [SerializeField] private ShopView _shopView;
        [SerializeField] private TradeView _tradeView;
        [SerializeField] private MaterialsUIView _materialsUIView;
        [SerializeField] private KingPanelView _kingPanelView;
        [SerializeField] private UpgradesPool _upgradesPool;
        
        [SerializeField] private StorageMaterialsConfig _storageMaterialsConfig;
        

        public override void InstallBindings()
        {
            Container.Bind<ShopView>().FromInstance(_shopView).AsSingle();
            Container.Bind<TradeView>().FromInstance(_tradeView).AsSingle();
            Container.Bind<MaterialsUIView>().FromInstance(_materialsUIView).AsSingle();
            Container.Bind<KingPanelView>().FromInstance(_kingPanelView).AsSingle();
            Container.Bind<UpgradesPool>().FromInstance(_upgradesPool).AsSingle();

            Container.Bind<StorageMaterialsConfig>().FromInstance(_storageMaterialsConfig).AsSingle();
            
            Container.Bind<UpgradesUIActions>().AsSingle();
            Container.Bind<EconomyOperator>().AsSingle();
            Container.Bind<TradeController>().AsSingle();
            Container.Bind<MaterialsOperator>().AsSingle();
            Container.Bind<UpgradeOperator>().AsSingle();
            Container.Bind<SecretUpgradeController>().AsSingle();
        }
    }
}