using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    public static CanvasController instance;

    public Transform finishPanel;

    public Image hungerFill;
    public Image energyFill;


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
}
