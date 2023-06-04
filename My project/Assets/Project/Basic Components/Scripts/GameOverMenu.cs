using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenu;

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        GameManager.Instance.PauseGame();
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
