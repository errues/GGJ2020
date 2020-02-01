using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalInteractable : Interactable {
    public enum RepairState{
        GOOD,
        NEEDREPAIR,
        CRITICAL
    }

    [Range(0, 1)]
    public float criticalSpeed = 0.1f;

    [Header("Repair States")]
    [Range(0, 1)]
    public float noNeedRepairState = 0.8f;
    [Range(0, 1)]
    public float criticalRepairState = 0.33f;

    protected float criticalState;
    protected RepairState repairState;

    protected virtual void Awake() {
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
        if (criticalState < criticalRepairState) {
            repairState = RepairState.CRITICAL;
            GetComponentInChildren<Renderer>().material.color = Color.red;
        } else if (criticalState < noNeedRepairState) {
            repairState = RepairState.NEEDREPAIR;
            GetComponentInChildren<Renderer>().material.color = Color.blue;
        } else {
            repairState = RepairState.GOOD;
            GetComponentInChildren<Renderer>().material.color = Color.green;
        }
    }

    public void RestoreCriticalState() {
        criticalState = 1f;
        repairState = RepairState.GOOD;
    }

    private void CriticalFinish() {
        GameController.instance.FinishGame();
    }
}
