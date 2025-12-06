using UnityEngine;

namespace Sources.Runtime.Gameplay.Character
{
    [CreateAssetMenu(menuName = "Data/Character", fileName = "Character Data")]
    public class CharacterData : ScriptableObject
    {
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float GroundCheckDistance { get; private set; }
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    }
}