using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class ElevatorSystem : MonoBehaviour
    {
        public event Action OnElevatorMovementStarted;
        public event Action OnElevatorMovementEnded;
        public event Action OnElevatorDoorsOpened;
        public event Action OnDoorsMoved;

        [SerializeField] Animator elevatorAnimator;
        [SerializeField] LayerMask collidableLayer;
        [SerializeField] ElevatorSound elevatorSound;

        [SerializeField] ElevatorButton[] elevatorButtons;
        [SerializeField] Floor[] floors;

        private List<GameObject> collidablesBlockingDoors = new List<GameObject>();

        private bool _isElevatorMoving = false;
        private bool _areDoorsClosed = true;
        private bool _areDoorsClosing = false;
        private bool _isDestinationSet = false;

        private int _currentElevatorLevel = 0;
        private const float ELEVATOR_SPEED = 4f;

        private void OnValidate() => SortFloorsByHeight();

        private void OnEnable()
        {
            elevatorSound.OnDoorbellSoundPlayed += OpenDoorAfterDoorbell;
            foreach (Floor floor in floors) floor.OnButtonClicked += MoveElevatorToPlayer;
            foreach (ElevatorButton button in elevatorButtons) button.OnButtonClicked += MoveElevatorToFloor;
        }

        private void OnDisable()
        {
            elevatorSound.OnDoorbellSoundPlayed -= OpenDoorAfterDoorbell;
            foreach (Floor floor in floors) floor.OnButtonClicked -= MoveElevatorToPlayer;
            foreach (ElevatorButton button in elevatorButtons) button.OnButtonClicked -= MoveElevatorToFloor;
        }

        private void Start()
        {
            SetFloorLevels();
            EnableElevatorButtons();
        }

        private void OnTriggerStay(Collider collider)
        {
            if (_isElevatorMoving || _areDoorsClosed) return;

            GameObject collidableObject = collider.gameObject;

            if (IsObjectsMaskCollidable(collidableObject.layer) && !collidablesBlockingDoors.Contains(collidableObject))
            {
                collidablesBlockingDoors.Add(collidableObject);

                if (_areDoorsClosing)
                {
                    _areDoorsClosing = false;

                    Floor currentFloor = floors[_currentElevatorLevel];
                    OpenTheDoor(currentFloor.DoorsAnimator, true);
                }
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (_isElevatorMoving || _areDoorsClosed) return;

            GameObject collidableObject = collider.gameObject;

            if (IsObjectsMaskCollidable(collidableObject.layer) && collidablesBlockingDoors.Contains(collidableObject))
            {
                collidablesBlockingDoors.Remove(collidableObject);

                if (_isDestinationSet && collidablesBlockingDoors.Count == 0)
                {
                    Floor currentFloor = floors[_currentElevatorLevel];
                    CloseTheDoor(currentFloor.DoorsAnimator, true);
                }
            }
        }

        private bool IsObjectsMaskCollidable(int objectsLayer) => (collidableLayer.value & (1 << objectsLayer)) > 0;

        public void SetDoorsToClosed() //Invoke after close_doors animation in Animation Event 
        {
            _areDoorsClosing = false;
            _areDoorsClosed = true;
        }

        private void MoveElevatorToPlayer(int targetLevel)
        {
            if (_isElevatorMoving) return;

            Floor targetFloor = floors[targetLevel];

            if (targetLevel == _currentElevatorLevel)
            {
                if (_areDoorsClosed) OnElevatorDoorsOpened?.Invoke();
            }
            else
            {
                _isDestinationSet = true;
                Vector3 targetPosition = new Vector3(transform.position.x, targetFloor.ElevatorTarget.position.y, transform.position.z);
                CloseTheDoor(floors[_currentElevatorLevel].DoorsAnimator);
                StartCoroutine(MoveElevatorCoroutine(targetPosition, targetLevel));
            }
        }

        private void MoveElevatorToFloor(int targetLevel)
        {
            if (targetLevel == _currentElevatorLevel || _isElevatorMoving) return;

            Floor targetFloor = floors[targetLevel];
            Animator currentFloorAnimator = floors[_currentElevatorLevel].DoorsAnimator;

            CloseTheDoor(currentFloorAnimator);

            _isDestinationSet = true;
            Vector3 targetPosition = new Vector3(transform.position.x, targetFloor.ElevatorTarget.position.y, transform.position.z);
            StartCoroutine(MoveElevatorCoroutine(targetPosition, targetLevel));
        }

        private IEnumerator MoveElevatorCoroutine(Vector3 targetPosition, int targetFloorLevel)
        {
            yield return new WaitUntil(() => _areDoorsClosed);

            _isElevatorMoving = true;

            OnElevatorMovementStarted?.Invoke();

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                float step = ELEVATOR_SPEED * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                yield return null;
            }

            OnElevatorMovementEnded.Invoke();

            _currentElevatorLevel = targetFloorLevel;
            _isElevatorMoving = false;
            _isDestinationSet = false;
        }

        private void OpenDoorAfterDoorbell() => OpenTheDoor(floors[_currentElevatorLevel].DoorsAnimator);

        private void OpenTheDoor(Animator doorsAnimator, bool invokeThroughPhotoCell = false)
        {
            _areDoorsClosed = false;

            OnDoorsMoved?.Invoke();

            float transitionOffset = invokeThroughPhotoCell ? 1 - elevatorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime : 0;

            elevatorAnimator.CrossFade("open_doors", 0, 0, transitionOffset);
            doorsAnimator.CrossFade("open_doors", 0, 0, transitionOffset);
        }

        private void CloseTheDoor(Animator doorsAnimator, bool invokeThroughPhotoCell = false)
        {
            if (collidablesBlockingDoors.Count > 0) return;

            _areDoorsClosing = true;

            OnDoorsMoved?.Invoke();

            float transitionOffset = invokeThroughPhotoCell ? 1 - elevatorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime : 0;

            elevatorAnimator.CrossFade("close_doors", 0, 0, transitionOffset);
            doorsAnimator.CrossFade("close_doors", 0, 0, transitionOffset);
        }

        private void SetFloorLevels()
        {
            for (int i = 0; i < floors.Length; i++)
            {
                floors[i].SetFloorLevel(i);
            }
        }

        private void EnableElevatorButtons()
        {
            foreach (ElevatorButton button in elevatorButtons)
            {
                bool setButtonToActive = button.TargetElevatorLevel <= floors.Length - 1;

                button.gameObject.SetActive(setButtonToActive);
            }
        }

        private void SortFloorsByHeight()
        {
            if (floors.Length == 0) return;

            Array.Sort(floors, (x, y) => x.ElevatorTarget.position.y.CompareTo(y.ElevatorTarget.position.y));
        }
    }
}
