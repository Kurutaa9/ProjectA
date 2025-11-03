using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Explosion Effect")]
    public GameObject explosionPrefab; // assign explosion particle prefab here

    void OnCollisionEnter(Collision collision)
    {
        // Ignore hitting the player
        if (collision.gameObject.CompareTag("Player")) return;

        // When it hits an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Spawn explosion effect at the collision point
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
            }

            // Destroy the enemy
            Destroy(collision.gameObject);
        }

        // Destroy the bullet after collision
        Destroy(gameObject);
    }
}
