using Sources.Runtime.Gameplay.Character;
using UnityEngine;

namespace Sources.Runtime.Game
{
    [CreateAssetMenu(menuName = "Data/Game", fileName = "Game Data")]
    public sealed class GameData : ScriptableObject
    {
        [field: SerializeField] public CharacterData CharacterData { get; private set; }
    }
}