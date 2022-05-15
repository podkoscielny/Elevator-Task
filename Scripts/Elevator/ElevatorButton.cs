using System;
using UnityEngine;

namespace ElevatorTask
{
    public class ElevatorButton : MonoBehaviour
    {
        public event Action<int> OnButtonClicked;

        [field: SerializeField] public int TargetElevatorLevel;

        private void OnMouseDown()
        {
            OnButtonClicked?.Invoke(TargetElevatorLevel);
        }
    }
}
