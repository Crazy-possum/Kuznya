using UnityEngine;
using Zenject;

namespace MainGUI
{
    public class GUIModuleInstaller : MonoInstaller
    {
        [SerializeField] private GUIView _guiView;
        [SerializeField] private ScalableView _scalableView;

        public override void InstallBindings()
        {
            Container.Bind<GUIView>().
                FromInstance(_guiView).AsSingle();
            Container.Bind<ScalableView>().
                FromInstance(_scalableView).AsSingle();
            Container.Bind<GUINavigationButtonsOperator>().AsSingle();
            Container.Bind<UIScalingAction>().AsSingle();
            Container.Bind<GUIMainOperator>().AsSingle();
        }
    }
}

