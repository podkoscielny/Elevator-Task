using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ElevatorTask
{
    public class ElevatorLevelPanel : MonoBehaviour
    {
        [SerializeField] ElevatorSystem elevatorSystem;
        [SerializeField] TextMeshPro panelText;

        private void OnEnable() => elevatorSystem.OnElevatorLevelChanged += SetCurrentElevatorLevelText;

        private void OnDisable() => elevatorSystem.OnElevatorLevelChanged -= SetCurrentElevatorLevelText;

        private void SetCurrentElevatorLevelText(int elevatorLevel) => panelText.text = $"{elevatorLevel}";
    }
}
