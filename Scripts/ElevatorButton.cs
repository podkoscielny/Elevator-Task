using System;
using UnityEngine;

namespace ElevatorTask
{
    public class ElevatorButton : MonoBehaviour
    {
        public event Action<int> OnButtonClicked;

        [SerializeField] private int targetElevatorLevel;

        private void OnMouseDown()
        {
            OnButtonClicked?.Invoke(targetElevatorLevel);
        }
    }
}
