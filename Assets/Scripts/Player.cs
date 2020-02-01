using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float movementSpeed;
    public float inLadderMovementSpeed;

    public float frontZPos = 0;
    public float backZPos = 1;

    public bool OnFirstFloor { get; set; }
    public bool InLadder { get; set; }
    
    private CharacterController characterController;

    private float fallingSpeed;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        OnFirstFloor = true;
        InLadder = false;
    }

    private void Update() {
        if (InLadder) {
            characterController.Move(new Vector3(0, Input.GetAxisRaw("Vertical"), 0) * Time.deltaTime * inLadderMovementSpeed);
        } else {
            fallingSpeed += Constants.GRAVITY;
            characterController.Move(new Vector3(Input.GetAxis("Horizontal"), fallingSpeed, 0) * Time.deltaTime * movementSpeed);
        
            if (characterController.isGrounded) {
                fallingSpeed = Constants.GRAVITY;
            }
        }
    }

    public void SetOnFirstFloor() {
        characterController.enabled = false;
        transform.position = new Vector3(transform.position.x, Bunker.instance.firstLevelHeight, transform.position.z);
        characterController.enabled = true;
        OnFirstFloor = true;
    }

    public void SetOnSecondFloor() {
        characterController.enabled = false;
        transform.position = new Vector3(transform.position.x, Bunker.instance.secondLevelHeight, transform.position.z);
        characterController.enabled = true;
        OnFirstFloor = false;
    }

    public void GoFront() {
        characterController.enabled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, frontZPos);
        characterController.enabled = true;
    }

    public void GoBack() {
        characterController.enabled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, backZPos);
        characterController.enabled = true;
    }

    public void Translate(Vector3 distance) {
        characterController.enabled = false;
        transform.Translate(distance);
        characterController.enabled = true;
    }

    public void SetPosition(Vector3 pos) {
        characterController.enabled = false;
        transform.position = pos;
        characterController.enabled = true;
    }
}
