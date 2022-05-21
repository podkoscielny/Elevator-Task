using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class Photocell : MonoBehaviour
    {
        //private void OnTriggerEnter(Collider collider) => HandlePhotoCellEntered(collider);

        //private void OnTriggerExit(Collider collider) => HandlePhotoCellExit(collider);

        //private void HandlePhotoCellEntered(Collider collider)
        //{
        //    if (_isElevatorMoving || _areDoorsClosed) return;

        //    GameObject collidableObject = collider.gameObject;

        //    if (IsObjectsMaskCollidable(collidableObject.layer) && !collidablesBlockingDoors.Contains(collidableObject))
        //    {
        //        collidablesBlockingDoors.Add(collidableObject);

        //        if (_areDoorsClosing)
        //        {
        //            _areDoorsClosing = false;

        //            Floor currentFloor = floors[CurrentElevatorLevel];
        //            OpenTheDoor(currentFloor.DoorsAnimator);
        //        }
        //    }
        //}

        //private void HandlePhotoCellExit(Collider collider)
        //{
        //    if (_isElevatorMoving || _areDoorsClosed) return;

        //    GameObject collidableObject = collider.gameObject;

        //    if (IsObjectsMaskCollidable(collidableObject.layer) && collidablesBlockingDoors.Contains(collidableObject))
        //    {
        //        collidablesBlockingDoors.Remove(collidableObject);

        //        if (_isDestinationSet && collidablesBlockingDoors.Count == 0)
        //        {
        //            Floor currentFloor = floors[CurrentElevatorLevel];
        //            CloseTheDoor(currentFloor.DoorsAnimator);
        //        }
        //    }
        //}
    }
}
