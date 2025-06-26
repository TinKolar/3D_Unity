using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : MonoBehaviour
{
    public Transform firePoint;

    private BulletPoolManager bulletPool;
    private int tankId;

    private void Start()
    {
        tankId = GetInstanceID();

        // Optional fallback if BulletPoolManager missed it
        if (bulletPool == null)
        {
            bulletPool = FindObjectOfType<BulletPoolManager>();
        }
    }

    public void AssignBulletPool(BulletPoolManager manager)
    {
        bulletPool = manager;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && bulletPool != null)
        {
            bulletPool.TryFire(tankId, firePoint.position, firePoint.forward);
        }
    }
}
