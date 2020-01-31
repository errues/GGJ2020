using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float movementSpeed;
    
    private CharacterController characterController;

    private float fallingSpeed;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }
    private void Update() {
        fallingSpeed += Constants.GRAVITY;
        characterController.Move(new Vector3(Input.GetAxis("Horizontal"), fallingSpeed, 0) * Time.deltaTime * movementSpeed);

        if (characterController.isGrounded) {
            fallingSpeed = Constants.GRAVITY;
        }
    }
}
