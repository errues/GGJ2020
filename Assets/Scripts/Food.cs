using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Interactable {
    private bool eating;

    protected override void InteractLoop() {
        if (eating) {
            if (Input.GetAxisRaw("Use") == 0) {
                eating = false;
                player.GetComponent<PlayerBars>().SetFillingHunger(false);
            } else {
                player.Hammer();
            }
        } else if (Input.GetAxisRaw("Use") != 0) {
            eating = true;
            player.GetComponent<PlayerBars>().SetFillingHunger(true);
            player.AutoRotate(true);
            player.Hammer();
        }
    }
}
