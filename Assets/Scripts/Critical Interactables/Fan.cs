using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : CriticalInteractable {
    [System.Serializable]
    public struct FanStage {
        public float indicatorSpeed;
        public Color fillColor;
    }

    [Header("Fan")]
    public float successFix = 0.2f;
    public Color initialBackgroundColor = Color.white;
    public FanStage[] fanStages;
    public float indicatorDetectionThreshold = 0.1f;

    private bool finishedRepair;

    private float alpha;
    private int currentStage;
    private FanBar fanBar;

    protected override void Awake() {
        base.Awake();
        fanBar = interactionIndicator.GetComponent<FanBar>();
    }

    protected override void InteractLoop() {
        if (repairState != RepairState.GOOD || !finishedRepair) {

            alpha += Time.deltaTime * fanStages[currentStage].indicatorSpeed;

            if (fanBar) {
                fanBar.SetAlpha(alpha);
            }

            if (Input.GetButtonDown("Use")) {
                if (alpha >= 1f - indicatorDetectionThreshold && alpha <= 1f + indicatorDetectionThreshold) {
                    currentStage++;
                    if (currentStage == fanStages.Length) {
                        criticalState = Mathf.Clamp01(criticalState + successFix);
                        finishedRepair = criticalState == 1;

                        if (finishedRepair) {
                            HideInteraction();
                        } else {
                            ShowInteraction();
                        }
                    } else {
                        alpha = 0f;
                        fanBar?.SetState(fanStages[currentStage - 1].fillColor, fanStages[currentStage].fillColor);
                    }
                } else {
                    ShowInteraction();
                }
            }

            if (alpha > 1f + indicatorDetectionThreshold) {
                ShowInteraction();
            }
        }

        if (finishedRepair && repairState != RepairState.GOOD) {
            ShowInteraction();
        }
    }

    protected override void ShowInteraction() {
        base.ShowInteraction();

        finishedRepair = false;
        currentStage = 0;
        alpha = 0f;

        fanBar?.SetState(initialBackgroundColor, fanStages[currentStage].fillColor);
    }
}
