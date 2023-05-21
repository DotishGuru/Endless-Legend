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
            GameManager.Instance.UnpauseGame();
        }
    }
}
