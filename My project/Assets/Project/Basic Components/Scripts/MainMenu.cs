using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject eventSystem;
    public GameObject eventSystemPrefab;
    
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("EventSystem") != null)
        {
            eventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        }
        else
        {
            eventSystem = Instantiate(eventSystemPrefab);
        }
    }

    public void ExitGame()
    {
        GameManager.Instance.ExitGame();
    }

    public void StartGame()
    {
        GameManager.Instance.LoadSpecificLevel(GameManager.Levels.Playground.ToString());
    }
}
