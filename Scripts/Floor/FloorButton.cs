using System;
using UnityEngine;

namespace ElevatorTask
{
    public class FloorButton : ButtonSound
    {
        public event Action<ButtonSound> OnFloorButtonClicked;

        protected override void OnMouseDown()
        {
            base.OnMouseDown();

            OnFloorButtonClicked?.Invoke(this);
        }
    }
}
