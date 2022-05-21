using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ElevatorTask
{
    [CreateAssetMenu(fileName = "ElevatorDataSO", menuName = "ScriptableObjects/ElevatorDataSO")]
    public class ElevatorDataSO : ScriptableObject
    {
        public event Action<int> OnElevatorLevelChanged;

        [field: SerializeField] public LayerMask CollidableLayer { get; private set; }

        public int CurrentElevatorLevel
        {
            get => _currentElevatorLevel;

            set
            {
                _currentElevatorLevel = value;
                OnElevatorLevelChanged?.Invoke(value);
            }
        }

        public bool IsElevatorMoving { get; set; } = false;
        public bool AreDoorsClosed { get; set; } = true;
        public bool AreDoorsClosing { get; set; } = false;
        public bool IsDestinationSet { get; set; } = false;
        public List<GameObject> CollidablesBlockingDoors { get; private set; } = new List<GameObject>();

        private int _currentElevatorLevel = 0;

#if UNITY_EDITOR
        private void OnEnable() => EditorApplication.playModeStateChanged += ResetElevatorState;

        private void OnDisable() => EditorApplication.playModeStateChanged -= ResetElevatorState;
#endif

        private void ResetElevatorState(PlayModeStateChange changedState)
        {
            if (changedState == PlayModeStateChange.ExitingPlayMode)
            {
                _currentElevatorLevel = 0;

                IsElevatorMoving = false;
                AreDoorsClosed = true;
                AreDoorsClosing = false;
                IsDestinationSet = false;

                CollidablesBlockingDoors = new List<GameObject>();
            }
        }
    }
}
