using Sources.Runtime.Gameplay.Character;
using Sources.Runtime.Services.Loaders.GameData;
using UnityEngine;

namespace Sources.Runtime.Services.Builders
{
    public class CharacterBuilder : IBuilder<CharacterRoot>
    {
        private readonly IGameDataLoader _gameDataLoader;
        private readonly CharacterInput _input;

        private CharacterBuilder(IGameDataLoader gameDataLoader, CharacterInput input)
        {
            _gameDataLoader = gameDataLoader;
            _input = input;
        }
        
        public CharacterRoot Build(CharacterRoot prefab, Vector3 spawnPosition, Transform parent = null)
        {
            CharacterRoot instance = Object.Instantiate(prefab, spawnPosition, Quaternion.identity, parent);

            instance
                .ProvideData(_gameDataLoader.Get().CharacterData)
                .ProvideInput(_input)
                .Initialize();

            return instance;
        }
    }
}