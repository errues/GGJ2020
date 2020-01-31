using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public static GameController instance;

    public CanvasController canvasController;

    private void Awake() {
        instance = this;
    }

    public void FinishGame() {

        canvasController.ShowFinishPanel();
    }
}
