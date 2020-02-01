using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public float speed;

    public Vector2 minLimits;
    public Vector2 maxLimits;

    private Vector3 offset;

    private void Awake() {
        offset = transform.position - target.position;
    }

    private void Update() {
        Vector3 targetPosition = Vector3.Lerp(transform.position, new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z), Time.deltaTime * speed);
        targetPosition.x = Mathf.Clamp(targetPosition.x, minLimits.x, maxLimits.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minLimits.y, maxLimits.y);

        transform.position = targetPosition;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3((minLimits.x + maxLimits.x) / 2, (minLimits.y + maxLimits.y) / 2, 0), new Vector3(maxLimits.x - minLimits.x, maxLimits.y - minLimits.y, 5));
    }
}
