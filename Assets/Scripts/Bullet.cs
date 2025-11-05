using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Explosion Effect")]
    public GameObject explosionPrefab; // assign explosion particle prefab here
    public AudioClip explosionSound;   // assign your explosion.mp3 here
    public float speed = 10f;

    void Update()
    {
        // move along the bullet's facing (Z) every frame
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 hitPoint = collision.contacts.Length > 0
            ? collision.contacts[0].point
            : collision.transform.position;

        HandleHit(collision.gameObject, hitPoint);
    }

    void HandleHit(GameObject hitObject, Vector3 hitPoint)
    {
        // Ignore hitting the player
        // if (hitObject.CompareTag("Player")) return;

        // When it hits an enemy
        if (hitObject.CompareTag("Enemy") | hitObject.CompareTag("Mafia"))
        {
            // Spawn explosion effect
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, hitPoint, Quaternion.identity);
            }

            // Play explosion sound (temporary AudioSource)
            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, hitPoint);
                PersistentManager.Instance.updateEnemyAmount(1); //decrement enemy amount by 1
            }

            // Destroy the enemy
            Destroy(hitObject);
        }

        // Destroy the bullet after collision
        Destroy(gameObject);
    }
}
