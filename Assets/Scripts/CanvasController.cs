using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour {
    [System.Serializable]
    public class DeathPhrases {
        public CauseOfDeath cause;
        public List<string> phrases;
    }

    public static CanvasController instance;

    public Transform finishPanel;
    public TextMeshProUGUI causeOfDeath;
    public TextMeshProUGUI youDied;

    public Image hungerFill;
    public Image energyFill;

    public Button restartButton;

    public TextMeshProUGUI timer;

    public List<DeathPhrases> deathPhrases;

    public GameObject fog;
    public Image[] fogSprites;
    public float maxAlphaFog = 0.6f;



    private void Awake() {
        instance = this;
    }

    private void Start() {
        restartButton.Select();
    }

    public void ShowFinishPanel(CauseOfDeath cause) {
        foreach (DeathPhrases dp in deathPhrases) {
            if (dp.cause == cause) {
                causeOfDeath.text = dp.phrases[Random.Range(0, dp.phrases.Count)];
                break;
            }
        }

        finishPanel.gameObject.SetActive(true);
    }

    public void SetBarsValues(float hunger, float energy) {
        Vector3 scale = hungerFill.rectTransform.localScale;
        scale.x = hunger;
        hungerFill.rectTransform.localScale = scale;

        scale.x = energy;
        energyFill.rectTransform.localScale = scale;
    }

    public void SetTimer(int seconds) {
        int mins = seconds / 60;
        int secs = seconds % 60;
        timer.text = mins.ToString() + ":" + (secs < 10 ? "0" : "") + secs.ToString();
    }

    public void ShowFog() {
        fog.SetActive(true);
    }

    public void HideFog() {
        fog.SetActive(false);
    }

    public void SetFogAlpha(float alpha) {
        foreach(Image img in fogSprites) {
            Color color = img.color;
            color.a = Mathf.Lerp(0, maxAlphaFog, alpha);
            img.color = color;
        }
    }
}
