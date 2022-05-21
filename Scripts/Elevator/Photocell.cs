using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class Photocell : MonoBehaviour
    {
        public event Action OnCollidableBlockingDoorsAdded;
        public event Action OnCollidableBlockingDoorsRemoved;

        [SerializeField] ElevatorDataSO elevatorData;

        private void OnTriggerEnter(Collider collider) => HandlePhotoCellEntered(collider);

        private void OnTriggerExit(Collider collider) => HandlePhotoCellExit(collider);

        private void HandlePhotoCellEntered(Collider collider)
        {
            if (elevatorData.IsElevatorMoving || elevatorData.AreDoorsClosed) return;

            GameObject collidableObject = collider.gameObject;

            if (IsObjectsMaskCollidable(collidableObject.layer) && !elevatorData.CollidablesBlockingDoors.Contains(collidableObject))
            {
                elevatorData.CollidablesBlockingDoors.Add(collidableObject);

                OnCollidableBlockingDoorsAdded?.Invoke();
            }
        }

        private void HandlePhotoCellExit(Collider collider)
        {
            if (elevatorData.IsElevatorMoving || elevatorData.AreDoorsClosed) return;

            GameObject collidableObject = collider.gameObject;

            if (IsObjectsMaskCollidable(collidableObject.layer) && elevatorData.CollidablesBlockingDoors.Contains(collidableObject))
            {
                elevatorData.CollidablesBlockingDoors.Remove(collidableObject);

                OnCollidableBlockingDoorsRemoved?.Invoke();
            }
        }

        private bool IsObjectsMaskCollidable(int objectsLayer) => (elevatorData.CollidableLayer.value & (1 << objectsLayer)) > 0;
    }
}
