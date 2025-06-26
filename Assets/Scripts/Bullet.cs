using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IDestroyable
{
    public float speed = 20f;
    public float lifeTime = 3f;

    private float lifeTimer;
    private Vector3 direction;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>(true);

        if (rb == null)
        {
            Debug.LogError("No Rigidbody found in Bullet or its children!");
        }

        gameObject.SetActive(false);
    }
    //void FixedUpdate()
    //{
    //    rb.velocity = rb.velocity.normalized * speed;
    //}

    public void Fire(Vector3 startPosition, Vector3 direction)
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(direction);

        transform.parent = null; // <-- make sure it's not a child of anything

        this.direction = direction.normalized;
        rb.velocity = this.direction * speed;

        lifeTimer = lifeTime;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Deactivate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var hitObj = collision.gameObject;

        // Destroyable target
        if (hitObj.TryGetComponent<IDestroyable>(out var destroyable))
        {
            destroyable.DestroyObject();
            Deactivate();
            return;
        }

        // Get collision contact info
        ContactPoint contact = collision.contacts[0];
        Vector3 incomingVelocity = rb.velocity;

        // Reflect the velocity based on surface normal
        Vector3 reflectedVelocity = Vector3.Reflect(incomingVelocity.normalized, contact.normal).normalized;

        // Prevent perfect upward bounces (e.g., from flat ceilings)
        if (Vector3.Angle(reflectedVelocity, Vector3.up) < 5f)
        {
            reflectedVelocity += Vector3.down * 0.1f; // push down slightly
            reflectedVelocity.Normalize();
        }

        // Apply new direction and speed
        direction = reflectedVelocity;
        rb.velocity = direction * speed;

        // Slightly offset the position to avoid sticking
        transform.position += contact.normal * 0.01f;
    }


    private void Deactivate()
    {
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void DestroyObject()
    {
        Deactivate() ;
    }
}
