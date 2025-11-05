using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;            // where bullets spawn
    public Transform target;               // usually player
    public float fireInterval = 1.5f;

    Coroutine loop;

    void OnEnable()  { loop = StartCoroutine(FireLoop()); }
    void OnDisable() { if (loop != null) StopCoroutine(loop); }

    IEnumerator FireLoop()
    {
        var wait = new WaitForSeconds(fireInterval);
        while (true)
        {
            FireOnce();
            yield return wait;
        }
    }

    public void FireOnce()
    {
        if (bulletPrefab == null || firePoint == null || target == null) return;

        Vector3 pos = firePoint.position;
        pos.y += 2f;
        firePoint.position = pos;

        var go = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        var hb = go.GetComponent<HomingBullet>();
        if (hb != null) hb.target = target;
    }
}
