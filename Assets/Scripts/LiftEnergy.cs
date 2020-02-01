using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftEnergy : Interactable {
    [Header("Lift Energy")]
    public int codeLenght = 3;

    private bool energyOn;
    private int[] activationCode;
    private int pressedCode;

    private LiftEnergyButtons liftEnergyButtons;
    private bool buttonUpPressed;
    private bool buttonDownPressed;

    private void Awake() {
        energyOn = true;
        liftEnergyButtons = interactionIndicator.GetComponent<LiftEnergyButtons>();
    }

    public void PowerOff() {
        energyOn = false;
        pressedCode = 0;
        GenerateCode();
    }

    private void PowerOn() {
        energyOn = true;
        HideInteraction();
    }

    private void GenerateCode() {
        activationCode = new int[codeLenght];
        for (int i = 0; i < codeLenght; i++) {
            activationCode[i] = Random.Range(0, 3);
        }
        if (liftEnergyButtons) {
            liftEnergyButtons.SetCode(activationCode);
        }
    }

    protected override void InteractLoop() {
        if (!energyOn) {
            if (!ShowingInteraction) {
                ShowInteraction();
            }

            if (buttonUpPressed && Input.GetAxisRaw("Vertical") < 0.9f) {
                buttonUpPressed = false;
            }
            if (buttonDownPressed && Input.GetAxisRaw("Vertical") > -0.9f) {
                buttonDownPressed = false;
            }

            bool pushedActionButton = Input.GetButtonDown("Use");
            bool pushedUpButton = Input.GetAxisRaw("Vertical") >= 0.9f && !buttonUpPressed;
            bool pushedDownButton = Input.GetAxisRaw("Vertical") <= -0.9f && !buttonDownPressed;

            if (pushedUpButton) {
                buttonUpPressed = true;
            }
            if (pushedDownButton) {
                buttonDownPressed = true;
            }

            if (pushedActionButton || pushedUpButton || pushedDownButton) {
                if (activationCode[pressedCode] == 0 && pushedActionButton && !pushedUpButton && !pushedDownButton ||
                    activationCode[pressedCode] == 1 && !pushedActionButton && pushedUpButton && !pushedDownButton ||
                    activationCode[pressedCode] == 2 && !pushedActionButton && !pushedUpButton && pushedDownButton) {

                    pressedCode++;
                    liftEnergyButtons.SetCodeProgress(pressedCode);
                } else {
                    pressedCode = 0;
                    liftEnergyButtons.SetCodeProgress(pressedCode);
                }
            }

            if (pressedCode == activationCode.Length) {
                PowerOn();
            }
        }
    }

    protected override void ShowInteraction() {
        if (!energyOn) {
            pressedCode = 0;
            ShowingInteraction = true;
            interactionIndicator.SetActive(true);
        }
    }
}
