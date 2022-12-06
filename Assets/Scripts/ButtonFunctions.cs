using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    bool upgradeMenuOpen = false;
    public void ResumeGame()
    {
        //Unpause the game
        GameManager.instance.ShowMenu(GameManager.MenuType.CloseAll);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        GameManager.instance.ShowMenu(GameManager.MenuType.CloseAll);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpgradeMenuOpen()
    {
        GameManager.instance.ShowMenu(GameManager.MenuType.Upgrade, true);
    }
    public void UpgradeMenuClose()
    {
        GameManager.instance.ShowMenu(GameManager.MenuType.Pause, true);
    }

    public void AddMaxJumps(int amount)
    {
        int cost = GameManager.instance.jumpCost;
        if (GameManager.instance.playerScript.coins >= cost)
        {
            GameManager.instance.playerScript.coins -= cost;
            GameManager.instance.playerScript.AddJumps(amount);
        }
    }

    public void AddDamage(int amount)
    {
        int cost = GameManager.instance.damageCost;
        if (GameManager.instance.playerScript.coins >= cost)
        {
            GameManager.instance.playerScript.coins -= cost;
            GameManager.instance.playerScript.AddDamage(amount);
        }
    }

    public void AddSpeed(int amount)
    {
        int cost = GameManager.instance.speedCost;
        if (GameManager.instance.playerScript.coins >= cost)
        {
            GameManager.instance.playerScript.coins -= cost;
            GameManager.instance.playerScript.AddSpeed(amount);
        }
    }
}
