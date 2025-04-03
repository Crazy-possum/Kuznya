using SO;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class AudioInstaller : MonoInstaller
    {
        [SerializeField] private AudioSourceView _audioSourceView;
        [SerializeField] private AudioContainer _audioContainer;
        
        public override void InstallBindings()
        {
            Container.Bind<AudioSourceView>().FromInstance(_audioSourceView).AsSingle();
            Container.Bind<AudioContainer>().FromInstance(_audioContainer).AsSingle();
            
            Container.Bind<AudioOperator>().AsSingle();
        }
    }
}