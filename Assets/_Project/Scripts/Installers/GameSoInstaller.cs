using _Project.Scripts.Data;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    [CreateAssetMenu(menuName = "Installers/Game/Game So Installer")]
    public class GameSoInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private ParticleLibrary particleLibrary;
        
        public override void InstallBindings()
        {
            Container.Bind<GameSettings>().FromInstance(gameSettings);
            Container.Bind<ParticleLibrary>().FromInstance(particleLibrary);
        }
    }
}