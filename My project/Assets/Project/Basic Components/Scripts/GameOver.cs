using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    private GameOverMenu gameOverMenu;
    private GameObject levelUI;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            levelUI = GameObject.FindGameObjectWithTag("LevelUI");
            gameOverMenu = levelUI.GetComponent<GameOverMenu>();
            gameOverMenu.GameOver();
        }  
    }
}
