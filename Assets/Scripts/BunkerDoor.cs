using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerDoor : CriticalInteractable {

    public float pushButtonFix = 0.1f;

    protected override void InteractLoop() {
        if (Input.GetKeyDown(KeyCode.E)) {
            criticalState = Mathf.Clamp01(criticalState + pushButtonFix);
        }
    }
}
