using UnityEngine;
using Zenject;

public class GameSceneMonoInstaller : MonoInstaller
{
    [SerializeField] private GameObject inputHandlerObject;
    [SerializeField] private GameObject objectPoolObject;
    [SerializeField] private GameObject dataHolderObject;
    
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<OnLevelCompletedSignal>().OptionalSubscriber();
        Container.DeclareSignal<OnLevelEndSignal>().OptionalSubscriber();
        Container.DeclareSignal<OnLevelStartSignal>().OptionalSubscriber();
        
        Container.Bind<ISaveSystem>().To<JsonSaveSystem>().AsSingle();
        Container.Bind<IInputHandler>().To<MobileInputHandler>().FromComponentInNewPrefab(inputHandlerObject).AsSingle();
        Container.BindInterfacesAndSelfTo<DataManager>().AsSingle();
        Container.Bind<ObjectPool>().FromComponentInNewPrefab(objectPoolObject).AsSingle();
        Container.BindInterfacesAndSelfTo<LineManager>().AsSingle();
        Container.Bind<DataHolder>().FromComponentInNewPrefab(dataHolderObject).AsSingle();
        Container.BindInterfacesAndSelfTo<MainStateMachine>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIStateMachine>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<StickManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.Bind<MatchController>().AsSingle();
    }
}
