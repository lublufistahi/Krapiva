using UnityEngine;

namespace Sources.Runtime.Gameplay.Character
{
    public sealed class CameraRotator
    {
        private readonly Transform _cameraHolder;
        private readonly Transform _character;
        private readonly CharacterInput _input;
        private readonly CharacterData _data;

        private float _xRotation = 0;
        
        public CameraRotator(Transform cameraHolder, Transform character, CharacterInput input, CharacterData data)
        {
            _cameraHolder = cameraHolder;
            _character = character;
            _input = input;
            _data = data;
        }

        public void Tick()
        {
            Vector2 lookPosition = _input.Movement.Look.ReadValue<Vector2>();
            
            float mouseX = lookPosition.x * _data.Sensitivity * Time.deltaTime;
            float mouseY = lookPosition.y * _data.Sensitivity * Time.deltaTime;
            
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, _data.MinCameraAngle, _data.MaxCameraAngle);
            _cameraHolder.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            
            _character.Rotate(Vector3.up * mouseX);
        }
    }
}