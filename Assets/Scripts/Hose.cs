﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hose : Interactable {
    public Transform hose;
    public Transform tap;

    public float rollUpTime;
    public AnimationCurve rollUpCurve;
    public float distanceOffset;
    
    private float distance;
    private bool hoseAtOrigin;
    private bool rollingUp;
    private float initialTime;
    private float initialDistance;
    private float alpha;

    private Player ownPlayerReference;

    private void Awake() {
        hoseAtOrigin = true;
    }

    protected override void InteractLoop() {
        player.HoldingHose = Input.GetAxisRaw("Use") != 0;
        if (player.HoldingHose) {
            rollingUp = false;
            hoseAtOrigin = false;
            HideInteraction();
        }
    }

    protected override void Update() {
        base.Update();


        if (ownPlayerReference != null) {
            if (ownPlayerReference.HoldingHose && Input.GetAxisRaw("Use") == 0) {
                ownPlayerReference.HoldingHose = false;
            }

            if (ownPlayerReference.HoldingHose) {
                distance = Vector3.Distance(transform.position, ownPlayerReference.transform.position) - distanceOffset;
                SetHoseSize();
            } else if (!rollingUp && !hoseAtOrigin) {
                rollingUp = true;
                initialTime = Time.time;
                initialDistance = distance;
                alpha = 0;

                if (player != null) {
                    ShowInteraction();
                }
            }

            if (rollingUp) {
                alpha = (Time.time - initialTime) / rollUpTime;
                distance = Mathf.Lerp(initialDistance, 0, rollUpCurve.Evaluate(alpha));
                SetHoseSize();

                if (alpha >= 1) {
                    rollingUp = false;
                    hoseAtOrigin = true;
                }
            }
        }
    }

    private void SetHoseSize() {
        hose.localScale = new Vector3(distance / 1.015535f, hose.localScale.y, hose.localScale.z);
        tap.localPosition = new Vector3(distance, tap.localPosition.y, tap.localPosition.z);
    }

    protected override void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            player = other.GetComponent<Player>();
            if (ownPlayerReference == null) {
                ownPlayerReference = player;
            }

            if (!ShowingInteraction) {
                ShowInteraction();
            }
        }
    }
}
