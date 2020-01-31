using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    protected List<Transform> playersInInteraction;

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

            ShowInteraction();
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

    }

    protected virtual void HideInteraction() {

    }
}
