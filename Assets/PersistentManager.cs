using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static PersistentManager Instance;
    public int health = 3; //when 0 player dies

    public int levelId = 2;

    public int score = 0;

    public bool updateRequest = false;

    public bool died = false;

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
}
