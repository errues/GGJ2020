using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : MonoBehaviour {
    public static Bunker instance;

    public float firstLevelHeight;
    public float secondLevelHeight;

    private void Awake() {
        instance = this;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(new Vector3(0, firstLevelHeight, 0), new Vector3(1, .01f, 1));
        Gizmos.DrawCube(new Vector3(0, secondLevelHeight, 0), new Vector3(1, .01f, 1));
    }
}
