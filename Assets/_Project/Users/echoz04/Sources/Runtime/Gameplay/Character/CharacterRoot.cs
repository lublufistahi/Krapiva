using Cysharp.Threading.Tasks;
using Sources.Runtime.Gameplay.Character.Combat;
using Sources.Runtime.Services.Loaders.GameData;
using UnityEngine;
using Sources.Runtime.Gameplay.Character.Movement;

namespace Sources.Runtime.Gameplay.Character
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class CharacterRoot : MonoBehaviour
    {
        [SerializeField] private CharacterController _controller;
        [SerializeField] private CharacterView _view;
        [SerializeField] private Transform _feetPoint;
        [SerializeField] private Transform _cameraHolder;
        
        private CharacterInput _input;
        private CharacterData _data;

        private CharacterMover _mover;
        private CameraRotator _cameraRotator;
        private GravityHandler _gravityHandler;
        private CharacterAttacker _attacker;

        private void OnValidate()
        {
            _controller ??= GetComponent<CharacterController>();
            _view ??= GetComponentInChildren<CharacterView>();
        }

        public void SetData(CharacterData data) => 
            _data = data;

        public void SetInput(CharacterInput input) => 
            _input = input;

        public void InitializeSystems()
        {
            _input.Enable();
            
            _gravityHandler = new GravityHandler(_data);
            _mover = new CharacterMover(_gravityHandler, _controller, _data, _input, _feetPoint);
            _cameraRotator = new CameraRotator(_cameraHolder, transform, _input, _data);
            _attacker = new CharacterAttacker(_input);

            _view.Initialize(_mover);
            _attacker.Initialize();
        }

        private void Update()
        {
            _mover.GatherInput();
            _cameraRotator.Tick();
            Vector3 slopeDirection = Vector3.back;
            
            var result = _mover.TryGetGroundHit(out slopeDirection);
            
            Debug.Log($"result: {result} and direction {slopeDirection}");
            Debug.DrawLine(transform.position, transform.position + slopeDirection, Color.red);
        }

        private void FixedUpdate()
        {
            _mover.CheckGround();
            _mover.HandleJump().Forget();
            _mover.ApplyGravity();
            _mover.HandleMove();
        }
        
        private void OnDrawGizmosSelected()
        {
            if (_data == null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_feetPoint.position, _data.GroundCheckRadius);
            
            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, -transform.up * _data.GroundCheckRadius);
        }

        private void OnDestroy()
        {
            _input.Disable();
            _mover.Dispose();
        }
    }
}
