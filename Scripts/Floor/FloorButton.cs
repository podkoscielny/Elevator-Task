using System;
using UnityEngine;

namespace ElevatorTask
{
    public class FloorButton : ButtonSound
    {
        public event Action<ButtonSound> OnFloorButtonClicked;

        protected override void OnMouseDown()
        {
            if (!Interactable.IsPlayerInRange(transform.position, PlayerLayer)) return;

            base.OnMouseDown();

            OnFloorButtonClicked?.Invoke(this);
        }
    }
}
