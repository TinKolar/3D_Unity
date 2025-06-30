using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IDestroyable
{
    public float speed = 20f;
    public float lifeTime = 3f;
    public LayerMask collisionMask;

    private float lifeTimer;
    private Vector3 direction;
    private VFXManager vFXManager;


    private void Awake()
    {
        gameObject.SetActive(false);
        vFXManager = VFXManager.instance;
    }

    public void Fire(Vector3 startPosition, Vector3 newDirection)
    {
        ResetBullet();
        gameObject.SetActive(true);
        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(newDirection);
        transform.parent = null;
        vFXManager.PlaySound(vFXManager.bulletShot, transform);

        direction = newDirection.normalized;
        lifeTimer = lifeTime;

    }


    private void Update()
    {
        if (this.isActiveAndEnabled)
        {

            float moveDistance = speed * Time.deltaTime;
            lifeTimer -= Time.deltaTime;

            if (lifeTimer <= 0f)
            {
                Deactivate();
                return;
            }

            Ray ray = new Ray(transform.position, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, moveDistance + 0.5f, collisionMask))
            {
                if (hit.collider.TryGetComponent<IDestroyable>(out var destroyable))
                {

                    if (hit.collider.TryGetComponent<DancingEnemy>(out var dancingEnemy))
                    {
                        vFXManager.PlaySound(vFXManager.keyDeath, dancingEnemy.transform);
                    }
                    destroyable.DestroyObject();
                    Deactivate();
                    return;
                }

                direction = Vector3.Reflect(direction, hit.normal).normalized;
                vFXManager.PlaySound(vFXManager.bulletBounce, transform);

                transform.position = hit.point + direction * 0.02f;
            }
            else
            {
                transform.position += direction * moveDistance;
            }

            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<IDestroyable>(out var destroyable))
        {
            destroyable.DestroyObject();
            Deactivate();
        }
    }
    private void Deactivate()
    {
        vFXManager.MakeBoom(transform);
        gameObject.SetActive(false);
    }

    public void DestroyObject()
    {
        Deactivate();
    }

    public void ResetBullet()
    {
        direction = Vector3.zero;
        lifeTimer = 0f;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;
        float rayLength = speed * Time.deltaTime + 0.5f;
        Gizmos.DrawRay(transform.position, direction * rayLength);
    }

}
