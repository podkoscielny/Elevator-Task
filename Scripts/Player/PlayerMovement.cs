using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;

        private const float MOVEMENT_SPEED = 5f;

        private void Update() => MovePlayer();

        private void MovePlayer()
        {
            float horizontalMovement = Input.GetAxis("Horizontal");
            float verticalMovement = Input.GetAxis("Vertical");

            Vector3 direction = transform.right * horizontalMovement + transform.forward * verticalMovement;

            characterController.Move(MOVEMENT_SPEED * Time.deltaTime * direction);
        }
    }
}
