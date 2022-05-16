using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Rigidbody playerRb;

        private float _verticalMovement;
        private float _horizontalMovement;

        private const float MOVEMENT_SPEED = 120f;

        private void Update()
        {
            GetMovementInput();
            SetPlayerMovementAnimation();
        }

        private void FixedUpdate() => MovePlayer();

        private void MovePlayer()
        {
            ResetMovementVelocity();

            Vector3 direction = transform.right * _horizontalMovement + transform.forward * _verticalMovement;

            playerRb.AddForce(MOVEMENT_SPEED * direction);
        }

        private void SetPlayerMovementAnimation()
        {
            Vector2 movement = new Vector2(_horizontalMovement, _verticalMovement).normalized;

            playerAnimator.SetFloat("Movement", movement.magnitude);
            playerAnimator.SetFloat("MovementX", _horizontalMovement);
            playerAnimator.SetFloat("MovementY", _verticalMovement);
        }

        private void GetMovementInput()
        {
            _horizontalMovement = Input.GetAxisRaw("Horizontal");
            _verticalMovement = Input.GetAxisRaw("Vertical");
        }

        private void ResetMovementVelocity()
        {
            Vector3 playerVelocity = playerRb.velocity;
            playerRb.velocity = new Vector3(0, playerVelocity.y, 0);
        }
    }
}
