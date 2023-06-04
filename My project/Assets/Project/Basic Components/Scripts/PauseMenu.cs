using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject levelSelectorMenu;

    public void Pause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if(!GameManager.IsPaused)
        {
            pauseMenu.SetActive(true);
            GameManager.Instance.PauseGame();
        }
        else
        {
            pauseMenu.SetActive(false);
            levelSelectorMenu.SetActive(false);
            GameManager.Instance.UnpauseGame();
        }
    }

    public void LoadPauseMenu()
    {
        if(levelSelectorMenu.activeSelf == true)
        {
            levelSelectorMenu.SetActive(false);
        }

        if(pauseMenu.activeSelf == false)
        {            
            pauseMenu.SetActive(true);
        }
    }

    public void LoadLevelSelectorMenu()
    {
        if(pauseMenu.activeSelf == true)
        {            
            pauseMenu.SetActive(false);
        }

        if(levelSelectorMenu.activeSelf == false)
        {
            levelSelectorMenu.SetActive(true);
        }        
    }    

    public void LoadMainMenu()
    {
        GameManager.Instance.LoadSpecificLevel(GameManager.Levels.MainMenu);
    }

    public void ExitGame()
    {
        GameManager.Instance.ExitGame();
    }

    public void RestartGame()
    {        
        GameManager.Instance.RestartGame();
    }

    public void RestartLevel()
    {
        GameManager.Instance.RestartLevel();
    }
}
