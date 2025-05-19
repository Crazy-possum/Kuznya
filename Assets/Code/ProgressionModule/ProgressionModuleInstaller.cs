using UnityEngine;
using Zenject;

namespace Progression
{
    public class ProgressionModuleInstaller : MonoInstaller
    {
        [SerializeField] private ProgressionContainer _progressionContainer;
        [SerializeField] private TutorialView _tutorialView;

        public override void InstallBindings()
        {
            Container.Bind<ProgressionContainer>().
                FromInstance(_progressionContainer).AsSingle();
            
            Container.Bind<TutorialView>().FromInstance(_tutorialView).AsSingle();
            
            Container.Bind<ProgressionData>().AsSingle();
            Container.Bind<ProgressionOperator>().AsSingle();
            Container.Bind<StagesCheckActions>().AsSingle();
            Container.Bind<TutorialActions>().AsSingle();
        }
    }
}
