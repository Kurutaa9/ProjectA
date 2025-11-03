using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    private Animator animator;

    [Header("Explosion Settings")]
    public GameObject explosionPrefab;   // same prefab as Bullet.explosionPrefab
    public AudioClip explosionSound;     // same clip as Bullet.explosionSound
    public float explodeDistance = 1.5f; // how close to player before exploding
    public float killDelay = 0.0f;       // delay before destroying objects (optional)

    private bool hasExploded = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null || hasExploded) return;

        // Chase the player
        agent.SetDestination(player.position);

        // Update run/walk animation
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        // Check distance to player
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= explodeDistance)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Stop moving
        if (agent != null) agent.isStopped = true;

        Vector3 pos = transform.position + Vector3.up * 0.1f;

        // Spawn explosion particle
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, pos, Quaternion.identity);
        }

        // Play explosion sound
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, pos);
        }

        // Kill the player (simple version: destroy the GameObject)
        if (player != null)
        {
            Destroy(player.gameObject, killDelay);
        }

        // Kill this enemy
        Destroy(gameObject, killDelay);
    }
}
