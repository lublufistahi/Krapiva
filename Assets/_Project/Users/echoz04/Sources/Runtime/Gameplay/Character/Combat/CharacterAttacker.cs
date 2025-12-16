using System;
using UnityEngine;

namespace Sources.Runtime.Gameplay.Character.Combat
{
    public sealed class CharacterAttacker : IDisposable
    {
        private readonly CharacterInput _input;
        private readonly CharacterAttackTriggers _triggers;
        
        private Action<UnityEngine.InputSystem.InputAction.CallbackContext> _lightAttackHandler;
        private Action<UnityEngine.InputSystem.InputAction.CallbackContext> _heavyAttackHandler;

        private bool _isAttacking = false;
        
        public CharacterAttacker(CharacterInput input, CharacterAttackTriggers triggers)
        {
            _input = input;
            _triggers = triggers;
        }

        public void Initialize()
        {
            _lightAttackHandler = ctx => LightAttackPressed();
            _heavyAttackHandler = ctx => HeavyAttackPressed();
            
            _input.Combat.LightAttack.performed += _lightAttackHandler;
            _input.Combat.HeavyAttack.performed += _heavyAttackHandler;
        }
        
        public void Dispose()
        {
            if (_lightAttackHandler != null)
                _input.Combat.LightAttack.performed -= _lightAttackHandler;

            if (_heavyAttackHandler != null)
                _input.Combat.HeavyAttack.performed -= _heavyAttackHandler;
        }

        private void OnAttackPressed()
        {
            if(_isAttacking == true)
                return;
            
            _isAttacking = true;
        }
        
        private void LightAttackPressed()
        {
            OnAttackPressed();
            
            Debug.Log("LightAttackPressed");
        }
        
        private void HeavyAttackPressed()
        {
            OnAttackPressed();
            
            Debug.Log("HeavyAttackPressed");
        }
    }
}