using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public GameObject interactionIndicator;

    protected List<Transform> playersInInteraction;

    protected bool ShowingInteraction { get; set; }

    protected virtual void Awake() {
        playersInInteraction = new List<Transform>();
    }

    protected virtual void Update() {
        if (playersInInteraction.Count > 0) {
            InteractLoop();
        }
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            playersInInteraction.Add(other.transform);

            if (!ShowingInteraction) {
                ShowInteraction();
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            playersInInteraction.Remove(other.transform);

            if (playersInInteraction.Count == 0) {
                HideInteraction();
            }
        }
    }

    protected virtual void InteractLoop() {

    }

    protected virtual void ShowInteraction() {
        interactionIndicator.SetActive(true);
        ShowingInteraction = true;
    }

    protected virtual void HideInteraction() {
        interactionIndicator.SetActive(false);
        ShowingInteraction = false;
    }
}
