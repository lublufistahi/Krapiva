using UnityEngine;

namespace Sources.Runtime.Gameplay.Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterView : MonoBehaviour
    {
        private readonly int WalkingHash = Animator.StringToHash("isWalking");
        private readonly int RunningHash = Animator.StringToHash("isRunning");
        
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
                    SetAnimatorMoveParameters(false, false);
                    break;
                case MoveState.Walk:
                    SetAnimatorMoveParameters(true, false);
                    break;
                case MoveState.Run:
                    SetAnimatorMoveParameters(false, true);
                    break;
            }
        }

        private void SetAnimatorMoveParameters(bool walkingState, bool runningState)
        {
            _animator.SetBool(WalkingHash, walkingState);
            _animator.SetBool(RunningHash, runningState);
        }
    }
}