using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] private Transform mainCamera;

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
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;

            transform.Rotate(Vector3.up * mouseX);
        }
    }
}
