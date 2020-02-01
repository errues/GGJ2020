using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftEnergyButtons : MonoBehaviour {
    public SpriteRenderer[] firstButtons;
    public SpriteRenderer[] secondButtons;
    public SpriteRenderer[] thirdButtons;

    public void SetCode(int[] code) {
        for (int i=0; i < firstButtons.Length; i++) {
            firstButtons[i].gameObject.SetActive(i == code [0]);
            firstButtons[i].color = new Color(firstButtons[i].color.r, firstButtons[i].color.g, firstButtons[i].color.b, 1f);
        }

        for (int i = 0; i < secondButtons.Length; i++) {
            secondButtons[i].gameObject.SetActive(i == code[1]);
            secondButtons[i].color = new Color(secondButtons[i].color.r, secondButtons[i].color.g, secondButtons[i].color.b, 1f);
        }

        for (int i = 0; i < thirdButtons.Length; i++) {
            thirdButtons[i].gameObject.SetActive(i == code[2]);
            thirdButtons[i].color = new Color(thirdButtons[i].color.r, thirdButtons[i].color.g, thirdButtons[i].color.b, 1f);
        }
    }

    public void SetCodeProgress(int codeProgress) {
        if (codeProgress == 0) {
            for (int i = 0; i < firstButtons.Length; i++) {
                firstButtons[i].color = new Color(firstButtons[i].color.r, firstButtons[i].color.g, firstButtons[i].color.b, 1f);
            }

            for (int i = 0; i < secondButtons.Length; i++) {
                secondButtons[i].color = new Color(secondButtons[i].color.r, secondButtons[i].color.g, secondButtons[i].color.b, 1f);
            }
        } else if (codeProgress == 1) {
            for (int i = 0; i < firstButtons.Length; i++) {
                firstButtons[i].color = new Color(firstButtons[i].color.r, firstButtons[i].color.g, firstButtons[i].color.b, 0.4f);
            }

            for (int i = 0; i < secondButtons.Length; i++) {
                secondButtons[i].color = new Color(secondButtons[i].color.r, secondButtons[i].color.g, secondButtons[i].color.b, 1f);
            }
        } else if (codeProgress >= 2) {
            for (int i = 0; i < firstButtons.Length; i++) {
                firstButtons[i].color = new Color(firstButtons[i].color.r, firstButtons[i].color.g, firstButtons[i].color.b, 0.4f);
            }

            for (int i = 0; i < secondButtons.Length; i++) {
                secondButtons[i].color = new Color(secondButtons[i].color.r, secondButtons[i].color.g, secondButtons[i].color.b, 0.4f);
            }
        }
    }
}
