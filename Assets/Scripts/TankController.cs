using Cinemachine.Utility;
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


    [Header("Shader Settings")]
    public float dissolveDuration = 1f;            // Duration of dissolve
    public float dissolveEndValue = 0.8f;          // Final dissolve value

    private string dissolveProperty = "_DissolveEffect";  // Name of the shader property
    private List<Material> allMaterials = new List<Material>();

    private float currentMoveSpeed = 0f;
    private float currentRotationSpeed = 0f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            // This ensures each renderer gets its own instance of the material
            allMaterials.Add(rend.material);
        }
    }
    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleJumpInput();
    }
    #region MOVEMENT
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
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up*jumpForce,ForceMode.Impulse);
        }
        if (rb.velocity.y < -20f)
        {
            rb.velocity = new Vector3(rb.velocity.x, -20f, rb.velocity.z);
        }
    }

    bool IsGrounded()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundedCheckDistance, groundMask);
        Debug.Log(isGrounded);
        return isGrounded;
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
    #endregion

    #region ShaderLogic

    private IEnumerator DissolveAndDestroy()
    {
        float elapsed = 0f;
        float startValue = 0f;

        while (elapsed < dissolveDuration)
        {
            float t = elapsed / dissolveDuration;
            float currentValue = Mathf.Lerp(startValue, dissolveEndValue, t);

            foreach (var mat in allMaterials)
            {
                mat.SetFloat(dissolveProperty, currentValue);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach (var mat in allMaterials)
        {
            mat.SetFloat(dissolveProperty, dissolveEndValue);
        }

        Destroy(gameObject);
    }


    #endregion

    public void DestroyObject()
    {

        Camera cam = GetComponentInChildren<Camera>();

        if (cam != null)
        {
            cam.transform.parent = null; // Unparent it so it’s not destroyed with the tank
        }

        maxMoveSpeed = 0;
        maxRotationSpeed = 0;
        jumpForce = 0;
        rb.velocity = Vector3.zero;
        StartCoroutine(DissolveAndDestroy());
        //Debug.Log("Tank Destroyed");
        //Destroy(gameObject);
    }
}
