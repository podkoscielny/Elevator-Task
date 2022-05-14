using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class ElevatorSystem : MonoBehaviour
    {
        [SerializeField] Animator elevatorAnimator;
        [SerializeField] Floor[] floors;

        private int _currentElevatorLevel = 0;

        private void OnValidate() => SortFloorsByHeight();

        private void OnEnable()
        {
            foreach (Floor floor in floors) floor.OnButtonClicked += MoveElevatorToFloor;
        }

        private void OnDisable()
        {
            foreach (Floor floor in floors) floor.OnButtonClicked -= MoveElevatorToFloor;
        }

        private void Start() => SetFloorLevels();

        private void MoveElevatorToFloor(Floor targetFloor)
        {
            if (targetFloor.FloorLevel == _currentElevatorLevel)
            {
                elevatorAnimator.SetTrigger("Open");
                targetFloor.DoorsAnimator.SetTrigger("Open");
            }
            else
            {
                Vector3 targetPosition = new Vector3(transform.position.x, targetFloor.ElevatorTarget.position.y, transform.position.z);
                transform.position = targetPosition;

                elevatorAnimator.SetTrigger("Open");
                targetFloor.DoorsAnimator.SetTrigger("Open");
            }
        }

        private void SetFloorLevels()
        {
            for (int i = 0; i < floors.Length; i++)
            {
                floors[i].SetFloorLevel(i);
            }
        }

        private void SortFloorsByHeight() => Array.Sort(floors, (x, y) => x.ElevatorTarget.position.y.CompareTo(y.ElevatorTarget.position.y));
    }
}
