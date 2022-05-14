using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class ElevatorSystem : MonoBehaviour
    {
        [SerializeField] Animator elevatorAnimator;
        [SerializeField] ElevatorButton[] elevatorButtons;
        [SerializeField] Floor[] floors;

        private bool _isElevatorMoving = false;
        private bool _areDoorsClosed = true;
        private int _currentElevatorLevel = 0;
        private const float ELEVATOR_SPEED = 4f;

        private void OnValidate() => SortFloorsByHeight();

        private void OnEnable()
        {
            foreach (Floor floor in floors) floor.OnButtonClicked += MoveElevatorToPlayer;
            foreach (ElevatorButton button in elevatorButtons) button.OnButtonClicked += MoveElevatorToFloor;
        }

        private void OnDisable()
        {
            foreach (Floor floor in floors) floor.OnButtonClicked -= MoveElevatorToPlayer;
            foreach (ElevatorButton button in elevatorButtons) button.OnButtonClicked -= MoveElevatorToFloor;
        }

        private void Start() => SetFloorLevels();

        private void MoveElevatorToPlayer(int targetLevel)
        {
            if (_isElevatorMoving) return;

            Floor targetFloor = floors[targetLevel];

            if (targetLevel == _currentElevatorLevel)
            {
                OpenTheDoor(targetFloor.DoorsAnimator);
            }
            else
            {
                Vector3 targetPosition = new Vector3(transform.position.x, targetFloor.ElevatorTarget.position.y, transform.position.z);
                StartCoroutine(MoveElevatorCoroutine(targetPosition, targetFloor.DoorsAnimator, targetLevel));
            }
        }

        private void MoveElevatorToFloor(int targetLevel)
        {
            if (targetLevel == _currentElevatorLevel || _isElevatorMoving) return;

            Floor targetFloor = floors[targetLevel];
            Animator currentFloorAnimator = floors[_currentElevatorLevel].DoorsAnimator;

            CloseTheDoor(currentFloorAnimator);

            Vector3 targetPosition = new Vector3(transform.position.x, targetFloor.ElevatorTarget.position.y, transform.position.z);
            StartCoroutine(MoveElevatorCoroutine(targetPosition, targetFloor.DoorsAnimator, targetLevel));
        }

        private IEnumerator MoveElevatorCoroutine(Vector3 targetPosition, Animator doorsAnimator, int targetFloorLevel)
        {
            _isElevatorMoving = true;

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                float step = ELEVATOR_SPEED * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                yield return null;
            }

            OpenTheDoor(doorsAnimator);

            _currentElevatorLevel = targetFloorLevel;
            _isElevatorMoving = false;
        }

        private void OpenTheDoor(Animator doorsAnimator)
        {
            elevatorAnimator.SetTrigger("Open");
            doorsAnimator.SetTrigger("Open");
        }

        private void CloseTheDoor(Animator doorsAnimator)
        {
            elevatorAnimator.SetTrigger("Close");
            doorsAnimator.SetTrigger("Close");
        }

        private void SetFloorLevels()
        {
            for (int i = 0; i < floors.Length; i++)
            {
                floors[i].SetFloorLevel(i);
            }
        }

        private void SortFloorsByHeight()
        {
            if (floors.Length == 0) return;

            Array.Sort(floors, (x, y) => x.ElevatorTarget.position.y.CompareTo(y.ElevatorTarget.position.y));
        }
    }
}
