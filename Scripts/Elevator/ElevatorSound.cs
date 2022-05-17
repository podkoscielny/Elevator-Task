using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElevatorTask
{
    public class ElevatorSound : MonoBehaviour
    {
        public event Action OnDoorbellSoundPlayed;

        [SerializeField] ElevatorSystem elevatorSystem;
        [SerializeField] AudioSource elevatorAudio;

        [Header("AudioClips")]
        [SerializeField] AudioClip elevatorMoveSound;
        [SerializeField] AudioClip doorMoveSound;
        [SerializeField] AudioClip doorbellSound;

        private WaitForSeconds _waitForDoorbell;

        private const float WAIT_FOR_DOORBELL_DURATION = 1f;
        private const float TURN_VOLUME_DOWN_LERP = 20f;

        private void OnEnable()
        {
            elevatorSystem.OnElevatorMovementStarted += PlayElevatorMovementSound;
            elevatorSystem.OnElevatorMovementEnded += TurnVolumeDownOnElevatorStopped;
            elevatorSystem.OnElevatorDoorsOpened += PlayDoorbellSound;
            elevatorSystem.OnDoorsMoved += PlayDoorsMovementSound;
        }

        private void OnDisable()
        {
            elevatorSystem.OnElevatorMovementStarted -= PlayElevatorMovementSound;
            elevatorSystem.OnElevatorMovementEnded -= TurnVolumeDownOnElevatorStopped;
            elevatorSystem.OnElevatorDoorsOpened -= PlayDoorbellSound;
            elevatorSystem.OnDoorsMoved -= PlayDoorsMovementSound;
        }

        private void Start() => _waitForDoorbell = new WaitForSeconds(WAIT_FOR_DOORBELL_DURATION);

        public void StopDoorsSound() //Invoke in open and close doors animation
        {
            elevatorAudio.Stop();
        }

        private void PlayDoorsMovementSound() => PlaySoundEffect(doorMoveSound, false);

        private void PlayElevatorMovementSound() => PlaySoundEffect(elevatorMoveSound, true);

        private void TurnVolumeDownOnElevatorStopped() => StartCoroutine(TurnVolumeDownCoroutine());

        private IEnumerator TurnVolumeDownCoroutine()
        {
            float timeElapsed = 0f;

            while (elevatorAudio.volume > 0.01f)
            {
                elevatorAudio.volume = Mathf.Lerp(elevatorAudio.volume, 0, timeElapsed / TURN_VOLUME_DOWN_LERP);
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            elevatorAudio.loop = false;
            elevatorAudio.volume = 1f;
            elevatorAudio.Stop();

            PlayDoorbellSound();
        }

        private void PlayDoorbellSound() => StartCoroutine(PlayDoorbellSoundCoroutine());

        private IEnumerator PlayDoorbellSoundCoroutine()
        {
            PlaySoundEffect(doorbellSound, false);

            yield return _waitForDoorbell;

            OnDoorbellSoundPlayed?.Invoke();
        }

        private void PlaySoundEffect(AudioClip soundEffect, bool isLoop)
        {
            elevatorAudio.clip = soundEffect;
            elevatorAudio.loop = isLoop;
            elevatorAudio.Play();
        }
    }
}
