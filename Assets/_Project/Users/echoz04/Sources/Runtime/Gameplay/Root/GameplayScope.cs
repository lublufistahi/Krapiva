using Sources.Runtime.Services.Builders;
using Sources.Runtime.Services.Loaders.GameData;
using Sources.Runtime.Services.Loaders.Resources;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Sources.Runtime.Gameplay.Root
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private Transform _characterSpawnPoint;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterLoaders(builder);
            RegisterBuilders(builder);
            RegisterInput(builder);
            RegisterFlow(builder);
        }

        private void RegisterLoaders(IContainerBuilder builder)
        {
            builder.Register<GameDataLoader>(Lifetime.Singleton)
                .As<IGameDataLoader>();
            
            builder.Register<ResourceLoader>(Lifetime.Singleton)
                .As<IResourceLoader>();
        }

        private void RegisterBuilders(IContainerBuilder builder)
        {
            builder.RegisterInstance(_characterSpawnPoint);
            
            builder.Register<CharacterBuilder>(Lifetime.Singleton)
                .AsSelf();
        }
        
        private void RegisterInput(IContainerBuilder builder)
        {
            builder.Register<CharacterInput>(Lifetime.Singleton)
                .AsSelf();
        }

        private void RegisterFlow(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayFlow>();
        }
    }
}
