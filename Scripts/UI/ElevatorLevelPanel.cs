using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ElevatorTask
{
    public class ElevatorLevelPanel : MonoBehaviour
    {
        [SerializeField] ElevatorDataSO elevatorData;
        [SerializeField] TextMeshPro panelText;

        private void OnEnable() => elevatorData.OnElevatorLevelChanged += SetCurrentElevatorLevelText;

        private void OnDisable() => elevatorData.OnElevatorLevelChanged -= SetCurrentElevatorLevelText;

        private void SetCurrentElevatorLevelText(int elevatorLevel) => panelText.text = $"{elevatorLevel}";
    }
}
