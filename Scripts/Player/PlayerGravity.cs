using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class PlayerGravity : MonoBehaviour
    {
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private CharacterController characterController;

        private Vector3 _playerVelocity;
        private bool _isGrounded;
        private float _gravityMultiplier = 2f;

        private const float GRAVITY = -9.81f;
        private const float GROUND_CHECK_RADIUS = 0.4f;

        private void Update()
        {
            CheckBeingGrounded();
            AddGravityToPlayer();
        }

        private void AddGravityToPlayer()
        {
            _playerVelocity.y += GRAVITY * _gravityMultiplier * Time.deltaTime;
            characterController.Move(_playerVelocity * Time.deltaTime);
        }

        private void CheckBeingGrounded()
        {
            _isGrounded = Physics.CheckSphere(groundCheck.position, GROUND_CHECK_RADIUS, groundLayer);

            if (_isGrounded && _playerVelocity.y < 0) _playerVelocity.y = 0f;
        }
    }
}
