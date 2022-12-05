using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void ResumeGame()
    {
        //Unpause the game
        GameManager.instance.PauseGame();
        GameManager.instance.ShowMenu(GameManager.MenuType.Pause, GameManager.instance.isPaused);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        //Unpause the game
        GameManager.instance.PauseGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void addJumps(int amount)
    {
        int cost = GameManager.instance.jumpCost;
        if (GameManager.instance.playerScript.coins >= cost)
        {
            GameManager.instance.playerScript.coins -= cost;
            GameManager.instance.playerScript.AddJumps(amount);
        }
    }
}
