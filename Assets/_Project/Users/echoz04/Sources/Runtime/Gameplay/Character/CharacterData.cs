using UnityEngine;

namespace Sources.Runtime.Gameplay.Character
{
    [CreateAssetMenu(menuName = "Data/Character", fileName = "Character Data")]
    public class CharacterData : ScriptableObject
    {
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float RunSpeed { get; private set; }
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float FallSpeedMultiplier { get; private set; } = 1.5f;
        [field: SerializeField] public float LowJumpSpeedMultiplier { get; private set; } = 2f;
        [field: SerializeField] public float GroundCheckRadius { get; private set; }
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }
        [field: Space]
        
        [field: SerializeField] public float Sensitivity { get; private set; }

        [field: SerializeField] public float MaxCameraAngle { get; private set; } = 80f;
        [field: SerializeField] public float MinCameraAngle { get; private set; } = -80f;
    }
}