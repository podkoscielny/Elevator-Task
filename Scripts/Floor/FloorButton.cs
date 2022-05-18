using System;
using UnityEngine;

namespace ElevatorTask
{
    public class FloorButton : ButtonSound
    {
        public event Action<ButtonSound> OnFloorButtonClicked;

        [SerializeField] InteractableDistance interactableDistance;

        protected override void OnMouseDown()
        {
            if (!Interactable.IsPlayerInRange(transform.position, PlayerLayer))
            {
                Interactable.ShowGetCloserPanel();
                return;
            }

            base.OnMouseDown();

            OnFloorButtonClicked?.Invoke(this);
        }
    }
}
