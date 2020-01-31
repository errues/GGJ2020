using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    public Transform finishPanel;

    public void ShowFinishPanel() {
        finishPanel.gameObject.SetActive(true);
    }
}
