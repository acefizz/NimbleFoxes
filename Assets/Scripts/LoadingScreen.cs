using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public float delayLoading;
    // Start is called before the first frame update
    void Start()
    {
        delayLoading = 10;
        LoadingLevel(1);
    }

    public void LoadingLevel(int scene)
    {
        SceneManager.LoadScene(0);
        new WaitForSeconds(delayLoading);
        SceneManager.LoadScene(scene);
    }
    public void NewGame()
    {
        LoadingLevel(2);
    }
    public void Continue()
    {

    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadLevelOne()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadLevelTwo()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadLevelThree()
    {
        SceneManager.LoadScene(4);
    }
    public void LoadBossLevel()
    {
        SceneManager.LoadScene(5);
    }
}
