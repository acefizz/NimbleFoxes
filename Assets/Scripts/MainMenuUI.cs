using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;
    LoadingScreen loadingScreen;

    //bool savePresent = false;

    private void Start()
    {
        //if there is a save present, continue should be enabled and save present should be set to true
        loadingScreen = new LoadingScreen();
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
    public void Apply()
    {

    }
    public void NewGame()
    {
        loadingScreen.NewGame();
    }
    public void Continue()
    {
        loadingScreen.Continue();
    }
}
