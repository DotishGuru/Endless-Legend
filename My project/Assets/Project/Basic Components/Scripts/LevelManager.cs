using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private GameObject player;
    private RPGEntity playerRPG;
    public GameObject spawnPoint;
    public TMP_Text levelNameText;
    public TMP_Text levelGoalText;
    public string levelGoal;
    
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
        
        SpawnPlayer();
        playerRPG.InitUIBars();
    }

    private void SpawnPlayer()
    {     
        player.transform.position = spawnPoint.transform.position;
    }
}
