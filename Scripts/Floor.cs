using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class Floor : MonoBehaviour
    {
        [field: SerializeField] public Animator DoorsAnimator { get; private set; }
        [field: SerializeField] public Transform ElevatorTarget { get; private set; }
    }
}
