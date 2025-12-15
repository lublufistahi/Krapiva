using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Character.Movement
{
    public sealed class CharacterMover : IDisposable
    {
        public event Action<MoveState> OnStateChanged;
        public event Action OnJumped;
        public event Action OnLanded;

        public MoveState CurrentState { get; private set; } = MoveState.Idle;
        public bool IsGrounded { get; private set; }

        private readonly GravityHandler _gravityHandler;
        private readonly CharacterController _controller;
        private readonly CharacterData _data;
        private readonly CharacterInput _input;
        private readonly Transform _feetPoint;

        private Vector3 _moveDirection;
        private Vector3 _velocity;
        private Vector3 _slopeSlideVelocity;

        private float _moveSpeed;
        private bool _jumpRequested;
        private bool _isJumping = false;
        private bool _wasGrounded;

        public CharacterMover(GravityHandler gravityHandler, CharacterController controller, CharacterData data,
            CharacterInput input, Transform feetPoint)
        {
            _gravityHandler = gravityHandler;
            _controller = controller;
            _data = data;
            _input = input;
            _feetPoint = feetPoint;

            _input.Movement.Jump.performed += _ => _jumpRequested = IsGrounded;
        }

        public void GatherInput()
        {
            Vector2 inputVector = _input.Movement.Move.ReadValue<Vector2>();
            _moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
            _moveDirection = _controller.transform.TransformDirection(_moveDirection);

            MoveState newState = _moveDirection == Vector3.zero ? MoveState.Idle : IsRunning() ? MoveState.Run : MoveState.Walk;

            if (newState != CurrentState)
            {
                CurrentState = newState;
                OnStateChanged?.Invoke(newState);
            }

            _moveSpeed = CurrentState == MoveState.Run && IsGrounded == true ? _data.RunSpeed : _data.MoveSpeed;
        }

        public async UniTask HandleJump()
        {
            if (IsGrounded == false || _jumpRequested == false)
                return;

            _isJumping = true;
            
            OnJumped?.Invoke();
            _jumpRequested = false;
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.4f));

            _isJumping = false;
            
            _velocity.y = Mathf.Sqrt(_data.JumpForce * -2f * Physics.gravity.y);
        }

        public void ApplyGravity()
        {
            _gravityHandler.ApplyGravity(ref _velocity);
        }

        public void HandleMove()
        {
            if(_isJumping == true)
                return;
            
            Vector3 horizontal = _moveDirection * _moveSpeed;
            Vector3 final = new Vector3(horizontal.x, _velocity.y, horizontal.z);

            _controller.Move(final * Time.deltaTime);
            
            _velocity.x = _controller.velocity.x;
            _velocity.z = _controller.velocity.z;
        }

        public void CheckGround()
        {
            bool isGrounded = Physics.CheckSphere(_feetPoint.position, _data.GroundCheckRadius);
            
            if(_wasGrounded == false && isGrounded == true) 
                OnLanded?.Invoke();
            
            IsGrounded = isGrounded;
            _wasGrounded = isGrounded;
        }

        public void Dispose()
        {
            _input.Movement.Jump.performed -= _ => _jumpRequested = true;
        }

        private bool IsRunning() => _input.Movement.Shift.IsPressed();
    }
}
