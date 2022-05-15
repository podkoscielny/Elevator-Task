using System;
using UnityEngine;

namespace ElevatorTask
{
    public class FloorButton : MonoBehaviour
    {
        public event Action OnFloorButtonClicked;

        private void OnMouseDown()
        {
            OnFloorButtonClicked?.Invoke();
        }
    }
}
