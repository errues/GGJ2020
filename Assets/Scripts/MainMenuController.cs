﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
    [Header("Main Menu")]
    public Button playButton;

    [Header("Credits")]
    public GameObject creditsPanel;
    public Button backButton;

    [Header("Loading")]
    public GameObject loadingPanel;

    private void Start() {
        playButton.Select();
    }

    public void StartGame() {
        loadingPanel.SetActive(true);
        StartCoroutine(LoadAsyncScene("Alex_Pruebas"));
    }

    private IEnumerator LoadAsyncScene(string sceneName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

    public void ShowCredits() {
        creditsPanel.SetActive(true);
        backButton.Select();
    }

    public void HideCredits() {
        creditsPanel.SetActive(false);
        playButton.Select();
    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}