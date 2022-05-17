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
        [SerializeField] AudioClip buzzerSound;

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
            SetInitialElevatorPosition();
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
                    OpenTheDoor(currentFloor.DoorsAnimator);
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
                    CloseTheDoor(currentFloor.DoorsAnimator);
                }
            }
        }

        private bool IsObjectsMaskCollidable(int objectsLayer) => (collidableLayer.value & (1 << objectsLayer)) > 0;

        public void SetDoorsToClosed() //Invoke after close_doors animation in Animation Event 
        {
            _areDoorsClosing = false;
            _areDoorsClosed = true;
        }

        private void MoveElevatorToPlayer(int targetLevel, ButtonSound buttonSound)
        {
            if (_isElevatorMoving)
            {
                buttonSound.PlayBuzzerSound();
                return;
            }

            Floor targetFloor = floors[targetLevel];

            if (targetLevel == _currentElevatorLevel)
            {
                if (_areDoorsClosed)
                {
                    _areDoorsClosed = false;
                    OnElevatorDoorsOpened?.Invoke();
                }
                else
                    buttonSound.PlayBuzzerSound();
            }
            else
            {
                _isDestinationSet = true;
                Vector3 targetPosition = new Vector3(transform.position.x, targetFloor.ElevatorTarget.position.y, transform.position.z);
                CloseTheDoor(floors[_currentElevatorLevel].DoorsAnimator);
                StartCoroutine(MoveElevatorCoroutine(targetPosition, targetLevel));
            }
        }

        private void MoveElevatorToFloor(int targetLevel, ButtonSound buttonSound)
        {
            if (targetLevel == _currentElevatorLevel || _isElevatorMoving || _isDestinationSet)
            {
                buttonSound.PlayBuzzerSound();
                return;
            }

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
        }

        private void OpenDoorAfterDoorbell()
        {
            _isDestinationSet = false;
            OpenTheDoor(floors[_currentElevatorLevel].DoorsAnimator);
        }

        private void OpenTheDoor(Animator doorsAnimator)
        {
            _areDoorsClosed = false;

            OnDoorsMoved?.Invoke();

            float stateNormalizedTime = Mathf.Min(1, elevatorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            float transitionOffset = 1 - stateNormalizedTime;

            elevatorAnimator.CrossFade("open_doors", 0, 0, transitionOffset);
            doorsAnimator.CrossFade("open_doors", 0, 0, transitionOffset);
        }

        private void CloseTheDoor(Animator doorsAnimator)
        {
            if (collidablesBlockingDoors.Count > 0) return;

            _areDoorsClosing = true;

            OnDoorsMoved?.Invoke();

            float stateNormalizedTime = Mathf.Min(1, elevatorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            float transitionOffset = 1 - stateNormalizedTime;

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

        private void SetInitialElevatorPosition()
        {
            float firstFloorPositionY = floors[0].ElevatorTarget.position.y;
            Vector3 targetPosition = new Vector3(transform.position.x, firstFloorPositionY, transform.position.z);

            transform.position = targetPosition;
        }

        private void SortFloorsByHeight()
        {
            if (floors.Length == 0) return;

            Array.Sort(floors, (x, y) => x.ElevatorTarget.position.y.CompareTo(y.ElevatorTarget.position.y));
        }
    }
}
