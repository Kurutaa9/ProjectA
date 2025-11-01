using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        if (player != null)
            agent.SetDestination(player.position);

        if (animator != null)
        {
            // Set Speed based on agent’s velocity
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }
}
