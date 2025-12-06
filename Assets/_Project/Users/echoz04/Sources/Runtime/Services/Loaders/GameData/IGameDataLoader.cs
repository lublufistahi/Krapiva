using Cysharp.Threading.Tasks;

namespace Sources.Runtime.Services.Loaders.GameData
{
    public interface IGameDataLoader
    {
        Game.GameData Get();
        
        UniTask<Game.GameData> LoadAsync();
    }
}