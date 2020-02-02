using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour {
    public static CanvasController instance;

    public Transform finishPanel;

    public Image hungerFill;
    public Image energyFill;

    public TextMeshProUGUI timer;


    private void Awake() {
        instance = this;
    }

    public void ShowFinishPanel() {
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
}
