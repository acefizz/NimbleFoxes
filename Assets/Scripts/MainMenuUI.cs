using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;
    

    //bool savePresent = false;

    private void Start()
    {
        //if there is a save present, continue should be enabled and save present should be set to true
        
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
        
    }
    public void Continue()
    {
        
    }
}
