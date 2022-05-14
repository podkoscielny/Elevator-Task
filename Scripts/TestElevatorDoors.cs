using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class TestElevatorDoors : MonoBehaviour
    {
        [SerializeField] Animator elevatorAnimator;
        [SerializeField] Animator floorDoorsAnimator;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                elevatorAnimator.SetTrigger("Close");
                floorDoorsAnimator.SetTrigger("Close");
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                elevatorAnimator.SetTrigger("Open");
                floorDoorsAnimator.SetTrigger("Open");
            }
        }
    }
}
