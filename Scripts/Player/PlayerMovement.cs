using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ElevatorTask
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] Animator playerAnimator;
        [SerializeField] Rigidbody playerRb;

        private bool _isWalkingFast = false;

        private float _timestepMultiplier;
        private Vector2 _movementInput;

        private const float INITIAL_TIMESTEP = 0.02f;
        private const float MOVEMENT_SPEED = 120f;
        private const float SPRINT_MULTIPLIER = 2f;

        private void Start() => SetTimestepMultiplier();

        private void Update() =>  SetPlayerMovementAnimation();

        private void FixedUpdate() => MovePlayer();

        public void GetMovementInput(InputAction.CallbackContext context) => _movementInput = context.ReadValue<Vector2>();

        public void Sprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _isWalkingFast = true;
            }
            else if (context.canceled)
            {
                _isWalkingFast = false;
            }
        }

        private void MovePlayer()
        {
            ResetMovementVelocity();

            Vector3 direction = transform.right * _movementInput.x + transform.forward * _movementInput.y;
            if (_isWalkingFast) direction *= SPRINT_MULTIPLIER;

            playerRb.AddForce(MOVEMENT_SPEED * _timestepMultiplier * direction);
        }

        private void SetPlayerMovementAnimation()
        {
            Vector2 movement = new Vector2(_movementInput.x, _movementInput.y).normalized;

            playerAnimator.SetFloat("Movement", movement.magnitude);
            playerAnimator.SetFloat("MovementX", _movementInput.x);
            playerAnimator.SetFloat("MovementY", _movementInput.y);

            playerAnimator.speed = _isWalkingFast ? 1.2f : 1f;
        }

        private void ResetMovementVelocity()
        {
            Vector3 playerVelocity = playerRb.velocity;
            playerRb.velocity = new Vector3(0, playerVelocity.y, 0);
        }

        private void SetTimestepMultiplier() => _timestepMultiplier = INITIAL_TIMESTEP / Time.fixedDeltaTime;
    }
}
