using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    bool upgradeMenuOpen = false;
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

    public void UpgradeMenuOpen()
    {
        upgradeMenuOpen = true;
        //Show upgrade menu.
        GameManager.instance.ShowMenu(GameManager.MenuType.Upgrade, upgradeMenuOpen);
        //Hide pause menu
        GameManager.instance.ShowMenu(GameManager.MenuType.Pause, !upgradeMenuOpen);
    }
    public void UpgradeMenuClose()
    {
        upgradeMenuOpen = false;
        //Show upgrade menu.
        GameManager.instance.ShowMenu(GameManager.MenuType.Upgrade, upgradeMenuOpen);
        //Hide pause menu
        GameManager.instance.ShowMenu(GameManager.MenuType.Pause, !upgradeMenuOpen);
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
