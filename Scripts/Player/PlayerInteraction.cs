using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] Transform mainCamera;
        [SerializeField] LayerMask interactableMask;

        private void Start() => LockCursor();

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) CheckInteractableItem();
        }

        private void LockCursor() => Cursor.lockState = CursorLockMode.Locked;

        private void CheckInteractableItem()
        {
            Physics.Raycast(mainCamera.position, mainCamera.forward, out RaycastHit hitInfo, Mathf.Infinity, interactableMask);

            if (hitInfo.collider != null && hitInfo.transform.TryGetComponent(out IInteractable interactable))
                interactable.Interact();
        }
    }
}
