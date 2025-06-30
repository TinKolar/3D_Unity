using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCameraFollow : MonoBehaviour
{
    public Transform target;                   
    public Vector3 offset = new Vector3(0f, 5f, -10f);     
    public Vector3 rotationOffset = new Vector3(20f, 0f, 0f); 
    public float followSpeed = 10f;            
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

        Vector3 desiredPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        transform.LookAt(target);

        transform.Rotate(rotationOffset);
    }
}
