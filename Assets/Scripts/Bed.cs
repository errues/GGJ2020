﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable {
    private bool sleeping;

    protected override void InteractLoop() {
        if (sleeping) {
            if (Input.GetAxisRaw("Use") == 0) {
                sleeping = false;
                player.GetComponent<PlayerBars>().SetFillingEnergy(false);
            } else {
                player.Hammer();
            }
        } else if (Input.GetAxisRaw("Use") != 0) {
            sleeping = true;
            player.GetComponent<PlayerBars>().SetFillingEnergy(true);
            player.AutoRotate(true);
            player.Hammer();
        }
    }
}
