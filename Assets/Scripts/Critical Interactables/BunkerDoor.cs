using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerDoor : CriticalInteractable {
    [Header("Bunker Door")]
    public float pushButtonFix = 0.1f;
    public Rigidbody[] boards = new Rigidbody[3];
    public float boardSpeed = 2f;

    public AudioClip doorHit;

    private Vector3[] boardPositions;
    private Quaternion[] boardRotations;
    private bool finishedRepair;


    protected override void Awake() {
        base.Awake();
        boardPositions = new Vector3[3];
        boardRotations = new Quaternion[3];
        for(int i=0; i < boards.Length; i++) {
            boardPositions[i] = boards[i].transform.localPosition;
            boardRotations[i] = boards[i].transform.localRotation;
        }
    }

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

    protected override void SetCriticalState() {
        base.SetCriticalState();
        if (criticalState < noNeedRepairState - ((noNeedRepairState - criticalRepairState) / 2f) && criticalState >= criticalRepairState) {
            LaunchBoard(0);
        } else if (criticalState >= noNeedRepairState - ((noNeedRepairState - criticalRepairState) / 2f) && criticalState <= noNeedRepairState) {
            ReturnBoard(0);
        }
    }

    protected override void ApplyCriticalState() {
        if (repairState == RepairState.CRITICAL) {
            LaunchBoard(2);
        } else if (repairState == RepairState.NEEDREPAIR) {
            LaunchBoard(1);
            ReturnBoard(2);
        } else {
            ReturnBoard(0);
            ReturnBoard(1);
            ReturnBoard(2);
        }
    }

    private void LaunchBoard(int index) {
        if (boards[index].isKinematic) {
            boards[index].isKinematic = false;
        }
    }

    private void ReturnBoard(int index) {
        if (!boards[index].isKinematic) {
            boards[index].isKinematic = true;
            StartCoroutine(ReturnBoardCoroutine(index));
        }
    }

    private IEnumerator ReturnBoardCoroutine(int index) {
        float alpha = 0f;
        Vector3 initialPosition = boards[index].transform.localPosition;
        Quaternion initialRotation = boards[index].transform.localRotation;

        while (alpha < 1f) {
            alpha += Time.deltaTime * boardSpeed;
            boards[index].transform.localPosition = Vector3.Lerp(initialPosition, boardPositions[index], alpha);
            boards[index].transform.localRotation = Quaternion.Lerp(initialRotation, boardRotations[index], alpha);

            yield return null;
        }
    }

    protected override void ShowInteraction() {
        if (repairState != RepairState.GOOD) {
            base.ShowInteraction();
        }
    }
}
