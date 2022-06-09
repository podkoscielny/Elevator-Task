using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ElevatorTask
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] Transform mainCamera;
        [SerializeField] Animator playerAnimator;
        [SerializeField] Rigidbody playerRb;

        private Vector2 _rotationInput;

        private float _xRotation = 0f;
        private float _mouseSensitivityY = 100f;
        private float _mouseSensitivityX = 0.1f;

        private const float Y_ROTATION_RANGE = 80f;

        private void Update()
        {
            HandleHeadRotation();
            HandleBodyRotation();
        }

        public void GetRotationInput(InputAction.CallbackContext context) => _rotationInput = context.ReadValue<Vector2>();

        private void HandleHeadRotation()
        {
            float mouseY = _rotationInput.y * _mouseSensitivityY * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -Y_ROTATION_RANGE, Y_ROTATION_RANGE);

            mainCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }

        private void HandleBodyRotation() => playerRb.rotation = Quaternion.Euler(playerRb.rotation.eulerAngles + GetTargetRotation());

        private Vector3 GetTargetRotation() => new Vector3(0f, 1f * _rotationInput.x * _mouseSensitivityX, 0f);
    }
}
