using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private GameObject player;    
    public GameObject playerPrefab;
    private GameObject eventSystem;
    public GameObject eventSystemPrefab;
    private RPGEntity playerRPG;
    public GameObject spawnPoint;
    public TMP_Text levelNameText;
    public TMP_Text levelGoalText;
    public string levelGoal;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            SpawnPlayer();       
        }
        else
        {
            player = Instantiate(playerPrefab);
            SpawnPlayer();
        }

        if(GameObject.FindGameObjectWithTag("EventSystem") != null)
        {
            eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        }
        else
        {
            eventSystem = Instantiate(eventSystemPrefab);
        }
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRPG = player.GetComponent<RPGEntity>();   

        if(levelNameText != null)
        {
            levelNameText.text = string.Format("Level Name: {0}", SceneManager.GetActiveScene().name);    
        }

        if(levelGoalText != null)
        {
            levelGoalText.text = string.Format("Level Goal: {0}", levelGoal);
        }
        
        playerRPG.InitUIBars();
    }

    private void SpawnPlayer()
    {     
        player.transform.position = spawnPoint.transform.position;
    }
}
