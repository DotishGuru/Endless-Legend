using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private static GameObject _player;
    private static GameObject _eventSystem;
    private static bool _isPaused;
    private static bool _isGameOver;
    private static GameObject levelUI;
    public static bool IsPaused {get{return _isPaused;}}
    public static bool IsGameOver {get{return _isGameOver;}}
    public static GameOverMenu gameOverMenu;
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
            Debug.Log("Manager destroyed");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Manager else");
            _instance = this;
        }
    }

    private void DestroyDontDestroyObject()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        Destroy(_player);

        _eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        Destroy(_eventSystem);
    }

    public void RestartLevel()
    {
        if(_isPaused)
        {
            UnpauseGame();
        }

        DestroyDontDestroyObject();
        _isGameOver = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        if(_isPaused)
        {
            UnpauseGame();
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartGame()
    {
        if(_isPaused)
        {
            UnpauseGame();
        }

        DestroyDontDestroyObject();
        _isGameOver = false;

        LoadSpecificLevel(Levels.Playground);
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
        _isGameOver = true;
        
        levelUI = GameObject.FindGameObjectWithTag("LevelUI");
        gameOverMenu = levelUI.GetComponent<GameOverMenu>();
        gameOverMenu.GameOver();
        PauseGame();
    }    

    public void ExitGame()
    {
        Debug.Log("Exit");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void LoadSpecificLevel(Levels level)
    {
        if(_isPaused)
        {
            UnpauseGame();
        }

        if(level == Levels.MainMenu)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            Destroy(_player);
            _isGameOver = false;
        }

        SceneManager.LoadScene(level.ToString());
    }

    public void LoadSpecificLevel(string sceneName)
    {
        if(_isPaused)
        {
            UnpauseGame();
        }

        if(sceneName == Levels.MainMenu.ToString())
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            Destroy(_player);
            _isGameOver = false;
        }

        SceneManager.LoadScene(sceneName);
    }

    public enum Levels
    {
        Playground,
        Level_1,
        MainMenu = 100
    }
}
