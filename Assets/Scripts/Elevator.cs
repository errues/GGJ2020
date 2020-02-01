using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Interactable {
    [Header("References")]
    public GameObject secondFloorInteractionIndicator;
    public Transform downLeftDoor;
    public Transform downRightDoor;
    public Transform topLeftDoor;
    public Transform topRightDoor;

    [Header("Times")]
    public float positioningTime;
    public float enteringTime;
    public float travelingTime;

    [Header("Distances")]
    public float backDistance;

    private Vector3 downFrontPosition;
    private Vector3 downBackPosition;
    private Vector3 topFrontPosition;
    private Vector3 topBackPosition;

    private bool doorsMoving;
    private bool topDoors;
    private bool openingDoors;

    private float alpha;
    private float initialTime;

    private void Awake() {
        downFrontPosition = transform.position;
        downBackPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + backDistance);
        topFrontPosition = new Vector3(transform.position.x, Bunker.instance.secondLevelHeight, transform.position.z);
        topBackPosition = new Vector3(transform.position.x, Bunker.instance.secondLevelHeight, transform.position.z + backDistance);
    }

    protected override void Update() {
        base.Update();

        if (doorsMoving) {
            alpha = Mathf.Clamp((Time.time - initialTime) / positioningTime, 0, 1);

            Vector3 scale = new Vector3(openingDoors ? 1 - alpha : alpha, 1, 1);

            if (topDoors) {
                topLeftDoor.localScale = scale;
                topRightDoor.localScale = scale;
            } else {
                downLeftDoor.localScale = scale;
                downRightDoor.localScale = scale;
            }

            if ((alpha == 0 && !openingDoors) || (alpha == 1 && openingDoors)) {
                doorsMoving = false;
            }
        }
    }

    protected override void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            player = other.GetComponent<Player>();

            if (!ShowingInteraction && !player.HoldingHose) {
                ShowInteraction();
            }
        }
    }

    protected override void InteractLoop() {
        if (Input.GetAxisRaw("Use") != 0 && !player.AutoMoving && !player.HoldingHose) {
            if (player.OnFirstFloor) {
                player.AddAutoMovement(downFrontPosition, positioningTime);
                player.AddAutoMovement(downBackPosition, enteringTime);
                player.AddAutoMovement(downBackPosition, positioningTime);
                player.AddAutoMovement(topBackPosition, travelingTime);
                player.AddAutoMovement(topFrontPosition, enteringTime);
                player.AddAutoMovement(topFrontPosition, positioningTime);
            } else {
                player.AddAutoMovement(topFrontPosition, positioningTime);
                player.AddAutoMovement(topBackPosition, enteringTime);
                player.AddAutoMovement(topBackPosition, positioningTime);
                player.AddAutoMovement(downBackPosition, travelingTime);
                player.AddAutoMovement(downFrontPosition, enteringTime);
                player.AddAutoMovement(downFrontPosition, positioningTime);
            }

            StartCoroutine(DoorsSequence(player.OnFirstFloor));
            player.OnFirstFloor = !player.OnFirstFloor;
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

    private void OpenDownDoors() {
        doorsMoving = true;
        topDoors = false;
        openingDoors = true;

        initialTime = Time.time;
    }

    private void CloseDownDoors() {
        doorsMoving = true;
        topDoors = false;
        openingDoors = false;

        initialTime = Time.time;
    }

    private void OpenTopDoors() {
        doorsMoving = true;
        topDoors = true;
        openingDoors = true;

        initialTime = Time.time;
    }

    private void CloseTopDoors() {
        doorsMoving = true;
        topDoors = true;
        openingDoors = false;

        initialTime = Time.time;
    }

    private IEnumerator DoorsSequence(bool goingUp) {
        if (goingUp) {
            OpenDownDoors();
        } else {
            OpenTopDoors();
        }

        yield return new WaitForSeconds(positioningTime + enteringTime);

        if (goingUp) {
            CloseDownDoors();
        } else {
            CloseTopDoors();
        }

        yield return new WaitForSeconds(positioningTime + travelingTime);

        if (goingUp) {
            OpenTopDoors();
        } else {
            OpenDownDoors();
        }

        yield return new WaitForSeconds(positioningTime);

        if (goingUp) {
            CloseTopDoors();
        } else {
            CloseDownDoors();
        }
    }

    public bool PlayerInInteraction() {
        return player != null;
    }

    public void ForceShowInteraction() {
        ShowInteraction();
    }
}
