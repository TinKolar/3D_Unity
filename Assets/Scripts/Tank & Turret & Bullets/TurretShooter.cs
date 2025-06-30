using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShooter : MonoBehaviour, IDestroyable, IShooter
{
    [Header("Turret Settings")]
    public Transform baseTransform;         // Rotates horizontally
    public Transform barrelTransform;       // Rotates vertically
    public Transform firePoint;

    public float detectionRadius = 20f;
    public float fireInterval = 1f;
    public float aimSpeed = 5f;
    public float verticalClampAngle = 20f;
    public LayerMask playerMask;
    public LayerMask visionObstacles;       // e.g. walls, terrain


    [Header("Shader Settings")]
    public float dissolveDuration = 1f;            // Duration of dissolve
    public float dissolveEndValue = 0.8f;          // Final dissolve value

    private string dissolveProperty = "_DissolveEffect";  // Name of the shader property
    private List<Material> allMaterials = new List<Material>();

    private Transform player;
    private float fireTimer;
    private BulletPoolManager bulletPool;

    VFXManager vFXManager;

    private int turretId;

    private void Start()
    {
        turretId = GetInstanceID();
        bulletPool = FindObjectOfType<BulletPoolManager>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            // This ensures each renderer gets its own instance of the material
            allMaterials.Add(rend.material);
        }

        if (bulletPool == null)
            Debug.LogError("BulletPool not found in scene!");
        else
            bulletPool.RegisterShooter(this); 

        if (player == null)
            Debug.LogError("Player not found in scene!");

        vFXManager = VFXManager.instance;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > detectionRadius)
        {
            //Debug.Log("Player is to far away");
            return;
        }
        Vector3 targetDir = player.position - firePoint.position;
        if (!CanSeePlayer(targetDir)) return;

        AimAt(targetDir);

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Fire();
            fireTimer = fireInterval;
        }
    }

    private bool CanSeePlayer(Vector3 directionToPlayer)
    {
        Vector3 origin = firePoint.position;
        Vector3 direction = directionToPlayer.normalized;

        Debug.DrawRay(origin, direction * detectionRadius, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, detectionRadius, visionObstacles))
        {
            //Debug.Log("Raycast hit: " + hit.collider.name);
            return hit.collider.CompareTag("Player");
        }

        //Debug.Log("Raycast didn't hit anything");
        return false;
    }

    private void AimAt(Vector3 worldDirection)
    {
        // Horizontal rotation (Y axis)
        Vector3 flatDir = new Vector3(worldDirection.x, 0, worldDirection.z);
        Quaternion targetBaseRot = Quaternion.LookRotation(flatDir);
        baseTransform.rotation = Quaternion.Slerp(baseTransform.rotation, targetBaseRot, Time.deltaTime * aimSpeed);


        // Vertical rotation (X axis)
        Quaternion targetBarrelRot = Quaternion.LookRotation(worldDirection);

        float offset=0;
        if(transform.position.y - player.transform.position.y>0.25)
        offset= 10f;

        float xAngle = targetBarrelRot.eulerAngles.x;
        if (targetBarrelRot.eulerAngles.x >= 35)
            return;
        xAngle = NormalizeAngle(xAngle);
        xAngle = Mathf.Clamp(xAngle, -verticalClampAngle, verticalClampAngle) + offset;
        //Debug.Log($"Angle X: {xAngle}");
        //Debug.Log($"Target Angle: {targetBarrelRot.eulerAngles.x}");
        //Debug.Log($"offset Angle: {offset}");


        Vector3 barrelEuler = new Vector3(xAngle, barrelTransform.localEulerAngles.y, barrelTransform.localEulerAngles.z);
        barrelTransform.localEulerAngles = Vector3.Lerp(barrelTransform.localEulerAngles, barrelEuler, Time.deltaTime * aimSpeed);
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle > 180 ? angle - 360 : angle;
        return angle;
    }

    private void Fire()
    {
        Vector3 dir = firePoint.forward;
        bulletPool.TryFire(turretId,firePoint.position, dir);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }


    public int GetShooterId()
    {
        return turretId;
    }

    public void AssignBulletPool(BulletPoolManager manager)
    {
        bulletPool = manager;
    }

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
        vFXManager.PlaySound(vFXManager.turretDeath, transform);
        StartCoroutine(DissolveAndDestroy());
    }

}
