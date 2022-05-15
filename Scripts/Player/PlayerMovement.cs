using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private CharacterController characterController;

        private const float MOVEMENT_SPEED = 2.5f;

        private void Update() => MovePlayer();

        private void MovePlayer()
        {
            float horizontalMovement = Input.GetAxisRaw("Horizontal");
            float verticalMovement = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(horizontalMovement, verticalMovement).normalized;
            Vector3 direction = transform.right * horizontalMovement + transform.forward * verticalMovement;

            playerAnimator.SetFloat("Movement", movement.magnitude);
            playerAnimator.SetFloat("MovementX", horizontalMovement);
            playerAnimator.SetFloat("MovementY", verticalMovement);

            characterController.Move(MOVEMENT_SPEED * Time.deltaTime * direction);
        }
    }
}
