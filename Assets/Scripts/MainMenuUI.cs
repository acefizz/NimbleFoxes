using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] Button continueButton;

    GameData data;
    //bool savePresent = false;

    private void Start()
    {
        Cursor.visible = true;
        //if there is a save present, continue should be enabled and save present should be set to true
        if (GameDataSave.LoadGameData() != null)
        {
            continueButton.enabled = true;
            data = GameDataSave.LoadGameData();
        }
        else
            continueButton.enabled = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Options()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    
    public void Back()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
    
    public void NewGame()
    {
        GameDataSave.DeleteSaves();
        SceneManager.LoadScene(2);
    }

    public void Continue()
    {
        GameManager.instance.LoadLevel(data.scene);
    }
    
}
