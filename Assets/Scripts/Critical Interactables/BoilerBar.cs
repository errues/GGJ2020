using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilerBar : MonoBehaviour {
    public Transform barFill;
    public Transform arrow;

    public float maxAngle;

    private float fillWidth = 1.3f;

    public void SetArrowPosition(float alpha) {
        arrow.localRotation = Quaternion.Euler(arrow.localRotation.eulerAngles.x, arrow.localRotation.eulerAngles.y, Mathf.Lerp(0, maxAngle, alpha));
    }

    public void ShowIndicator(float barFillPosition, float barFillSize) {
        barFill.localRotation = Quaternion.Euler(barFill.localRotation.eulerAngles.x, barFill.localRotation.eulerAngles.y, Mathf.Lerp(0, maxAngle, barFillPosition));
        barFill.localScale = new Vector3(Mathf.Abs(maxAngle * barFillSize) / fillWidth, barFill.localScale.y, barFill.localScale.z);
        gameObject.SetActive(true);
    }
}
