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

        [SerializeField] ElevatorDataSO elevatorData;
        [SerializeField] Animator elevatorAnimator;
        [SerializeField] Photocell photocell;
        [SerializeField] ElevatorSound elevatorSound;
        [SerializeField] AudioClip buzzerSound;

        [SerializeField] ElevatorButton[] elevatorButtons;
        [SerializeField] Floor[] floors;

        private const float ELEVATOR_SPEED = 4f;

        private void OnValidate() => SortFloorsByHeight();

        private void OnEnable()
        {
            photocell.OnCollidableBlockingDoorsAdded += OpenTheDoorThroughPhotocell;
            photocell.OnCollidableBlockingDoorsRemoved += CloseTheDoorThroughPhotocell;
            elevatorSound.OnDoorbellSoundPlayed += OpenDoorAfterDoorbell;
            foreach (Floor floor in floors) floor.OnButtonClicked += MoveElevatorToPlayer;
            foreach (ElevatorButton button in elevatorButtons) button.OnButtonClicked += MoveElevatorToFloor;
        }

        private void OnDisable()
        {
            photocell.OnCollidableBlockingDoorsAdded -= OpenTheDoorThroughPhotocell;
            photocell.OnCollidableBlockingDoorsRemoved -= CloseTheDoorThroughPhotocell;
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

        public void SetDoorsToClosed() //Invoke after close_doors animation in Animation Event 
        {
            elevatorData.AreDoorsClosing = false;
            elevatorData.AreDoorsClosed = true;
        }

        private void OpenTheDoorThroughPhotocell()
        {
            if (elevatorData.AreDoorsClosing)
            {
                elevatorData.AreDoorsClosing = false;

                Floor currentFloor = floors[elevatorData.CurrentElevatorLevel];
                OpenTheDoor(currentFloor.DoorsAnimator);
            }
        }

        private void CloseTheDoorThroughPhotocell()
        {
            if (elevatorData.IsDestinationSet && elevatorData.CollidablesBlockingDoors.Count == 0)
            {
                Floor currentFloor = floors[elevatorData.CurrentElevatorLevel];
                CloseTheDoor(currentFloor.DoorsAnimator);
            }
        }

        private void MoveElevatorToPlayer(int targetLevel, ButtonSound buttonSound)
        {
            if (elevatorData.IsElevatorMoving)
            {
                buttonSound.PlayBuzzerSound();
                return;
            }

            if (targetLevel == elevatorData.CurrentElevatorLevel)
            {
                if (elevatorData.AreDoorsClosed)
                {
                    elevatorData.AreDoorsClosed = false;
                    OnElevatorDoorsOpened?.Invoke();
                }
                else
                    buttonSound.PlayBuzzerSound();
            }
            else
                SetElevatorDestination(targetLevel);
        }

        private void MoveElevatorToFloor(int targetLevel, ButtonSound buttonSound)
        {
            if (targetLevel == elevatorData.CurrentElevatorLevel || elevatorData.IsElevatorMoving || elevatorData.IsDestinationSet)
            {
                buttonSound.PlayBuzzerSound();
                return;
            }

            SetElevatorDestination(targetLevel);
        }

        private void SetElevatorDestination(int targetLevel)
        {
            elevatorData.IsDestinationSet = true;
            Vector3 targetPosition = new Vector3(transform.position.x, floors[targetLevel].ElevatorTarget.position.y, transform.position.z);
            CloseTheDoor(floors[elevatorData.CurrentElevatorLevel].DoorsAnimator);
            StartCoroutine(MoveElevatorCoroutine(targetPosition, targetLevel));
        }

        private IEnumerator MoveElevatorCoroutine(Vector3 targetPosition, int targetFloorLevel)
        {
            yield return new WaitUntil(() => elevatorData.AreDoorsClosed);

            SetElevatorMovementToStarted();

            int levelDifference = targetFloorLevel - elevatorData.CurrentElevatorLevel;

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                float step = ELEVATOR_SPEED * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                CheckElevatorLevel(targetFloorLevel, levelDifference);

                yield return null;
            }

            SetElevatorMovementToEnded(targetFloorLevel);
        }

        private void SetElevatorMovementToStarted()
        {
            elevatorData.IsElevatorMoving = true;
            OnElevatorMovementStarted?.Invoke();
        }

        private void SetElevatorMovementToEnded(int targetFloorLevel)
        {
            OnElevatorMovementEnded?.Invoke();

            elevatorData.CurrentElevatorLevel = targetFloorLevel;
            elevatorData.IsElevatorMoving = false;
        }

        private void CheckElevatorLevel(int targetLevel, int levelDifference)
        {
            if (targetLevel == elevatorData.CurrentElevatorLevel) return;

            if (levelDifference > 0)
            {
                int nextLevelIndex = elevatorData.CurrentElevatorLevel + 1;
                float nextLevelYPositon = floors[nextLevelIndex].ElevatorTarget.position.y;

                if (transform.position.y >= nextLevelYPositon) elevatorData.CurrentElevatorLevel = nextLevelIndex;
            }
            else
            {
                int nextLevelIndex = elevatorData.CurrentElevatorLevel - 1;
                float nextLevelYPositon = floors[nextLevelIndex].ElevatorTarget.position.y;

                if (transform.position.y <= nextLevelYPositon) elevatorData.CurrentElevatorLevel = nextLevelIndex;
            }
        }

        private void OpenDoorAfterDoorbell()
        {
            elevatorData.IsDestinationSet = false;
            OpenTheDoor(floors[elevatorData.CurrentElevatorLevel].DoorsAnimator);
        }

        private void OpenTheDoor(Animator doorsAnimator)
        {
            elevatorData.AreDoorsClosed = false;
            CrossfadeDoorsAnimation(doorsAnimator, "open_doors");
        }

        private void CloseTheDoor(Animator doorsAnimator)
        {
            if (elevatorData.CollidablesBlockingDoors.Count > 0) return;

            elevatorData.AreDoorsClosing = true;
            CrossfadeDoorsAnimation(doorsAnimator, "close_doors");
        }

        private void CrossfadeDoorsAnimation(Animator doorsAnimator, string animationName)
        {
            OnDoorsMoved?.Invoke();

            float stateNormalizedTime = Mathf.Min(1, elevatorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            float transitionOffset = 1 - stateNormalizedTime;

            elevatorAnimator.CrossFade(animationName, 0, 0, transitionOffset);
            doorsAnimator.CrossFade(animationName, 0, 0, transitionOffset);
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
            float firstFloorPositionY = floors[elevatorData.CurrentElevatorLevel].ElevatorTarget.position.y;
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
