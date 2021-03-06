﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boiler : CriticalInteractable {
    [Header("Boiler")]
    public float successFix = 0.2f;
    public float indicatorSpeed = 0.8f;
    [Range(0, 1)]
    public float indicatorPosition = 0.5f;
    public float indicatorDetectionThreshold = 0.05f;
    public Hose boilerHose;

    [Header("Particles")]
    public ParticleSystem[] particles;
    public Light criticalLight;

    [Header("Sounds")]
    public AudioSource normalAudioSource;
    public AudioSource criticalAudioSource;
    public AudioSource needRepairAudioSource;
    public AudioSource minigameAudioSource;
    public AudioClip succesClip;
    public AudioClip errorClip;
    public AudioClip explosionClip;

    private bool finishedRepair;

    private float alpha;
    private bool increment;
    private BoilerBar boilerBar;

    protected override void Awake() {
        base.Awake();
        boilerBar = interactionIndicator.GetComponent<BoilerBar>();
    }

    protected override void InteractLoop() {
        if (repairState == RepairState.CRITICAL) {
            if (ShowingInteraction) {
                HideInteraction();
            }

            if (player.HoldingHose) {
                boilerHose.PlayHose();
                RestoreCriticalState();
                finishedRepair = true;
            }

        } else if ((repairState != RepairState.GOOD || !finishedRepair) && ShowingInteraction) {

            if (increment) {
                alpha += Time.deltaTime * indicatorSpeed;
            } else {
                alpha -= Time.deltaTime * indicatorSpeed;
            }

            if (alpha > 1f || alpha < 0f) {
                alpha = Mathf.Clamp01(alpha);
                increment = !increment;
            }

            if (boilerBar) {
                boilerBar.SetArrowPosition(alpha);
            }

            if (Input.GetButtonDown("Use")) {
                if (alpha >= indicatorPosition - indicatorDetectionThreshold && alpha <= indicatorPosition + indicatorDetectionThreshold) {
                    player.AutoRotate(true);
                    player.Hammer();
                    criticalState = Mathf.Clamp01(criticalState + successFix);
                    finishedRepair = criticalState == 1;
                    SetCriticalState();

                    minigameAudioSource.PlayOneShot(succesClip);

                    if (finishedRepair) {
                        HideInteraction();
                    }
                } else {
                    minigameAudioSource.PlayOneShot(errorClip);

                    ShowInteraction();
                }
            }
        }

        if (!ShowingInteraction && repairState == RepairState.NEEDREPAIR) {
            ShowInteraction();
        }
    }

    protected override void ApplyCriticalState() {
        if (repairState == RepairState.CRITICAL) {
            anim.SetBool("broken", true);
            criticalLight.enabled = true;
            foreach (ParticleSystem ps in particles) {
                ps.Play(true);
            }
            criticalAudioSource.volume = 1;
            normalAudioSource.volume = 0;
            needRepairAudioSource.volume = 0;
        } else if (repairState == RepairState.NEEDREPAIR) {
            anim.SetBool("broken", true);
            criticalLight.enabled = true;
            foreach (ParticleSystem ps in particles) {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            criticalAudioSource.volume = 0;
            normalAudioSource.volume = 0;
            needRepairAudioSource.volume = 1;
        } else {
            anim.SetBool("broken", false);
            criticalLight.enabled = false;
            foreach (ParticleSystem ps in particles) {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            criticalAudioSource.volume = 0;
            normalAudioSource.volume = 1;
            needRepairAudioSource.volume = 0;
        }
    }

    protected override void ShowInteraction() {
        if (repairState != RepairState.GOOD) {
            base.ShowInteraction();
            if (boilerBar) {
                boilerBar.ShowIndicator(indicatorPosition, indicatorDetectionThreshold);
            }
        }

        finishedRepair = false;
        alpha = 0f;
        increment = true;
    }
}
