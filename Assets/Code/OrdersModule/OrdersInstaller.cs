using UnityEngine;
using Zenject;

namespace Orders
{
    public class OrdersInstaller : MonoInstaller
    {
        [SerializeField] private ClientsPoolConfig _clientsPoolConfig;
        [SerializeField] private DialogueView _dialogueView;


        public override void InstallBindings()
        {
            Container.Bind<ClientsPoolConfig>().FromInstance(_clientsPoolConfig).AsSingle();

            Container.Bind<DialogueView>().FromInstance(_dialogueView).AsSingle();

            Container.Bind<ClientsOperator>().AsSingle();
            Container.Bind<OrdersOperator>().AsSingle();
            Container.Bind<DialogueActions>().AsSingle();

        }
    }
}
