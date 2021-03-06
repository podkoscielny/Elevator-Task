using System;
using UnityEngine;

namespace ElevatorTask
{
    public class Floor : MonoBehaviour
    {
        public event Action<int, ButtonSound> OnButtonClicked;

        [field: SerializeField] public Animator DoorsAnimator { get; private set; }
        [field: SerializeField] public Transform ElevatorTarget { get; private set; }

        [SerializeField] FloorButton floorButton;

        private int _floorLevel;

        private void OnEnable() => floorButton.OnFloorButtonClicked += SendButtonClickInfo;

        private void OnDisable() => floorButton.OnFloorButtonClicked -= SendButtonClickInfo;

        public void SetFloorLevel(int level) => _floorLevel = level;

        private void SendButtonClickInfo(ButtonSound buttonSound) => OnButtonClicked?.Invoke(_floorLevel, buttonSound);
    }
}
