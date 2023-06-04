using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelectorMenu : MonoBehaviour
{
    public TMP_Dropdown levelSelectorDropdown;
    private List<string> levelsNames;
    public GameObject pauseMenu;
    public GameObject levelSelectorMenu;

    private void Awake() 
    {
        levelsNames = new List<string>();

        foreach (GameManager.Levels levelName in System.Enum.GetValues(typeof(GameManager.Levels)))
        {
            if((int)levelName >= 100)
            {
                break;
            }
            Debug.Log(string.Format("Scene name: {0}", levelName.ToString()));
            levelsNames.Add(levelName.ToString());            
        }

        levelSelectorDropdown.AddOptions(levelsNames); 
    }

    public void LoadPauseMenu()
    {
        levelSelectorMenu.SetActive(false);
        pauseMenu.SetActive(true);          
    }

    public void LoadSelectedLevel()
    {
        GameManager.Instance.LoadSpecificLevel((GameManager.Levels)levelSelectorDropdown.value);
    }
}
