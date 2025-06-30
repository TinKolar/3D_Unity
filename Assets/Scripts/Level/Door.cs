using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [Header("Movement Settings")]
    public float pullDistance = 3f;      // How far to move down
    public float pullSpeed = 1f;         // Units per second

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool isPulling = false;

    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        targetPos = startPos - new Vector3(0f, pullDistance, 0f);
    }
    public void OpenDoor()
    {
        if (!isPulling)
        {
            StartCoroutine(PullDown());
        }
    }

    private IEnumerator PullDown()
    {
        isPulling = true;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, pullSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos; // Snap to exact position at the end
        isPulling = false;
    }

}
