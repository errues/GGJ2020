using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable {
    public float offsetInExtremes;
    public float inputThreshold;
    public float displacementOnEngage;

    protected override void InteractLoop() {
        if (player.InLadder) {
            if (player.transform.position.y < Bunker.instance.firstLevelHeight + offsetInExtremes) {
                player.SetOnFirstFloor();
                DisengageLadder();
            } else if (player.transform.position.y > Bunker.instance.secondLevelHeight - offsetInExtremes) {
                player.SetOnSecondFloor();
                DisengageLadder();
            }
        } else{
            if (player.OnFirstFloor && Input.GetAxisRaw("Vertical") > inputThreshold) {
                EngageLadder(true);
            } else if (!player.OnFirstFloor && Input.GetAxisRaw("Vertical") < inputThreshold) {
                EngageLadder(false);
            }
        }
    }

    private void DisengageLadder() {
        player.InLadder = false;
        player.GoFront();
    }

    private void EngageLadder(bool up) {
        player.GoBack();
        player.InLadder = true;
        Vector3 pos = player.transform.position;
        pos.x = transform.position.x;
        pos.y += displacementOnEngage * (up ? 1 : -1);
        player.SetPosition(pos);
    }
}
