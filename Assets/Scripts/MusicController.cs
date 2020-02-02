using System.Collections;
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
                audioSource.PlayOneShot(dc.clip);
                break;
            }
        }
    }
}
