using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerDoor : CriticalInteractable {
    [Header("Bunker Door")]
    public float pushButtonFix = 0.1f;

    private bool finishedRepair;

    protected override void InteractLoop() {
        if (Input.GetButtonDown("Use") && (repairState != RepairState.GOOD || !finishedRepair)) {
            player.Hammer();

            criticalState = Mathf.Clamp01(criticalState + pushButtonFix);
            finishedRepair = criticalState == 1;

            if (finishedRepair) {
                HideInteraction();
            }
        }

        if (finishedRepair && repairState != RepairState.GOOD) {
            ShowInteraction();
        }
    }
}
