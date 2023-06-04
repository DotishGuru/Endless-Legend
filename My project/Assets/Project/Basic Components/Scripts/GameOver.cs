using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private GameOverMenu gameOverMenu;
    private GameObject levelUI;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Void") || other.gameObject.CompareTag("LevelEnd"))
        {
            levelUI = GameObject.FindGameObjectWithTag("LevelUI");
            gameOverMenu = levelUI.GetComponent<GameOverMenu>();
            gameOverMenu.GameOver();
        }  
    }
}
