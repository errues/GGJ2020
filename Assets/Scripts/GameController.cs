using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController instance;

    public CanvasController canvasController;
    public float timeInSeconds;

    private float timeLeft;

    private void Awake() {
        instance = this;

        timeLeft = timeInSeconds;
    }

    public void FinishGame() {

        canvasController.ShowFinishPanel();
    }

    private void Update() {
        timeLeft -= Time.deltaTime;

        canvasController.SetTimer(Mathf.CeilToInt(timeLeft));
    }
}
