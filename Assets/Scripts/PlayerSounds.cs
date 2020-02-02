using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSounds : MonoBehaviour {
    [Header("Sounds")]
    public List<AudioClip> hammerClips;
    public AudioMixerGroup hammerGroup;
    public List<AudioClip> stepClips;
    public AudioMixerGroup stepGroup;
    public List<AudioClip> ladderClips;
    public AudioMixerGroup ladderGroup;

    private AudioSource hammerSource;
    private AudioSource stepSource;
    private AudioSource ladderSource;

    private bool leftShoeLadder;

    private void Awake() {
        hammerSource = gameObject.AddComponent<AudioSource>();
        hammerSource.playOnAwake = false;
        hammerSource.outputAudioMixerGroup = hammerGroup;
        stepSource = gameObject.AddComponent<AudioSource>();
        stepSource.playOnAwake = false;
        stepSource.outputAudioMixerGroup = stepGroup;
        ladderSource = gameObject.AddComponent<AudioSource>();
        ladderSource.playOnAwake = false;
        ladderSource.outputAudioMixerGroup = ladderGroup;
    }

    public void PlayHammerSound() {
        if (hammerClips.Count > 0) {
            hammerSource.PlayOneShot(hammerClips[Random.Range(0, hammerClips.Count)]);
        }
    }

    public void PlayStepSound() {
        if (stepClips.Count > 0) {
            stepSource.PlayOneShot(stepClips[Random.Range(0, hammerClips.Count)]);
        }
    }

    public void PlayLadderSound() {
        if (ladderClips.Count > 0) {
            ladderSource.PlayOneShot(ladderClips[leftShoeLadder ? 0 : 1]);
            leftShoeLadder = !leftShoeLadder;
        }
    }
}
