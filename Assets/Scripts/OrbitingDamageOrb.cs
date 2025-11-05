using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class OrbitingDamageOrb : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 1;


    [Header("Knockback")]
    public float knockbackForce = 18f;       // How strong the push is
    public ForceMode knockbackMode = ForceMode.Impulse;
    void Awake()
    {
        // Ensure a valid trigger collider (Sphere is best for orbs)
        var col = GetComponent<Collider>();
        if (col is MeshCollider mc && !mc.convex)
        {
            // Replace invalid concave mesh collider
            Destroy(mc);
            col = gameObject.AddComponent<SphereCollider>();
        }
        col.isTrigger = true;

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Your player health script name here:
        // var hp = other.GetComponent<PlayerHealth>();
        // if (hp != null)
        // {
        //     hp.TakeDamage(damage, transform.position);
        // }

        var rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Calculate direction from orb to player
            Vector3 dir = (other.transform.position - transform.position).normalized;
            dir.y = 0.3f; // slight upward lift, optional
            rb.AddForce(dir * knockbackForce, knockbackMode);
        }

        if (PersistentManager.Instance.health <= 1)
        {
            PersistentManager.Instance.updateHealth(1);
        }
        else
        {
            PersistentManager.Instance.updateHealth(1);
            PersistentManager.Instance.died = true;
        }

        // Keep orbiting; do not destroy unless you want one-hit orbs.
        // Destroy(gameObject);
    }
}
