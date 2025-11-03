using UnityEngine;
using TMPro;
using System;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.SceneManagement;
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

            Schedule(5f, () => changeLevel(PersistentManager.Instance.levelId));//change level using helper above
            Schedule(5f, () => PersistentManager.Instance.health = 3);//change level using helper above
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
