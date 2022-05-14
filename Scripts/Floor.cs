using System;
using UnityEngine;

namespace ElevatorTask
{
    public class Floor : MonoBehaviour
    {
        public event Action<Floor> OnButtonClicked;

        [field: SerializeField] public Animator DoorsAnimator { get; private set; }
        [field: SerializeField] public Transform ElevatorTarget { get; private set; }

        [SerializeField] private FloorButton floorButton;

        public int FloorLevel { get; private set; }

        private void OnEnable() => floorButton.OnFloorButtonClicked += SendButtonClickInfo;

        private void OnDisable() => floorButton.OnFloorButtonClicked -= SendButtonClickInfo;

        public void SetFloorLevel(int level) => FloorLevel = level;

        private void SendButtonClickInfo() => OnButtonClicked?.Invoke(this);
    }
}
