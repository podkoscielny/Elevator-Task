using System;
using UnityEngine;

namespace ElevatorTask
{
    public class ElevatorButton : ButtonSound, IInteractable
    {
        public event Action<int, ButtonSound> OnButtonClicked;

        [field: SerializeField] public int TargetElevatorLevel;

        public void Interact()
        {
            if (!Interactable.IsPlayerInRange(transform.position, PlayerLayer))
            {
                Interactable.ShowGetCloserPanel();
                return;
            }

            PlayClickSound();
            OnButtonClicked?.Invoke(TargetElevatorLevel, this);
        }
    }
}
