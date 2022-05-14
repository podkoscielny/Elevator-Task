using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class TestElevatorDoors : MonoBehaviour
    {
        [SerializeField] Animator elevatorAnimator;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                elevatorAnimator.SetTrigger("Close");
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                elevatorAnimator.SetTrigger("Open");
            }
        }
    }
}
