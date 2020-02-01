using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public GameObject interactionIndicator;

    protected Player player;

    protected bool ShowingInteraction { get; set; }

    protected virtual void Update() {
        if (player != null) {
            InteractLoop();
        }
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            player = other.GetComponent<Player>();

            if (!ShowingInteraction) {
                ShowInteraction();
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            player = null;
            HideInteraction();
        }
    }

    protected virtual void InteractLoop() {

    }

    protected virtual void ShowInteraction() {
        if (interactionIndicator != null) {
            interactionIndicator.SetActive(true);
        }
        ShowingInteraction = true;
    }

    protected virtual void HideInteraction() {
        if (interactionIndicator != null) {
            interactionIndicator.SetActive(false);
        }
        ShowingInteraction = false;
    }
}
