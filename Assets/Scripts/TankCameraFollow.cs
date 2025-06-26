using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCameraFollow : MonoBehaviour
{
    public Transform target;             // Assign your tank/player here
    public Vector3 offset = new Vector3(0f, 5f, -10f);  // Camera position relative to tank
    public float followSpeed = 10f;      // Smooth following speed

    void LateUpdate()
    {
        if (target == null) return;

        // Desired camera position relative to the target’s rotation
        Vector3 desiredPosition = target.TransformPoint(offset);

        // Smoothly move the camera to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Always look at the target
        transform.LookAt(target);
    }
}
