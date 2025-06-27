using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletCount = 6;

    private Bullet[] bullets;

    private int activeTanks = 0;

    private void Awake()
    {
        bullets = new Bullet[bulletCount];

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bulletObj = Instantiate(bulletPrefab);
            bulletObj.SetActive(false);
            bullets[i] = bulletObj.GetComponent<Bullet>();
        }
    }

    private void Start()
    {
        
    }

    public bool TryFire(Vector3 position, Vector3 direction)
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].gameObject.activeSelf)
            {
                bullets[i].enabled=true;
                bullets[i].Fire(position, direction);
                return true;
            }
        }
        return false; // All bullets in use
    }
}
