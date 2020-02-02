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

        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public void FinishGame(CauseOfDeath cause) {
        if (cause != CauseOfDeath.VICTORY) {
            GetComponent<MusicController>().PlayDeathClip(cause);
        }

        AudioListener.pause = true;
        Time.timeScale = 0;
        canvasController.ShowFinishPanel(cause);
    }

    private void Update() {
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
        } else {
            FinishGame(CauseOfDeath.VICTORY);
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
