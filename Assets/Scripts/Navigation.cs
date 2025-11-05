using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections;
using System;
// using System.Numerics;
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

    public float chaseRange = 10f;   // how far to start chasing
    public float stopDistance = 3f;  // how close before stopping

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null || hasExploded) return;

        if (agent.CompareTag("Mafia"))
        {
            float distance = Vector3.Distance(transform.position, player.position);

            // Start chasing if within chase range but not too close
            if (distance < chaseRange && distance > stopDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
            else
            {
                agent.isStopped = true;  // stops movement
            }
        }
        // Chase the player
        else
        {
            agent.SetDestination(player.position);
        }
        
        Vector3 direction = player.position - transform.position;
        direction.y = 0f; // keep the rotation flat on the ground
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
        }


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
            PersistentManager.Instance.updateEnemyAmount(2);
            if (PersistentManager.Instance.health > 1)
            {
                Debug.Log("Player is hit!");
                PersistentManager.Instance.updateHealth(1);
            }

            
            else //else when health is 0 restart the level
            {
                PersistentManager.Instance.updateHealth(1); //decrease health by 1 and call updatevalue
                PersistentManager.Instance.died = true;


            }
            

            
        }

        // Kill this enemy
        Destroy(gameObject, killDelay);
    }
}
