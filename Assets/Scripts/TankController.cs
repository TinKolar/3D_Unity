using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour , IDestroyable
{
    [Header("Movement Settings")]
    public float maxMoveSpeed = 5f;
    public float maxRotationSpeed = 100f;

    [Header("Acceleration Settings")]
    public float accelerationFactor = 5f;
    public float rotationAccelerationFactor = 2f;

    [Header("Deceleration Settings")]
    public float decelerationTime = 0.5f;

    [Header("Ground Check Settings")]
    public float slopeLimit = 30f;
    public float groundRayLength = 1.5f;
    public LayerMask groundMask;

    [Header("Jump Settings")]
    public float jumpForce = 7f;
    public float groundedCheckDistance = 0.2f;


    private float currentMoveSpeed = 0f;
    private float currentRotationSpeed = 0f;
    private bool isGrounded;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleJumpInput();
        CheckGrounded();
    }

    void HandleMovement()
    {
        float targetInput = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            targetInput = 1f;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            targetInput = -1f;

        float targetSpeed = targetInput * maxMoveSpeed;

        if (targetInput != 0)
        {
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, targetSpeed, accelerationFactor * Time.deltaTime);
        }
        else
        {
            float decelerationRate = maxMoveSpeed / decelerationTime;
            currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, 0f, decelerationRate * Time.deltaTime);
        }

        if (!IsSteepSlope(transform.forward * Mathf.Sign(currentMoveSpeed)))
        {
            transform.position += transform.forward * currentMoveSpeed * Time.deltaTime;
        }
    }

    void HandleRotation()
    {
        float targetInput = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            targetInput = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            targetInput = 1f;

        float targetRotationSpeed = targetInput * maxRotationSpeed;

        if (targetInput != 0)
        {
            currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, targetRotationSpeed, rotationAccelerationFactor * Time.deltaTime);
        }
        else
        {
            float decelerationRate = maxRotationSpeed / decelerationTime;
            currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, 0f, decelerationRate * Time.deltaTime);
        }

        transform.Rotate(0f, currentRotationSpeed * Time.deltaTime, 0f);
    }

    void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    void CheckGrounded()
    {
        // Raycast down to check if grounded
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        isGrounded = Physics.Raycast(ray, groundedCheckDistance + 0.1f, groundMask);
    }


    bool IsSteepSlope(Vector3 direction)
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        Vector3 rightOffset = transform.right * 0.4f;
        Vector3 leftOffset = -transform.right * 0.4f;

        Vector3[] rayOrigins = new Vector3[]
        {
        origin,
        origin + rightOffset,
        origin + leftOffset
        };

        foreach (Vector3 rayOrigin in rayOrigins)
        {
            Ray ray = new Ray(rayOrigin, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, groundRayLength, groundMask))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                if (angle > slopeLimit)
                    return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        Vector3 rightOffset = transform.right * 0.4f;
        Vector3 leftOffset = -transform.right * 0.4f;
        Vector3 direction = transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, direction * groundRayLength);
        Gizmos.DrawRay(origin + rightOffset, direction * groundRayLength);
        Gizmos.DrawRay(origin + leftOffset, direction * groundRayLength);
    }

    public void DestroyObject()
    {
        Debug.Log("Tank Destroyed");
        Destroy(gameObject);
    }
}
