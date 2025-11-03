using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winPanel;

    private int enemiesAlive = 0;

    void Awake()
    {
        // simple singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy()
    {
        enemiesAlive++;
        Debug.Log("Enemy spawned. Alive: " + enemiesAlive);
    }

    public void UnregisterEnemy()
    {
        enemiesAlive--;
        Debug.Log("Enemy killed. Alive: " + enemiesAlive);

        if (enemiesAlive <= 0)
        {
            OnAllEnemiesDead();
        }
    }

    void OnAllEnemiesDead()
    {
        Debug.Log("YOU WIN!");

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}
