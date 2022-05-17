using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public abstract class ButtonSound : MonoBehaviour
    {
        [field: SerializeField] public LayerMask PlayerLayer { get; private set; }

        [SerializeField] AudioSource buttonAudio;
        [SerializeField] AudioClip clickSound;
        [SerializeField] AudioClip buzzerSound;

        public void PlayBuzzerSound()
        {
            buttonAudio.clip = buzzerSound;
            buttonAudio.Play();
        }

        protected virtual void OnMouseDown()
        {
            buttonAudio.clip = clickSound;
            buttonAudio.Play();
        }
    }
}
