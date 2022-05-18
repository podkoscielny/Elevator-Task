using System;
using UnityEngine;

namespace ElevatorTask
{
    public static class Interactable
    {
        public static event Action OnGetCloserPanelShowed;

        private const float INTERACTABLE_MAX_DISTANCE = 2.4f;

        public static bool IsPlayerInRange(Vector3 interactablePosition, LayerMask playerMask)
        {
            Collider[] colliders = Physics.OverlapSphere(interactablePosition, INTERACTABLE_MAX_DISTANCE, playerMask);
            
            return colliders.Length > 0;
        }

        public static void ShowGetCloserPanel() => OnGetCloserPanelShowed?.Invoke();
    }
}
