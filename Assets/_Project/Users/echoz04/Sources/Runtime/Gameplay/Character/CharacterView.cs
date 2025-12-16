using Cysharp.Threading.Tasks;
using UnityEngine;
using Sources.Runtime.Gameplay.Character.Movement;

namespace Sources.Runtime.Gameplay.Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterView : MonoBehaviour
    {
        private readonly int IdleHash = Animator.StringToHash("Idle");
        private readonly int WalkHash = Animator.StringToHash("Walk");
        private readonly int RunHash = Animator.StringToHash("Run");
        private readonly int JumpHash = Animator.StringToHash("Jumping Up");
        
        private const float _crossFadeDuration = 0.15f;
        
        [SerializeField] private Animator _animator;
        
        private CharacterMover _mover;
        private bool _isJumping = false;
        private bool _isLanded = false;

        private void OnValidate()
        {
            _animator ??= GetComponent<Animator>();
        }
        
        public void Initialize(CharacterMover mover)
        {
            _mover = mover;
            
            _mover.OnStateChanged += OnMoveStateChanged;
            _mover.OnJumped += OnJumped;
            _mover.OnLanded += OnLanded;
        }

        public void OnJumpAnimationFinished()
        {
            _isJumping = false;
        }
        
        private void OnDestroy()
        {
            _mover.OnStateChanged -= OnMoveStateChanged;
            _mover.OnJumped -= OnJumped;
            _mover.OnLanded -= OnLanded;
        }

        private void OnMoveStateChanged(MoveState state)
        {
            if (_isLanded == false)
                return;
            
            switch (state)
            {
                case MoveState.Idle:
                    _animator.CrossFade(IdleHash, _crossFadeDuration/2f);
                    break;
                case MoveState.Walk:
                    _animator.CrossFade(WalkHash, _crossFadeDuration);
                    break;
                case MoveState.Run:
                    _animator.CrossFade(RunHash, _crossFadeDuration);
                    break;
            }
        }

        private void OnJumped()
        {
            _isJumping = true;
            _isLanded = false;
            _animator.CrossFade(JumpHash, 0.05f);
        }

        private void OnLanded()
        {
            if (_isJumping == false)
            {
                _isLanded = true;
                
                OnMoveStateChanged(_mover.CurrentState);
            }
        }
    }
}