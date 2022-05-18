using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class StepDetection : MonoBehaviour
    {
        [SerializeField] float stepHeight = 0.4f;
        [SerializeField] Transform stepDetection;
        [SerializeField] Transform groundCheck;
        [SerializeField] LayerMask groundMask;
        [SerializeField] Rigidbody playerRb;

        private void Start() => SetStepDetectionHeight();

        private void OnCollisionStay(Collision collision)
        {
            if (Physics.Raycast(stepDetection.position, Vector3.down, out RaycastHit hitInfo, stepHeight, groundMask))
                playerRb.position = Vector3.Lerp(playerRb.position, playerRb.position + (hitInfo.point - groundCheck.position), Time.deltaTime * 4);
        }

        private void SetStepDetectionHeight()
        {
            Vector3 stepDetectionHeight = new Vector3(stepDetection.position.x, stepDetection.position.y + stepHeight, stepDetection.position.z);
            stepDetection.position = stepDetectionHeight;
        }
    }
}
