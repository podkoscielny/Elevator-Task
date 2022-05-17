using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class PlayerSound : MonoBehaviour
    {
        [SerializeField] AudioSource playerAudio;

        public void PlayFootstepSound() => playerAudio.Play();
    }
}
