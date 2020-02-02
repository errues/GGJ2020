using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBars : MonoBehaviour {
    public Vector2 hungerSpeedLimits;
    public Vector2 energySpeedLimits;

    public float fillingSpeed;

    private CanvasController canvasController;

    private float hungerSpeed;
    private float energySpeed;

    private float currentEnergy;
    private float currentHunger;

    private bool fillingHunger;
    private bool fillingEnergy;

    private void Awake() {
        canvasController = CanvasController.instance;

        currentEnergy = 100;
        currentHunger = 100;
    }

    private void Start() {
        NewHungerSpeed();
        NewEnergySpeed();
    }

    private void Update() {
        currentEnergy = Mathf.Clamp(currentEnergy + (fillingEnergy ? fillingSpeed : -energySpeed) * Time.deltaTime, 0, 100);
        currentHunger = Mathf.Clamp(currentHunger + (fillingHunger ? fillingSpeed : -hungerSpeed) * Time.deltaTime, 0, 100);

        canvasController.SetBarsValues(currentHunger / 100, currentEnergy / 100);

        if (currentEnergy == 0) {
            GameController.instance.FinishGame(CauseOfDeath.ENERGY);
        } else if (currentHunger == 0) {
            GameController.instance.FinishGame(CauseOfDeath.HUNGER);
        }
    }

    private void NewHungerSpeed() {
        hungerSpeed = Random.Range(hungerSpeedLimits.x, hungerSpeedLimits.y);
    }

    private void NewEnergySpeed() {
        energySpeed = Random.Range(energySpeedLimits.x, energySpeedLimits.y);
    }

    public void SetFillingEnergy(bool filling) {
        fillingEnergy = filling;
    }

    public void SetFillingHunger(bool filling) {
        fillingHunger = filling;
    }
}
