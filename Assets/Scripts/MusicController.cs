using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
    public AudioClip musicClip;

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        StartMusic();
    }

    public void StartMusic() {
        audioSource.clip = musicClip;
        audioSource.Play();
    }

    public void StopMusic() {
        audioSource.Stop();
    }
}
