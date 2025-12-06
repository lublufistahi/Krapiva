using Cysharp.Threading.Tasks;
using Sources.Runtime.Services.Builders;
using Sources.Runtime.Services.Loaders.GameData;
using Sources.Runtime.Services.Loaders.Resources;
using UnityEngine;
using VContainer.Unity;

namespace Sources.Runtime.Gameplay.Root
{
    public class GameplayFlow : IStartable
    {
        private readonly IGameDataLoader _gameDataLoader;
        private readonly IResourceLoader _resourceLoader;
        private readonly CharacterBuilder _characterBuilder;
        private readonly Transform _characterSpawnPoint;

        private GameplayFlow(IGameDataLoader gameDataLoader, IResourceLoader resourceLoader, 
            CharacterBuilder characterBuilder, Transform characterSpawnPoint)
        {
            _gameDataLoader = gameDataLoader;
            _resourceLoader = resourceLoader;
            _characterBuilder = characterBuilder;
            _characterSpawnPoint = characterSpawnPoint;
        }
        
        public void Start()
        {
            RunAsync().Forget();
        }

        private async UniTask RunAsync()
        {
            await _gameDataLoader.LoadAsync();
            var characterPrefab = await _resourceLoader.LoadCharacterAsync();

            _characterBuilder.Build(characterPrefab, _characterSpawnPoint.position);
        }
    }
}