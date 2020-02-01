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

    private AudioSource audioSource;

    private void Awake() {
        downFrontPosition = transform.position;
        downBackPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + backDistance);
        topFrontPosition = new Vector3(transform.position.x, Bunker.instance.secondLevelHeight, transform.position.z);
        topBackPosition = new Vector3(transform.position.x, Bunker.instance.secondLevelHeight, transform.position.z + backDistance);

        audioSource = GetComponent<AudioSource>();
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

    protected override void InteractLoop() {
        if (Input.GetButtonDown("Use") && !player.AutoMoving && !player.HoldingHose) {
            if (player.OnFirstFloor) {
                player.AddAutoMovement(downFrontPosition, positioningTime, false, 1);
                player.AddAutoMovement(downBackPosition, enteringTime, false, 0);
                player.AddAutoMovement(downBackPosition, positioningTime, true, 2);
                player.AddAutoMovement(topBackPosition, travelingTime, true, 0);
                player.AddAutoMovement(topFrontPosition, enteringTime, false, 0);
                player.AddAutoMovement(topFrontPosition, positioningTime, true, 0);
            } else {
                player.AddAutoMovement(topFrontPosition, positioningTime, false, 1);
                player.AddAutoMovement(topBackPosition, enteringTime, false, 0);
                player.AddAutoMovement(topBackPosition, positioningTime, true, 2);
                player.AddAutoMovement(downBackPosition, travelingTime, true, 0);
                player.AddAutoMovement(downFrontPosition, enteringTime, false, 0);
                player.AddAutoMovement(downFrontPosition, positioningTime, true, 0);
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
        audioSource.Play();
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

        audioSource.Play();
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
}
