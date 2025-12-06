using Cysharp.Threading.Tasks;
using Sources.Runtime.Gameplay.Character;

namespace Sources.Runtime.Services.Loaders.Resources
{
    public interface IResourceLoader
    {
        UniTask<CharacterRoot> LoadCharacterAsync();
    }
}