using System.Linq.Expressions;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Character
{
    public sealed class CharacterMover
    {
        private readonly Rigidbody _rigidbody;
        private readonly CharacterData _data;
        private readonly CharacterInput _input;
        private readonly Transform _feetPoint;
        
        private bool _isGrounded;
        
        public CharacterMover(Rigidbody rigidbody, CharacterData data, CharacterInput input, Transform feetPoint)
        {
            _rigidbody = rigidbody;
            _data = data;
            _input = input;
            _feetPoint = feetPoint;
        }

        public void HandleMove()
        {
            Vector2 moveInput = GetMoveInput();
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
            
            moveDirection = _rigidbody.transform.TransformDirection(moveDirection);

            Vector3 current = _rigidbody.linearVelocity;
            
            var currentMoveSpeed = IsShifting() ? _data.RunSpeed : _data.MoveSpeed;

            Vector3 velocity = new Vector3(moveDirection.x * currentMoveSpeed, current.y, 
                moveDirection.z * currentMoveSpeed);

            _rigidbody.linearVelocity = velocity;
        }
        
        public void HandleJump()
        {
            if (_isGrounded == false)
                return;
            
            if(IsJumped() == false)
                return;

            Vector3 velocity = _rigidbody.linearVelocity;
            velocity.y = 0f; 

            _rigidbody.linearVelocity = velocity;
            _rigidbody.AddForce(Vector3.up * _data.JumpForce, ForceMode.Impulse);
        }
        
        public void CheckGround()
        {
            Vector3 position = _feetPoint.position;

            _isGrounded = Physics.CheckSphere(position, _data.GroundCheckRadius);
            
            Color gizmoColor = _isGrounded ? Color.green : Color.red;
            Debug.DrawRay(position, Vector3.down * 0.1f, gizmoColor);
        }

        private Vector2 GetMoveInput() =>
            _input.Movement.Move.ReadValue<Vector2>();
        
        private bool IsJumped() =>
            _input.Movement.Jump.WasPressedThisFrame();
        
        private bool IsShifting() =>
            _input.Movement.Shift.IsPressed();
    }
}