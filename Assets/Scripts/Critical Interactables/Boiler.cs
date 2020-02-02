using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boiler : CriticalInteractable {
    [Header("Boiler")]
    public float successFix = 0.2f;
    public float indicatorSpeed = 0.8f;
    [Range(0, 1)]
    public float indicatorPosition = 0.5f;
    public float indicatorDetectionThreshold = 0.05f;

    [Header("Particles")]
    public ParticleSystem[] particles;

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
                criticalState = 1;
                finishedRepair = true;
            }

        } else if (repairState != RepairState.GOOD || !finishedRepair) {

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

                    if (finishedRepair) {
                        HideInteraction();
                    }
                } else {
                    ShowInteraction();
                }
            }
        }

        if (finishedRepair && repairState != RepairState.GOOD) {
            ShowInteraction();
        }
    }

    protected override void ApplyCriticalState() {
        if (repairState == RepairState.CRITICAL) {
            anim.SetBool("broken", true);
            foreach (ParticleSystem ps in particles) {
                ps.Play(true);
            }
        } else if (repairState == RepairState.NEEDREPAIR) {
            anim.SetBool("broken", true);
            foreach (ParticleSystem ps in particles) {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        } else {
            anim.SetBool("broken", false);
            foreach (ParticleSystem ps in particles) {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    protected override void ShowInteraction() {
        base.ShowInteraction();

        if (boilerBar) {
            boilerBar.ShowIndicator(indicatorPosition, indicatorDetectionThreshold);
        }

        finishedRepair = false;
        alpha = 0f;
        increment = true;
    }
}
