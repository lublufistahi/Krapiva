using System;
using Sources.Runtime.Services.Loaders.GameData;
using UnityEngine;
using VContainer;

namespace Sources.Runtime.Gameplay.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class CharacterRoot : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _feetPoint;
        [SerializeField] private Transform _cameraHolder;
        
        private CharacterInput _input;
        private IGameDataLoader _gameDataLoader;
        
        private CharacterData _data;
        private CharacterMover _mover;
        private CameraRotator _cameraRotator;
        private GravityHandler _gravityHandler;

        private void OnValidate()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        public void SetData(CharacterData data) =>
            _data = data;

        public void SetInput(CharacterInput input) =>
            _input = input;

        public void InitializeSystems()
        {
            _input.Enable();
            
            _mover = new CharacterMover(_rigidbody, _data, _input, _feetPoint);
            _cameraRotator = new CameraRotator(_cameraHolder, transform, _input, _data);
            _gravityHandler = new GravityHandler(_rigidbody, _data);
        }

        private void Update()
        {
            _mover.CheckGround();
            _mover.HandleJump();
            _cameraRotator.Tick();
            _gravityHandler.Tick();
        }

        private void FixedUpdate()
        {
            _mover.HandleMove();
        }

        private void OnDestroy()
        {
            _input.Disable();
        }
    }
}