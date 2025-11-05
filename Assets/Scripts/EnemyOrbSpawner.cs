using UnityEngine;

public class EnemyOrbSpawner : MonoBehaviour
{
    [Header("Orb Setup")]
    public GameObject orbPrefab;
    public int orbCount = 4;
    public float radius = 2.5f;
    public float heightOffset = 0.8f;

    [Header("Rotation")]
    public float angularSpeedDeg = 180f; // ring rotation speed

    private Transform ring;

    void Start()
    {
        if (!orbPrefab || orbCount <= 0)
        {
            Debug.LogWarning("EnemyOrbSpawner: Missing orbPrefab or orbCount <= 0");
            return;
        }

        // ✅ Create the rotating ring under the enemy
        ring = new GameObject("OrbitRing").transform;
        ring.SetParent(transform, worldPositionStays: false);
        ring.localPosition = Vector3.up * heightOffset;

        // Evenly space orbs around the ring
        float stepDeg = 360f / orbCount;
        for (int i = 0; i < orbCount; i++)
        {
            float rad = Mathf.Deg2Rad * (i * stepDeg);
            Vector3 localPos = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * radius;

            // ✅ Spawn orb as child of the ring (which is child of the enemy)
            var orb = Instantiate(orbPrefab, ring);
            orb.transform.localPosition = localPos;
            orb.transform.localRotation = Quaternion.identity;
            orb.transform.localScale = Vector3.one;

            // Also make sure the orb’s trigger works
            var orbScript = orb.GetComponent<OrbitingDamageOrb>();
            if (orbScript == null)
                orb.AddComponent<OrbitingDamageOrb>();
        }
    }

    void Update()
    {
        if (ring == null) return;
        ring.Rotate(0f, angularSpeedDeg * Time.deltaTime, 0f, Space.World);
    }
}
