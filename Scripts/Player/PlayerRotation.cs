using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] Transform mainCamera;
        [SerializeField] Animator playerAnimator;
        [SerializeField] Rigidbody playerRb;

        private float _xRotation = 0f;
        private float _mouseSensitivityX = 500f;
        private float _mouseSensitivityY = 300f;

        private const float Y_ROTATION_RANGE = 80f;

        private void Update()
        {
            HandleHeadRotation();
            HandleBodyRotation();
        }

        private void HandleHeadRotation()
        {
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivityY * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -Y_ROTATION_RANGE, Y_ROTATION_RANGE);

            mainCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }

        private void HandleBodyRotation()
        {
            float mouseInputX = Input.GetAxisRaw("Mouse X");
            float mouseX = mouseInputX * _mouseSensitivityX * Time.deltaTime;

            transform.Rotate(mouseX * Vector3.up);
        }
    }
}
