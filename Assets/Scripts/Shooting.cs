using System.Linq;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Refs")]
    public GameObject bulletPrefab;      // assign BulletLite_02
    public Transform muzzleTransform;    // assign Muzzle
    public Transform yawSource;          // assign Player (for direction)

    [Header("Ballistics")]
    public float spawnOffset = 0.6f;
    public float bulletSpeed = 2f;
    public float bulletLifetime = 5f;

    [Header("Auto-fire")]
    public bool autoFireEnabled = true;
    public float fireRate = 0.15f;   // seconds between shots
    public KeyCode toggleKey = KeyCode.Space;

    [Header("Position Lock")]
    public float chestHeightOffset = 1.2f;

    private float fireCountdown = 0f; // countdown timer
    private Collider[] shooterCols;

    void Awake()
    {
        shooterCols = GetComponentsInParent<Collider>(true)
            .Concat(GetComponents<Collider>()).ToArray();
    }

    void Update()
    {
        // toggle auto fire
        if (Input.GetKeyDown(toggleKey))
            autoFireEnabled = !autoFireEnabled;

        // decrease countdown every frame
        if (fireCountdown > 0f)
            fireCountdown -= Time.deltaTime;

        // fire when countdown hits 0
        if (autoFireEnabled && fireCountdown <= 0f)
        {
            Fire();
            fireCountdown = fireRate; // reset countdown
        }

        // debug direction line
        if (muzzleTransform)
        {
            Vector3 flatPos = muzzleTransform.position;
            flatPos.y = yawSource.position.y + chestHeightOffset;
            Debug.DrawRay(flatPos, yawSource.forward * 2f, Color.cyan);
        }
    }

    void Fire()
    {
        if (!bulletPrefab || !muzzleTransform || !yawSource) return;

        // Flat forward direction from player
        Vector3 dir = yawSource.forward; dir.y = 0f; dir.Normalize();

        // Chest-level spawn position
        Vector3 spawnPos = muzzleTransform.position;
        spawnPos.y = yawSource.position.y + chestHeightOffset;
        spawnPos += dir * spawnOffset;

        // Rotation (with your 90° fix)
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up) ;

        GameObject b = Instantiate(bulletPrefab, spawnPos, rot);
        b.transform.SetParent(null, true); // be 100% sure it's not parented

        // Remove trail if any
        var trail = b.GetComponent<TrailRenderer>();
        if (trail) Destroy(trail);

        // --------- Ensure Rigidbody exists and is usable ----------
        // find RB even if it's on a child
        Rigidbody rb = b.GetComponent<Rigidbody>();
        if (!rb) rb = b.GetComponentInChildren<Rigidbody>();

        if (!rb)
        {
            // add one if prefab didn't have it
            rb = b.AddComponent<Rigidbody>();
        }
        // make sure physics won't block motion
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.drag = 0f;
        rb.angularDrag = 0f;
        rb.constraints = RigidbodyConstraints.None;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Give it velocity (VelocityChange ignores mass)
        rb.velocity = Vector3.zero; // reset any leftover
        rb.AddForce(dir * bulletSpeed, ForceMode.VelocityChange);

        // --------- Ignore collisions with the shooter ----------
        var bulletCol = b.GetComponent<Collider>();
        if (!bulletCol) bulletCol = b.GetComponentInChildren<Collider>();
        if (bulletCol)
        {
            foreach (var c in shooterCols)
                if (c) Physics.IgnoreCollision(bulletCol, c, true);
        }

        Destroy(b, bulletLifetime);
    }
}
