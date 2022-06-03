using System;
using UnityEngine;

namespace ElevatorTask
{
    public class FloorButton : ButtonSound, IInteractable
    {
        public event Action<ButtonSound> OnFloorButtonClicked;

        public void Interact()
        {
            if (!Interactable.IsPlayerInRange(transform.position, PlayerLayer))
            {
                Interactable.ShowGetCloserPanel();
                return;
            }

            PlayClickSound();
            OnFloorButtonClicked?.Invoke(this);
        }
    }
}
