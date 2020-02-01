using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public float speed;

    private Vector3 offset;

    private void Awake() {
        offset = transform.position - target.position;
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z), Time.deltaTime * speed);
    }
}
