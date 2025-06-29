using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCameraFollow : MonoBehaviour
{
    public Transform target;                   // Assign your tank/player here
    public Vector3 offset = new Vector3(0f, 5f, -10f);     // Camera offset
    public Vector3 rotationOffset = new Vector3(20f, 0f, 0f); // Adjustable camera angle
    public float followSpeed = 10f;            // Smooth following speed

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        if (cam != null)
        {
            Camera.main.tag = "Untagged";
            cam.tag = "MainCamera";
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate desired position based on offset from target
        Vector3 desiredPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Base look at the target
        transform.LookAt(target);

        // Apply additional rotation offset
        transform.Rotate(rotationOffset);
    }
}
