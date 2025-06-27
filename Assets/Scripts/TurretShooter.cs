using UnityEngine;

public class TurretShooter : MonoBehaviour, IDestroyable, IShooter
{
    public Transform baseTransform;         // Rotates horizontally
    public Transform barrelTransform;       // Rotates vertically
    public Transform firePoint;

    public float detectionRadius = 20f;
    public float fireInterval = 1f;
    public float aimSpeed = 5f;
    public float verticalClampAngle = 20f;
    public LayerMask playerMask;
    public LayerMask visionObstacles;       // e.g. walls, terrain

    private Transform player;
    private float fireTimer;
    private BulletPoolManager bulletPool;

    private int turretId;

    private void Start()
    {
        turretId = GetInstanceID();
        bulletPool = FindObjectOfType<BulletPoolManager>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (bulletPool == null)
            Debug.LogError("BulletPool not found in scene!");
        else
            bulletPool.RegisterShooter(this); // ✅ Register turret shooter

        if (player == null)
            Debug.LogError("Player not found in scene!");
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

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public int GetShooterId()
    {
        return turretId;
    }

    public void AssignBulletPool(BulletPoolManager manager)
    {
        bulletPool=manager;
    }
}
