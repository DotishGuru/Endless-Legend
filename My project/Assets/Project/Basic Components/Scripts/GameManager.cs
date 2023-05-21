using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private static bool _isPaused;
    public static bool IsPaused {get{return _isPaused;}}
    public static GameObject gameOverMenu;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.Log("Game manager was not created");
            }            

            return _instance;
        }        
    }

    private void Awake()
    {
        Debug.Log("Manager awake");
        _instance = this;

        if (_instance != null && _instance != this)
        {          
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void GameOver()
    {
        gameOverMenu = GameObject.FindGameObjectWithTag("GameOverMenu");
        gameOverMenu.SetActive(true);
        PauseGame();
    }    
}
