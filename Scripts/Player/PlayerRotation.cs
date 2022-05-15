using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] private Transform mainCamera;
        [SerializeField] private Animator playerAnimator;

        private float _xRotation = 0f;
        private float _mouseSensitivity = 600f;

        private const float Y_ROTATION_RANGE = 80f;

        private void Update()
        {
            HandleBodyRotation();
            HandleHeadRotation();
        }

        private void HandleHeadRotation()
        {
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -Y_ROTATION_RANGE, Y_ROTATION_RANGE);

            mainCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }

        private void HandleBodyRotation()
        {
            float mouseInputX = Input.GetAxisRaw("Mouse X");

            float mouseX = mouseInputX * _mouseSensitivity * Time.deltaTime;

            playerAnimator.SetFloat("Turn", Mathf.Clamp(mouseInputX, -1, 1));

            transform.Rotate(Vector3.up * mouseX);
        }
    }
}
