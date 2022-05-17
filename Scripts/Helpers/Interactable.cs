using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public static class Interactable
    {
        private const float INTERACTABLE_MAX_DISTANCE = 2.4f;

        public static bool IsPlayerInRange(Vector3 interactablePosition, LayerMask playerMask)
        {
            Collider[] colliders = Physics.OverlapSphere(interactablePosition, INTERACTABLE_MAX_DISTANCE, playerMask);
            
            return colliders.Length > 0;
        }
    }
}
