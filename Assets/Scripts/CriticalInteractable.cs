using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalInteractable : Interactable {
    public float criticalSpeed = 0.1f;

    protected float criticalState;

    protected void Awake() {
        RestoreCriticalState();
    }

    protected override void Update() {
        base.Update();
        criticalState -= Time.deltaTime * criticalSpeed;
        ApplyCriticalState();

        if (criticalState <= 0) {
            CriticalFinish();
        }
    }

    public virtual void ApplyCriticalState() {
        GetComponentInChildren<MeshRenderer>().material.color = Color.Lerp(Color.red, Color.white, criticalState);
    }

    public void RestoreCriticalState() {
        criticalState = 1f;
    }

    private void CriticalFinish() {
        GameController.instance.FinishGame();
    }
}
