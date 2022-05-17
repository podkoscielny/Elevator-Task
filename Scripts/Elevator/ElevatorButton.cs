using System;
using UnityEngine;

namespace ElevatorTask
{
    public class ElevatorButton : ButtonSound
    {
        public event Action<int, ButtonSound> OnButtonClicked;

        [field: SerializeField] public int TargetElevatorLevel;

        protected override void OnMouseDown()
        {
            if (!Interactable.IsPlayerInRange(transform.position, PlayerLayer)) return;

            base.OnMouseDown();

            OnButtonClicked?.Invoke(TargetElevatorLevel, this);
        }
    }
}
