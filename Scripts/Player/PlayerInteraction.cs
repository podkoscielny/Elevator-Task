using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ElevatorTask
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] Transform mainCamera;
        [SerializeField] LayerMask interactableMask;

        private void Start() => LockCursor();

        public void CheckInteractableItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Physics.Raycast(mainCamera.position, mainCamera.forward, out RaycastHit hitInfo, Mathf.Infinity, interactableMask);

                Collider hitInfoCollider = hitInfo.collider;

                if (hitInfoCollider != null && hitInfoCollider.TryGetComponent(out IInteractable interactable))
                    interactable.Interact();
            }
        }

        private void LockCursor() => Cursor.lockState = CursorLockMode.Locked;
    }
}
