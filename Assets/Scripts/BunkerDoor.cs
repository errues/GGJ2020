using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerDoor : CriticalInteractable {

    public int pushButtonTimesToFix = 6;

    private int buttonTimesPushed;

    protected override void InteractLoop() {
        if (Input.GetKeyDown(KeyCode.E)) {
            buttonTimesPushed++;
            if (buttonTimesPushed == pushButtonTimesToFix) {
                buttonTimesPushed = 0;
                RestoreCriticalState();
            }
        }
    }

    protected override void ShowInteraction() {
        base.ShowInteraction();
        buttonTimesPushed = 0;
    }

    protected override void HideInteraction() {
        base.HideInteraction();
        buttonTimesPushed = 0;
    }
}
