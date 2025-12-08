using UnityEngine;

namespace Sources.Runtime.Gameplay.Character.Movement
{
    public sealed class GravityHandler
    {
        private readonly Rigidbody _rigidbody;
        private readonly CharacterData _data;
        
        public GravityHandler(Rigidbody rigidbody, CharacterData data)
        {
            _rigidbody = rigidbody;
            _data = data;
        }

        public void Tick()
        {
            var velocity = _rigidbody.linearVelocity;
            
            if (velocity.y < 0)
                velocity += Vector3.up * velocity.y * (_data.FallSpeedMultiplier - 1) * Time.deltaTime;
            else if (velocity.y > 0)
                velocity += Vector3.up * velocity.y * (_data.LowJumpSpeedMultiplier - 1) * Time.deltaTime;

            _rigidbody.linearVelocity = velocity;
        }
    }
}