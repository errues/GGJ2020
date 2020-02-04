using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {
    public static GameController instance;

    public CanvasController canvasController;
    public float timeInSeconds;

    private float timeLeft;

    private bool gameFinished;

    private void Awake() {
        instance = this;

        timeLeft = timeInSeconds;

        Time.timeScale = 1;
        AudioListener.pause = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void FinishGame(CauseOfDeath cause) {
        if (!gameFinished) {
            gameFinished = true;

            if (cause != CauseOfDeath.VICTORY) {
                GetComponent<MusicController>().PlayDeathClip(cause);
            }

            AudioListener.pause = true;
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            canvasController.ShowFinishPanel(cause);
        }
    }

    private void Update() {
        if (!gameFinished) {
            if (timeLeft > 0) {
                timeLeft -= Time.deltaTime;
            } else {
                FinishGame(CauseOfDeath.VICTORY);
            }

            canvasController.SetTimer(Mathf.CeilToInt(timeLeft));

            if (Input.GetButtonDown("Cheat1") && Input.GetButton("Cheat2") && Input.GetButton("Cheat3")) {
                Cheat();
            }
        }
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    private void Cheat() {
        GetComponent<MusicController>().GoToTime(timeInSeconds);
        FinishGame(CauseOfDeath.VICTORY);
    }
}
