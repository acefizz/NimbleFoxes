using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public GameObject player;
    public PlayerController playerScript;
    public GameObject playerSpawnPos;
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI enemiesLeft;

    public GameObject reticle;

    internal bool isPaused = false;
    float timeScaleOriginal;

    [Header("--- UI Menus ---")]
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject upgradeMenu;
    public GameObject playerFlashDamage;

    [Header("--- UI Upgrade Text ---")]
    //textmeshpro
    public TextMeshProUGUI speedCount;
    public TextMeshProUGUI jumpCount;
    public TextMeshProUGUI damageCount;
    public TextMeshProUGUI playerCoins;

    [Header("--- Upgrade Costs ---")]
    [SerializeField]
    [Range(1, 3)] public int jumpCost = 1;
    [SerializeField]
    [Range(1, 3)] public int damageCost = 1;
    [SerializeField]
    [Range(1, 3)] public int speedCost = 1;

    public int enemyCount = 0;

    //An enum to enforce menu types.
    public enum MenuType { Pause, Win, Lose, Upgrade, PlayerDamageFlash, CloseAll }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");

        timeScaleOriginal = Time.timeScale;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel")  && !playerScript.isDead)
        {
            isPaused = !isPaused;
            if (isPaused)
                ShowMenu(MenuType.Pause, true);
            else
                ShowMenu(MenuType.CloseAll);
        }
        if (isPaused)
            DoStats();
    }

    private void DoStats()
    {
        jumpCount.text = "Jumps : " + playerScript.GetMaxJumps();
        damageCount.text = "Damage : " + playerScript.GetDamage();
        speedCount.text = "Speed : " + playerScript.GetSpeed();
        playerCoins.text = playerScript.coins.ToString() + " out of " + jumpCost.ToString() + " coins";
    }

    /// <summary>
    /// Pauses gameplay then shows/hides a menu with the given name 
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="activeState"></param>


    public void ShowMenu(MenuType menu, bool activeState = false)
    {
        if (menu != MenuType.PlayerDamageFlash)
        {
            Cursor.lockState = activeState ? CursorLockMode.Confined : CursorLockMode.Locked;
            Time.timeScale = activeState ? 0 : timeScaleOriginal;
        }

        switch (menu)
        {
            case MenuType.Pause:
                pauseMenu.SetActive(activeState);
                upgradeMenu.SetActive(false);
                break;
            case MenuType.Win:
                GameManager.instance.playerScript.isDead = true; //lol
                winMenu.SetActive(activeState);
                break;
            case MenuType.Lose:
                loseMenu.SetActive(activeState);
                break;
            case MenuType.Upgrade:
                pauseMenu.SetActive(false);
                upgradeMenu.SetActive(activeState);
                break;
            case MenuType.PlayerDamageFlash:
                playerFlashDamage.SetActive(activeState);
                break;
            case MenuType.CloseAll:
                pauseMenu.SetActive(false);
                winMenu.SetActive(false);
                loseMenu.SetActive(false);
                upgradeMenu.SetActive(false);
                playerFlashDamage.SetActive(false);
                isPaused = false;
                break;
            default:
                break;

        }
    }

    public void UpdateEnemyCount(int amount)
    {
        enemyCount += amount;
        enemiesLeft.text = "Enemies Left: " + enemyCount;
        if (enemyCount <= 0)
        {
            ShowMenu(MenuType.Win, true);
        }
    }
    public void UpdatePlayerHealth(int hp, int hpOrig)
    {
        playerHealth.text = "HP: " + hp + " / " + hpOrig;
    }

}
