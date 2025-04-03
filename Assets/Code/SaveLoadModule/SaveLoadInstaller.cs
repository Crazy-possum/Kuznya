using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public class SaveLoadInstaller : MonoInstaller
    {
        [SerializeField] private SaveDataKeysContainer _saveDataKeysContainer;
        
        public override void InstallBindings()
        {
            Container.Bind<SaveDataKeysContainer>().FromInstance(_saveDataKeysContainer).AsSingle();
            
            Container.Bind<SaveLoadActions>().AsSingle();
            Container.Bind<SavableDataContainer>().AsSingle();
        }
    }
}