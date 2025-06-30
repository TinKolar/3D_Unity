using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DancingEnemy : MonoBehaviour
{
    VFXManager vfxManager;

    private void Start()
    {
        vfxManager=VFXManager.instance;
    }
    private void OnDestroy()
    {
        vfxManager.PlaySound(vfxManager.keyDeath, transform);
    }
}
