using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class SceneListTest : MonoBehaviour
{
    public TMP_Dropdown levelSelectorDropdown;
    private List<string> levelsNames;

    private void Awake() 
    {
        levelsNames = new List<string>();
        /*
        Scene tmpScene;
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            tmpScene = SceneManager.GetSceneByBuildIndex(i);

            Debug.Log(string.Format("Scene name: {0}", tmpScene.name));
        } 
        */
        //Debug.Log(string.Format("Scene name: {0}", GameManager.Levels.Level_1.ToString()));
        foreach (string item in System.Enum.GetNames(typeof(GameManager.Levels)))
        {
            Debug.Log(string.Format("Scene name: {0}", item.ToString()));
            levelsNames.Add(item.ToString());
            
        }
        levelSelectorDropdown.AddOptions(levelsNames);

        
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(string.Format("Item text: {0}", levelSelectorDropdown.itemText));        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
