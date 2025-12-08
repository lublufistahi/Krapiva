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
        
        private const float _crossFadeDuration = 0.15f;
        
        [SerializeField] private Animator _animator;
        
        private CharacterMover _mover;

        private void OnValidate()
        {
            _animator ??= GetComponent<Animator>();
        }
        
        public void Initialize(CharacterMover mover)
        {
            _mover = mover;
            
            _mover.OnStateChanged += OnMoveStateChanged;
        }

        private void OnDestroy()
        {
            _mover.OnStateChanged -= OnMoveStateChanged;
        }

        private void OnMoveStateChanged(MoveState state)
        {
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
    }
}