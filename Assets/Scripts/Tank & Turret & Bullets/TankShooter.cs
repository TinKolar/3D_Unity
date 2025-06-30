using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : MonoBehaviour, IShooter
{
    public Transform firePoint;
    public float fireInterval = 0.5f;

    private BulletPoolManager bulletPool;
    private int tankId;
    private float fireCooldown = 0;


    private void Start()
    {
        tankId = GetInstanceID();

        if (bulletPool == null)
        {
            bulletPool = FindObjectOfType<BulletPoolManager>();
        }

        if (bulletPool != null)
        {
            bulletPool.RegisterShooter(this); 
        }
    }

    public void AssignBulletPool(BulletPoolManager manager)
    {
        bulletPool = manager;
    }

    private void Update()
    {
        fireCooldown-= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && bulletPool != null && fireCooldown<=0f)
        {
            bulletPool.TryFire(tankId, firePoint.position, firePoint.forward);
            fireCooldown=fireInterval;
        }
    }

    public int GetShooterId()
    {
        return tankId;
    }
}
