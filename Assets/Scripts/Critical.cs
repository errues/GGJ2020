using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critical : MonoBehaviour {

    public float criticalMaxValue = 10f;
    public float criticalSpeed = 1f;

    private float criticalState;

    private void Awake() {
        RestoreCriticalState();
    }

    private void Update() {
        criticalState -= Time.deltaTime * criticalSpeed;
        ApplyCriticalState();

        if (criticalState <= 0) {
            CriticalFinish();
        }
    }

    public virtual void ApplyCriticalState() {
        GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.red, Color.white, criticalState / criticalMaxValue);
    }

    public void RestoreCriticalState() {
        criticalState = criticalMaxValue;
    }

    private void CriticalFinish() {
        GameController.instance.FinishGame();
    }
}
