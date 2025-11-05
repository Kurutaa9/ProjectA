using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HomingBullet : MonoBehaviour
{
    public Transform target;            // assign on spawn
    public float speed = 12f;
    public float turnRateDeg = 360f;    // how fast to rotate toward target
    public float lifeTime = 5f;         // auto-despawn
    public bool destroyOnAnyHit = true; // optional

    public float targetYOffset = 1.0f;

    Rigidbody rb;
    float spawnTime;

    void Awake() => rb = GetComponent<Rigidbody>();

    void OnEnable()
    {
        spawnTime = Time.time;
        if (rb) rb.velocity = transform.forward * speed;
    }

    void FixedUpdate()
    {
        // lifetime/despawn
        if (Time.time - spawnTime >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        if (target == null) return;

        // rotate toward target

        // Vector3 pos = target.position;
        // pos.y += 2f;
        // target.position = pos;

        Vector3 aimPoint = target.position + Vector3.up * targetYOffset;

        Vector3 toTarget = (aimPoint - transform.position).normalized;
        // Vector3 toTarget = (target.position - transform.position).normalized;
        Quaternion desired = Quaternion.LookRotation(toTarget, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, desired, turnRateDeg * Time.fixedDeltaTime
        );

        // move forward
        rb.velocity = transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!destroyOnAnyHit) return;
        // You can add damage here if you have a Health script.
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is hit! by enemy mafia bullet");
            if (PersistentManager.Instance.health >= 1)
            {
                PersistentManager.Instance.updateHealth(1);
            }
            Destroy(gameObject);
        }

        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
