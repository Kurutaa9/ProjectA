using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Explosion Effect")]
    public GameObject explosionPrefab; // assign explosion particle prefab here
    public float speed = 10f;

    void Update()
    {
        // move along the bullet's facing (Z) every frame
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // Called when using NON-trigger colliders (isTrigger = false)
    void OnCollisionEnter(Collision collision)
    {
        Vector3 hitPoint = collision.contacts.Length > 0
            ? collision.contacts[0].point
            : collision.transform.position;

        HandleHit(collision.gameObject, hitPoint);
    }

    // Called when using trigger colliders (isTrigger = true)
    void OnTriggerEnter(Collider other)
    {
        Vector3 hitPoint = other.ClosestPoint(transform.position);
        HandleHit(other.gameObject, hitPoint);
    }

    void HandleHit(GameObject hitObject, Vector3 hitPoint)
    {
        // Ignore hitting the player
        if (hitObject.CompareTag("Player")) return;

        // When it hits an enemy
        if (hitObject.CompareTag("Enemy"))
        {
            // Spawn explosion effect at the collision point
            if (explosionPrefab != null)
            {
                // small offset so it’s not inside the floor
                Vector3 spawnPos = hitPoint + Vector3.up * 0.05f;
                Instantiate(explosionPrefab, spawnPos, Quaternion.identity);
            }

            // Destroy the enemy
            Destroy(hitObject);
        }

        // Destroy the bullet after collision
        Destroy(gameObject);
    }
}
