using Cysharp.Threading.Tasks;
using Sources.Runtime.Gameplay.Character;

namespace Sources.Runtime.Services.Loaders.Resources
{
    public class ResourceLoader : IResourceLoader
    {
        private const string CharacterRootPath = "Character Root";
        private CharacterRoot _characterRoot;
        
        public async UniTask<CharacterRoot> LoadCharacterAsync()
        {
            _characterRoot ??= (CharacterRoot)await UnityEngine.Resources.LoadAsync<CharacterRoot>(CharacterRootPath);
            
            return _characterRoot;
        }
    }
}