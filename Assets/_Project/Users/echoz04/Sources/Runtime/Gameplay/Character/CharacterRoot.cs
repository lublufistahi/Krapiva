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
        
        private CharacterInput _input;
        private IGameDataLoader _gameDataLoader;
        
        private CharacterData _data;
        private CharacterMover _mover;

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
        }

        private void Update()
        {
            _mover.CheckGround();
            _mover.HandleJump();
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