using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winPanel;

    private int enemiesAlive = 0;

    public Renderer heart; //debug
    public Color redColor = Color.red;
    public Color blueColor = Color.blue;
    public Color greenColor = Color.green;

    public string levelString = "Level ";
    public TextMeshProUGUI levelText;

    void Awake()
    {
        // simple singleton
        // if (Instance == null)
        // {
        //     Instance = this;
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }

    }

    public void updateValues()
    {
        switch (PersistentManager.Instance.health)
        {
            case 1:
                heart.material.color = redColor;
                break;
            case 2:
                heart.material.color = blueColor;
                break;
            case 3:
                heart.material.color = greenColor;
                break;
            default:
                Debug.Log("health not found/error");
                heart.material.color = Color.black;
                break;
        }

        levelText.text = levelString + PersistentManager.Instance.levelId.ToString();// "level " + "1"
        Debug.Log(levelString + PersistentManager.Instance.levelId.ToString());
    }

    void Update()
    {
        if (PersistentManager.Instance.updateRequest)
        {

            Debug.Log("UpdateRequest called!");
            updateValues();
            PersistentManager.Instance.updateRequest = false;
        }
    }
    void Start()
    {
        updateValues();
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
