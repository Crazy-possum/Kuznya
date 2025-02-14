using UnityEngine;
using Zenject;

namespace Orders
{
    public class OrdersInstaller : MonoInstaller
    {
        [SerializeField] private ClientsPoolConfig _clientsPoolConfig;


        public override void InstallBindings()
        {
            Container.Bind<ClientsPoolConfig>().FromInstance(_clientsPoolConfig).AsSingle();

            Container.Bind<ClientsOperator>().AsSingle();
            Container.Bind<OrdersOperator>().AsSingle();

        }
    }
}
