using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static PersistentManager Instance;

    [Header("Player Stats")]
    public int health = 3; // when 0 player dies
    public int levelId = 2;
    public int score = 0;

    [Header("Status Flags")]
    public bool updateRequest = false;
    public bool died = false;
    public bool winBool = false;

    [Header("Audio")]
    public AudioClip hurtSound;
    public AudioClip diedSound;
    public float soundVolume = 1.0f;

    [Header("Particles")]
    public GameObject damageParticle; // assign particle prefab here

    [Header("Enemy Tracking")]
    public int enemyAmountPersistent;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateHealth(int val)
    {
        health -= val;
        updateRequest = true;

        if (val > 0 && health > 0)
        {
            PlayDamageEffects();
        }
        if (health <= 0 && !died)
        {
            died = true;
            PlayDeathEffects();
            Debug.Log("Player has died!");
        }
    }

    void PlayDamageEffects()
    {
        // Try to find player position
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 pos = player ? player.transform.position + Vector3.up * 1f : Vector3.zero;

        // Spawn damage particle
        if (damageParticle != null)
        {
            Instantiate(damageParticle, pos, Quaternion.identity);
        }

        // Play hurt sound
        if (hurtSound != null)
        {
            AudioSource.PlayClipAtPoint(hurtSound, pos, soundVolume);
        }
    }

    void PlayDeathEffects()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 pos = player ? player.transform.position + Vector3.up * 1f : Vector3.zero;

        // Play death particle (reuse damage particle or add another if you want)
        if (damageParticle != null)
        {
            Instantiate(damageParticle, pos, Quaternion.identity);
        }

        // Play death sound
        if (diedSound != null)
        {
            AudioSource.PlayClipAtPoint(diedSound, pos, soundVolume);
        }
    }

    
    public void updateEnemyAmount(int val)
    {
        enemyAmountPersistent -= val;
        Debug.Log("enemy currently: " + enemyAmountPersistent);
        // Debug.Log(enemyAmountPersistent);
    }
}
