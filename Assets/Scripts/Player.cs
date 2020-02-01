using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private class AutoMovement {
        public Vector3 targetPosition;
        public float time;
        public bool wait;
        public int rotate; // 0 = No rotation, 1 = Front, 2 = Back

        public AutoMovement(Vector3 p, float t, bool w, int r) {
            targetPosition = p;
            time = t;
            wait = w;
            rotate = r;
        }
    }

    public float movementSpeed;
    public float rotationSpeed;
    public float inLadderMovementSpeed;
    public float holdingHoseMovementSpeed;

    public float frontZPos = 0;
    public float backZPos = 1;

    private Queue<AutoMovement> autoMovements;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float initialAutoMovementTime;
    private float targetAutoMovementTime;
    private float autoMovementAlpha;

    private bool autoRotating;
    private Quaternion targetRotation;
    private Quaternion initialRotation;
    private float autoRotationAlpha;

    public bool OnFirstFloor { get; set; }
    public bool InLadder { get; set; }
    public bool AutoMoving { get; set; }
    public bool HoldingHose { get; set; }

    private CharacterController characterController;
    private Animator animator;

    private float fallingSpeed;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        OnFirstFloor = transform.position.y < Bunker.instance.secondLevelHeight - 1;
        InLadder = false;

        autoMovements = new Queue<AutoMovement>();
    }

    private void Update() {
        if (AutoMoving) {
            autoMovementAlpha = Mathf.Clamp((Time.time - initialAutoMovementTime) / targetAutoMovementTime, 0, 1);
            transform.position = Vector3.Lerp(initialPosition, targetPosition, AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(autoMovementAlpha));

            if (autoMovementAlpha == 1) {                
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
                animator.speed = Input.GetAxisRaw("Vertical") != 0 ? 1 : 0;
            } else if (NotHammering()) {
                fallingSpeed += Constants.GRAVITY;
                characterController.Move(new Vector3(Input.GetAxisRaw("Horizontal"), fallingSpeed, 0) * Time.deltaTime * (HoldingHose ? holdingHoseMovementSpeed : movementSpeed));

                animator.SetBool("running", Input.GetAxisRaw("Horizontal") != 0);
        
                if (characterController.isGrounded) {
                    fallingSpeed = Constants.GRAVITY;
                }
            }
        }

        if (autoRotating) {
            autoRotationAlpha = Mathf.Clamp(autoRotationAlpha + Time.deltaTime * rotationSpeed, 0, 1);

            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, autoRotationAlpha);

            if (autoRotationAlpha == 1) {
                autoRotating = false;
            }
        } else if (!AutoMoving && !InLadder && NotHammering()) {
            if (Input.GetAxis("Horizontal") > 0 || HoldingHose) {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            } else if (Input.GetAxis("Horizontal") < 0) {
                transform.rotation = Quaternion.Euler(0, 180, 0);
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

    public void AddAutoMovement(Vector3 targetPosition, float time, bool wait, int rotate) {
        autoMovements.Enqueue(new AutoMovement(targetPosition, time, wait, rotate));
    }

    public void StartAutomovement() {
        if (autoMovements.Count > 0) {
            characterController.enabled = false;
            NextAutoMovement();
        }
    }

    public void EngageLadder() {
        transform.rotation = Quaternion.Euler(0, 90, 0);
        animator.SetBool("climbing", true);
    }

    public void DisengageLadder() {
        animator.SetBool("climbing", false);
    }

    public void NextAutoMovement() {
        AutoMovement am = autoMovements.Dequeue();
        initialPosition = transform.position;
        targetPosition = am.targetPosition;
        initialAutoMovementTime = Time.time;
        targetAutoMovementTime = am.time;

        animator.SetBool("running", !am.wait);

        if (am.rotate != 0) {
            AutoRotate(am.rotate == 1);
        }

        autoMovementAlpha = 0;
        AutoMoving = true;
    }

    public void AutoRotate(bool front) {
        autoRotating = true;
        targetRotation = Quaternion.Euler(0, 90 * (front ? -1 : 1), 0);
        initialRotation = transform.rotation;
        autoRotationAlpha = 0;
    }

    public void StartHammering() {
        animator.SetTrigger("hammer");
    }

    public void StopHammering() {
        animator.SetTrigger("putDownHammer");
    }

    private bool NotHammering() {
        if (animator.IsInTransition(0)) {
            return !animator.GetNextAnimatorStateInfo(0).IsName("GrabHammer") &&
            !animator.GetNextAnimatorStateInfo(0).IsName("Hammer") &&
            !animator.GetNextAnimatorStateInfo(0).IsName("PutDownHammer");
        }

        return !animator.GetCurrentAnimatorStateInfo(0).IsName("GrabHammer") && 
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Hammer") && 
            !animator.GetCurrentAnimatorStateInfo(0).IsName("PutDownHammer");
    }

    public void Hammer() {
        animator.SetTrigger("hammer");
    }

    public void HosePose(bool grabbing) {
        animator.SetBool("grabbingHose", grabbing);
    }
}
