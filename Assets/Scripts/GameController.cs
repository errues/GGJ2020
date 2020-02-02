using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {
    public static GameController instance;

    public CanvasController canvasController;
    public float timeInSeconds;

    private float timeLeft;

    private void Awake() {
        instance = this;

        timeLeft = timeInSeconds;
    }

    public void FinishGame(CauseOfDeath cause) {
        if (cause != CauseOfDeath.VICTORY) {
            GetComponent<MusicController>().PlayDeathClip(cause);
        }

        canvasController.ShowFinishPanel(cause);
    }

    private void Update() {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0) {
            canvasController.ShowFinishPanel(CauseOfDeath.VICTORY);
        }

        canvasController.SetTimer(Mathf.CeilToInt(timeLeft));
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
