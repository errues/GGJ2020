using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Interactable {
    public GameObject secondFloorInteractionIndicator;

    public float positioningTime;
    public float enteringTime;
    public float travelingTime;

    public float backDistance;

    private Vector3 downFrontPosition;
    private Vector3 downBackPosition;
    private Vector3 topFrontPosition;
    private Vector3 topBackPosition;

    private void Awake() {
        downFrontPosition = transform.position;
        downBackPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + backDistance);
        topFrontPosition = new Vector3(transform.position.x, Bunker.instance.secondLevelHeight, transform.position.z);
        topBackPosition = new Vector3(transform.position.x, Bunker.instance.secondLevelHeight, transform.position.z + backDistance);
    }

    protected override void InteractLoop() {
        if (Input.GetAxisRaw("Use") != 0 && !player.AutoMoving) {
            if (player.OnFirstFloor) {
                player.AddAutoMovement(downFrontPosition, positioningTime);
                player.AddAutoMovement(downBackPosition, enteringTime);
                player.AddAutoMovement(topBackPosition, travelingTime);
                player.AddAutoMovement(topFrontPosition, enteringTime);
                player.OnFirstFloor = false;
            } else {
                player.AddAutoMovement(topFrontPosition, positioningTime);
                player.AddAutoMovement(topBackPosition, enteringTime);
                player.AddAutoMovement(downBackPosition, travelingTime);
                player.AddAutoMovement(downFrontPosition, enteringTime);
                player.OnFirstFloor = true;
            }

            player.StartAutomovement();
            HideInteraction();
        }
    }

    protected override void ShowInteraction() {
        if (player.OnFirstFloor) {
            interactionIndicator.SetActive(true);
        } else {
            secondFloorInteractionIndicator.SetActive(true);
        }
        ShowingInteraction = true;
    }

    protected override void HideInteraction() {
        base.HideInteraction();
        secondFloorInteractionIndicator.SetActive(false);
    }
}
