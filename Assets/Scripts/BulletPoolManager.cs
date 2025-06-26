using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletsPerTank = 6;

    private Dictionary<int, List<Bullet>> tankBulletPool = new();

    private void Start()
    {
        TankShooter[] tanks = FindObjectsOfType<TankShooter>();

        foreach (TankShooter tank in tanks)
        {
            int tankId = tank.GetInstanceID();
            tankBulletPool[tankId] = new List<Bullet>();

            for (int i = 0; i < bulletsPerTank; i++)
            {
                GameObject bulletObj = Instantiate(bulletPrefab);
                bulletObj.SetActive(false);

                Bullet bullet = bulletObj.GetComponent<Bullet>();
                tankBulletPool[tankId].Add(bullet);
            }

            tank.AssignBulletPool(this);  // Let the tank register
        }
    }

    public bool TryFire(int tankId, Vector3 position, Vector3 direction)
    {
        if (!tankBulletPool.ContainsKey(tankId)) return false;

        foreach (Bullet bullet in tankBulletPool[tankId])
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.Fire(position, direction);
                return true;
            }
        }

        return false; // No available bullet
    }
}
