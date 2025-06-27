using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooter
{
        int GetShooterId(); // usually returns GetInstanceID()
        void AssignBulletPool(BulletPoolManager manager);

}
