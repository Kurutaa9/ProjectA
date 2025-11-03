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

        // 1️⃣ Get flat forward direction
        Vector3 dir = yawSource.forward;
        dir.y = 0f;
        dir.Normalize();

        // 2️⃣ Compute chest-level spawn position
        Vector3 spawnPos = muzzleTransform.position;
        spawnPos.y = yawSource.position.y + chestHeightOffset;
        spawnPos += dir * spawnOffset;

        // 3️⃣ Instantiate bullet
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up) * Quaternion.Euler(0f, 90f, 0f);
        GameObject b = Instantiate(bulletPrefab, spawnPos, rot);

        // 4️⃣ Ensure Rigidbody exists and is not kinematic
        Rigidbody rb = b.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = false;      // ✅ important — must be false
            rb.useGravity = false;       // no falling
            rb.velocity = dir * bulletSpeed; // ✅ move forward
        }
        else
        {
            Debug.LogWarning("Bullet prefab has no Rigidbody component!");
        }

        // 5️⃣ Ignore shooter collisions
        Collider bulletCol = b.GetComponent<Collider>();
        if (bulletCol)
        {
            foreach (var c in shooterCols)
                if (c) Physics.IgnoreCollision(bulletCol, c, true);
        }

        // 6️⃣ Optional: clean up trail
        var trail = b.GetComponent<TrailRenderer>();
        if (trail) Destroy(trail);

        // 7️⃣ Destroy after lifetime
        Destroy(b, bulletLifetime);
    }

}
