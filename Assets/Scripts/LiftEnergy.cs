using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftEnergy : Interactable {
    [Header("Lift Energy")]
    public CriticalInteractable.CriticalSpeed[] timesNextBlackout = new CriticalInteractable.CriticalSpeed[3];
    public int codeLenght = 3;

    [Header("References")]
    public Elevator elevator;
    public List<GameObject> lights;

    [Header("Sounds")]
    public AudioClip powerOnClip;
    public AudioClip powerOffClip;

    private bool energyOn;
    private int[] activationCode;
    private int pressedCode;

    private LiftEnergyButtons liftEnergyButtons;
    private bool buttonUpPressed;
    private bool buttonDownPressed;

    private float initialTime;
    private int currentTimeIndex;

    private AudioSource audioSource;

    private void Awake() {
        energyOn = true;
        liftEnergyButtons = interactionIndicator.GetComponent<LiftEnergyButtons>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        initialTime = Time.time;
        currentTimeIndex = 0;

        StartCoroutine(WaitToNextBlackout());
    }

    private IEnumerator WaitToNextBlackout() {
        Vector2 time;
        if (currentTimeIndex == timesNextBlackout.Length - 1) {
            time = new Vector2(timesNextBlackout[currentTimeIndex].criticalSpeed.x, timesNextBlackout[currentTimeIndex].criticalSpeed.y);
        } else {
            time = new Vector2(
                Mathf.Lerp(timesNextBlackout[currentTimeIndex].criticalSpeed.x, timesNextBlackout[currentTimeIndex + 1].criticalSpeed.x, (timesNextBlackout[currentTimeIndex].time - (Time.time - initialTime)) / timesNextBlackout[currentTimeIndex].time),
                Mathf.Lerp(timesNextBlackout[currentTimeIndex].criticalSpeed.y, timesNextBlackout[currentTimeIndex + 1].criticalSpeed.y, (timesNextBlackout[currentTimeIndex].time - (Time.time - initialTime)) / timesNextBlackout[currentTimeIndex].time)
                );

            if (Time.time - initialTime > timesNextBlackout[currentTimeIndex].time) {
                currentTimeIndex++;
            }
        }

        yield return new WaitForSeconds(Random.Range(time.x, time.y));
        PowerOff();
    }

    public void PowerOff() {
        energyOn = false;
        elevator.Deactivate();
        foreach(GameObject go in lights) {
            go.SetActive(false);
        }
        pressedCode = 0;
        GenerateCode();
        audioSource.PlayOneShot(powerOffClip);
    }

    private void PowerOn() {
        energyOn = true;
        elevator.Activate();
        foreach (GameObject go in lights) {
            go.SetActive(true);
        }
        HideInteraction();
        WaitToNextBlackout();
        audioSource.PlayOneShot(powerOnClip);
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
                player.AutoRotate(true);
                player.Hammer();
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
