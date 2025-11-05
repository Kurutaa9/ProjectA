using UnityEngine;
using TMPro;
using System;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
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

    public Transform player;

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
        heart = GameObject.FindGameObjectWithTag("Heart").GetComponent<Renderer>();
        // levelText = GameObject.FindGameObjectWithTag("LevelText").GetComponent<TextMeshProUGUI>();
    }
    
    public void updateValues()
    {
        if (!heart) return;
        heart = GameObject.FindGameObjectWithTag("Heart").GetComponent<Renderer>();
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
            case 0:
                Debug.Log("player died/player reached 0 health");
                PersistentManager.Instance.died = true;
                heart.material.color = Color.black;
                break;
            default:
                Debug.Log("health not found/error");
                heart.material.color = Color.yellow;
                break;
        }

        levelText.text = levelString + PersistentManager.Instance.levelId.ToString();// "level " + "1"
        Debug.Log(levelString + PersistentManager.Instance.levelId.ToString());
    }

    public void Schedule(float time, Action action)
    {
        StartCoroutine(ScheduleRoutine(time, action));
        Debug.Log("in shceudler");
    }

    private IEnumerator ScheduleRoutine(float time, Action action)
    {
        Debug.Log("waiting");
        yield return new WaitForSecondsRealtime(time);

        Debug.Log("waited");
        action();
    }

    public void changeLevel(int levelId)
    {
        string levelnow = "level";
        levelnow += levelId.ToString(); //concate "level" with "1" or other number
        SceneManager.LoadScene(levelnow);//load it using the new string "level1"
        PersistentManager.Instance.levelId = levelId; // update manager's levelid
    }
    
    void Update()
    {
        if (PersistentManager.Instance.updateRequest)
        {

            Debug.Log("UpdateRequest called!");
            updateValues();
            PersistentManager.Instance.updateRequest = false;
        }
        if (PersistentManager.Instance.died)
        {
            PersistentManager.Instance.died = false;
            Destroy(player.gameObject);
            Schedule(5f, () => changeLevel(PersistentManager.Instance.levelId));//change level using helper above
            Schedule(5f, () => PersistentManager.Instance.health = 3);//change level using helper above
        }

        if(PersistentManager.Instance.enemyAmountPersistent <= 0)
        {
            OnAllEnemiesDead();
        }

    }
    void Start()
    {

        heart = GameObject.FindGameObjectWithTag("Heart").GetComponent<Renderer>();

        if (!heart) return;
        switch (PersistentManager.Instance.levelId)
        {
            case 1:
                Debug.Log("Persistent Manager has 5 enemys");
                PersistentManager.Instance.enemyAmountPersistent = 10;
                break;
            case 2:
                Debug.Log("Persistent Manager has 6 enemys");

                PersistentManager.Instance.enemyAmountPersistent = 6;
                break;
            case 3:
                Debug.Log("Persistent Manager has 7 enemys");

                PersistentManager.Instance.enemyAmountPersistent = 8;
                break;
        }
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
            if (PersistentManager.Instance.levelId == 3)
            {
                // changeLevel(1);// 3->1
                Schedule(3f, () => changeLevel(1));
            }
            else
            {
                // changeLevel(PersistentManager.Instance.levelId += 1); // 1->2, 2->3 
                Schedule(3f, () => changeLevel(PersistentManager.Instance.levelId += 1));

            }
        }

        // Time.timeScale = 0f;
    }

    
}
