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
    

    //bool savePresent = false;

    private void Start()
    {
        Cursor.visible= true;
        //if there is a save present, continue should be enabled and save present should be set to true
        if (GameManager.instance.data != null)
        {
            continueButton.enabled = true;
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
        GameManager.instance.Load(2);
    }
    public void Continue()
    {
        GameManager.instance.Load(PlayerPrefs.GetInt("scene"));
    }
    
}
