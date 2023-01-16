using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    //bool upgradeMenuOpen = false;
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
    public void Respawn()
    {

        GameManager.instance.playerScript.ReturnController().enabled = false;
        GameManager.instance.ShowMenu(GameManager.MenuType.CloseAll);
        GameManager.instance.playerSpawnLocation = GameManager.instance.checkpoint;
        GameManager.instance.player.transform.position = GameManager.instance.playerSpawnLocation;
        GameManager.instance.playerScript.ReturnController().enabled = true;
    }
    public void UpgradeMenuOpen()
    {
        GameManager.instance.ShowMenu(GameManager.MenuType.Upgrade, true);
    }
    public void UpgradeMenuClose()
    {
        GameManager.instance.ShowMenu(GameManager.MenuType.Pause, true);
    }
    public void OptionsOpen()
    {
        GameManager.instance.ShowMenu(GameManager.MenuType.OptionsMenu, true);
    }
    public void OptionsClose()
    {
        GameManager.instance.ShowMenu(GameManager.MenuType.OptionsMenu, false);
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
