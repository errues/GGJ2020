using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBar : MonoBehaviour {
    public SpriteRenderer backgroundCircle;
    public SpriteRenderer circleFill;
    public SpriteMask circleProgress;
    public Transform arrow;

    public void SetState(Color backgroundColor, Color fillColor) {
        backgroundCircle.color = backgroundColor;
        circleFill.color = fillColor;

        arrow.rotation = Quaternion.Euler(arrow.rotation.eulerAngles.x, arrow.rotation.eulerAngles.y , 0f);
        circleProgress.alphaCutoff = 0f;
    }


    public void SetAlpha(float alpha) {
        arrow.rotation = Quaternion.Euler(arrow.rotation.eulerAngles.x, arrow.rotation.eulerAngles.y, Mathf.Lerp(0, -360, alpha));
        circleProgress.alphaCutoff = alpha;
    }
}
