using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletsPerShooter = 6;

    private Dictionary<int, List<Bullet>> shooterBulletPool = new();

    private void Start()
    {
        // Find all shooters currently in the scene
        IShooter[] shooters = FindObjectsOfType<MonoBehaviour>().OfType<IShooter>().ToArray();
        foreach (IShooter shooter in shooters)
        {
            RegisterShooter(shooter);
        }
    }

    public void RegisterShooter(IShooter shooter)
    {
        int id = shooter.GetShooterId();
        if (shooterBulletPool.ContainsKey(id))
        {
            //Debug.LogWarning($"Shooter with ID {id} is already registered.");
            return;
        }

        //Debug.Log($"Registering shooter with ID: {id}");

        var bulletList = new List<Bullet>();

        for (int i = 0; i < bulletsPerShooter; i++)
        {
            GameObject bulletObj = Instantiate(bulletPrefab);
            bulletObj.SetActive(false);
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bulletList.Add(bullet);
        }

        shooterBulletPool[id] = bulletList;
        shooter.AssignBulletPool(this);
    }

    public bool TryFire(int shooterId, Vector3 position, Vector3 direction)
    {
        //Debug.Log($"TryFire called by shooter ID: {shooterId}");

        if (!shooterBulletPool.TryGetValue(shooterId, out List<Bullet> bullets))
        {
            //Debug.LogWarning($"Shooter ID {shooterId} not found in bullet pool.");
            return false;
        }

        foreach (Bullet bullet in bullets)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.Fire(position, direction);
                return true;
            }
        }

        //Debug.Log($"No available bullets for shooter ID {shooterId}.");
        return false;
    }
}
