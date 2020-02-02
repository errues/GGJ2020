using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalInteractable : Interactable {
    [System.Serializable]
    public class CriticalSpeed {
        public float time = 60;
        public Vector2 criticalSpeed = new Vector2(0.01f, 0.02f);
    }

    public enum RepairState{
        GOOD,
        NEEDREPAIR,
        CRITICAL
    }

    public CauseOfDeath causeOfDeath;
    public float timeToStart = 0f;

    public CriticalSpeed[] criticalSpeeds = new CriticalSpeed[3];

    [Header("Repair States")]
    [Range(0, 1)]
    public float noNeedRepairState = 0.8f;
    [Range(0, 1)]
    public float criticalRepairState = 0.33f;

    protected Animator anim;

    protected float criticalState;
    protected RepairState repairState;

    private float currentCriticalSpeed;
    private int currentCriticalSpeedIndex;

    private float initialTime;


    public bool Enabled { get; private set; }

    protected virtual void Awake() {
        ChangeCriticalSpeed();
        currentCriticalSpeedIndex = 0;
        anim = GetComponentInChildren<Animator>();
    }

    private IEnumerator Start() {
        RestoreCriticalState();
        initialTime = Time.time;
        yield return new WaitForSeconds(timeToStart);
        Enabled = true;
    }

    protected override void Update() {
        base.Update();
        if (Enabled) {
            criticalState -= Time.deltaTime * currentCriticalSpeed;
            SetCriticalState();

            if (criticalState <= 0) {
                CriticalFinish();
            }
        }
    }

    protected void ChangeCriticalSpeed() {
        Vector2 speed;
        if (currentCriticalSpeedIndex == criticalSpeeds.Length) {
            speed = new Vector2(criticalSpeeds[currentCriticalSpeedIndex - 1].criticalSpeed.x, criticalSpeeds[currentCriticalSpeedIndex - 1].criticalSpeed.y);
        } else {
            if (currentCriticalSpeedIndex == 0) {
                speed = new Vector2(criticalSpeeds[currentCriticalSpeedIndex].criticalSpeed.x, criticalSpeeds[currentCriticalSpeedIndex].criticalSpeed.y);
            } else {
                speed = new Vector2(
                    Mathf.Lerp(criticalSpeeds[currentCriticalSpeedIndex - 1].criticalSpeed.x, criticalSpeeds[currentCriticalSpeedIndex].criticalSpeed.x, (criticalSpeeds[currentCriticalSpeedIndex].time - (Time.time - initialTime)) / criticalSpeeds[currentCriticalSpeedIndex].time),
                    Mathf.Lerp(criticalSpeeds[currentCriticalSpeedIndex - 1].criticalSpeed.y, criticalSpeeds[currentCriticalSpeedIndex].criticalSpeed.y, (criticalSpeeds[currentCriticalSpeedIndex].time - (Time.time - initialTime)) / criticalSpeeds[currentCriticalSpeedIndex].time)
                    );
            }

            if (Time.time - initialTime > criticalSpeeds[currentCriticalSpeedIndex].time) {
                currentCriticalSpeedIndex++;
            }
        }


        currentCriticalSpeed = Random.Range(speed.x, speed.y);
    }

    private void SetCriticalState() {
        RepairState lastRepairState = repairState;

        if (criticalState < criticalRepairState && repairState != RepairState.CRITICAL) {
            repairState = RepairState.CRITICAL;
        } else if (criticalState < noNeedRepairState && criticalState  >= criticalRepairState && repairState != RepairState.NEEDREPAIR) {
            repairState = RepairState.NEEDREPAIR;
        } else if (criticalState >= noNeedRepairState && repairState != RepairState.GOOD) {
            repairState = RepairState.GOOD;
        }

        if (lastRepairState != repairState) {
            ApplyCriticalState();
            ChangeCriticalSpeed();
        }
    }

    protected virtual void ApplyCriticalState() {
        if (repairState == RepairState.CRITICAL) {
            GetComponentInChildren<Renderer>().material.color = Color.red;
        } else if (repairState == RepairState.NEEDREPAIR) {
            GetComponentInChildren<Renderer>().material.color = Color.blue;
        } else {
            GetComponentInChildren<Renderer>().material.color = Color.green;
        }
    }

    public void RestoreCriticalState() {
        criticalState = 1f;
        repairState = RepairState.GOOD;
        ApplyCriticalState();
        ChangeCriticalSpeed();
    }

    private void CriticalFinish() {
        GameController.instance.FinishGame(causeOfDeath);
    }
}
