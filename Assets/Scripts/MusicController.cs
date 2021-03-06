﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
    [System.Serializable]
    public class DeathClip {
        public CauseOfDeath cause;
        public AudioClip clip;
    }

    public AudioClip musicClip;

    public List<DeathClip> deathClips;

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;
    }

    private void Start() {
        StartMusic();
    }

    public void StartMusic() {
        audioSource.clip = musicClip;
        audioSource.Play();
    }

    public void PlayDeathClip(CauseOfDeath cause) {
        audioSource.Stop();
        foreach (DeathClip dc in deathClips) {
            if (dc.cause == cause) {
                audioSource.clip = dc.clip;
                audioSource.Play();
                break;
            }
        }
    }

    public void GoToTime(float time) {
        audioSource.time = time;
    }
}
