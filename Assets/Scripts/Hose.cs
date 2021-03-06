﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hose : Interactable {
    public Transform hose;
    public Transform tap;

    public float rollUpTime;
    public AnimationCurve rollUpCurve;
    public float distanceOffset;

    public AudioClip splashWater;
    public ParticleSystem waterParticles;
    
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
            // Coge la manguera
            rollingUp = false;
            hoseAtOrigin = false;
            HideInteraction();
            ownPlayerReference.HosePose(true);
        }
    }

    protected override void Update() {
        base.Update();


        if (ownPlayerReference != null) {
            if (ownPlayerReference.HoldingHose && Input.GetAxisRaw("Use") == 0) {
                ownPlayerReference.HoldingHose = false;
            }
        }
    }

    private void LateUpdate() {
        if (ownPlayerReference != null) {
            if (ownPlayerReference.HoldingHose && Input.GetAxisRaw("Use") == 0) {
                ownPlayerReference.HoldingHose = false;
            }

            if (ownPlayerReference.HoldingHose) {
                distance = Vector3.Distance(transform.position, ownPlayerReference.transform.position) - distanceOffset;
                SetHoseSize();
            } else if (!rollingUp && !hoseAtOrigin) {
                // Suelta la manguera
                ownPlayerReference.HosePose(false);

                if (player != null) {
                    ShowInteraction();
                }
                rollingUp = true;
                initialTime = Time.time;
                initialDistance = distance;
                alpha = 0;
            }

            if (rollingUp) {
                alpha = (Time.time - initialTime) / rollUpTime;
                distance = Mathf.Lerp(initialDistance, 0, rollUpCurve.Evaluate(alpha));

                if (alpha >= 1) {
                    rollingUp = false;
                    hoseAtOrigin = true;
                }

                SetHoseSize();
            }
        }
    }

    private void SetHoseSize() {
        hose.localScale = new Vector3(distance / 1.015535f, hoseAtOrigin ? 0 : 1, hoseAtOrigin ? 0 : 1);
        tap.localPosition = new Vector3(distance, tap.localPosition.y, tap.localPosition.z);
    }

    public void PlayHose() {
        AudioSource.PlayClipAtPoint(splashWater, waterParticles.transform.position);
        waterParticles.Play();
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
