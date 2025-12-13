using UnityEngine;

namespace Sources.Runtime.Gameplay.Character.Movement
{
    public sealed class GravityHandler
    {
        private readonly CharacterData _data;

        public GravityHandler(CharacterData data)
        {
            _data = data;
        }

        public void ApplyGravity(ref Vector3 velocity)
        {
            float gravityMultiplier = velocity.y < 0 ? _data.FallSpeedMultiplier : _data.LowJumpSpeedMultiplier;

            velocity.y += Physics.gravity.y / 2 * gravityMultiplier * Time.deltaTime;
        }
    }
}