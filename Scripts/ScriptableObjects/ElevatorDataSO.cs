using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private int _currentElevatorLevel = 0;
    }
}
