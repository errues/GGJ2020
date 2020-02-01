using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private class AutoMovement {
        public Vector3 targetPosition;
        public float time;

        public AutoMovement(Vector3 p, float t) {
            targetPosition = p;
            time = t;
        }
    }

    public float movementSpeed;
    public float inLadderMovementSpeed;

    public float frontZPos = 0;
    public float backZPos = 1;

    private Queue<AutoMovement> autoMovements;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float initialTime;
    private float targetTime;
    private float autoMoveAlpha;

    public bool OnFirstFloor { get; set; }
    public bool InLadder { get; set; }
    public bool AutoMoving { get; set; }
    
    private CharacterController characterController;

    private float fallingSpeed;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        OnFirstFloor = transform.position.y < Bunker.instance.secondLevelHeight - 1;
        InLadder = false;

        autoMovements = new Queue<AutoMovement>();
    }

    private void Update() {
        if (AutoMoving) {
            autoMoveAlpha = Mathf.Clamp((Time.time - initialTime) / targetTime, 0, 1);
            transform.position = Vector3.Lerp(initialPosition, targetPosition, AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(autoMoveAlpha));

            if (autoMoveAlpha == 1) {                
                if (autoMovements.Count > 0) {
                    NextAutoMovement();
                } else {
                    AutoMoving = false;
                    characterController.enabled = true;
                }
            }
        } else {
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

    public void AddAutoMovement(Vector3 targetPosition, float time) {
        autoMovements.Enqueue(new AutoMovement(targetPosition, time));
    }

    public void StartAutomovement() {
        if (autoMovements.Count > 0) {
            characterController.enabled = false;
            NextAutoMovement();
        }
    }

    public void NextAutoMovement() {
        AutoMovement am = autoMovements.Dequeue();
        initialPosition = transform.position;
        targetPosition = am.targetPosition;
        initialTime = Time.time;
        targetTime = am.time;

        autoMoveAlpha = 0;
        AutoMoving = true;
    }
}
