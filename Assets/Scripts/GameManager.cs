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

    internal bool isPaused;
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

    [Header("--- Upgrade Costs ---")]
    [SerializeField]
    [Range(1, 3)] public int jumpCost = 1;
    [SerializeField]
    [Range(1, 3)] public int damageCost = 1;
    [SerializeField]
    [Range(1, 3)] public int speedCost = 1;

    public int enemyCount = 0;

    //An enum to enforce menu types.
    public enum MenuType { Pause, Win, Lose, Upgrade, PlayerDamageFlash }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        timeScaleOriginal = Time.timeScale;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseGame();
            ShowMenu(MenuType.Pause, isPaused);
            //Always default to closed
            ShowMenu(MenuType.Upgrade, false);
        }
        if (isPaused)
            DoStats();
    }

    private void DoStats()
    {
        jumpCount.text = "Jumps : " + playerScript.GetMaxJumps();
        damageCount.text = "Damage : " + playerScript.GetDamage();
        speedCount.text = "Speed : " + playerScript.GetSpeed();
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : timeScaleOriginal;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    /// <summary>
    /// Pauses gameplay then shows/hides a menu with the given name 
    /// </summary>
    /// <param name="menu"></param>
    /// <param name="activeState"></param>


    public void ShowMenu(MenuType menu, bool activeState)
    {
        //refactor the menu to an enum later. It'll be much better on intellisense and easier to know what to put into the params.
        switch (menu)
        {
            case MenuType.Pause:
                pauseMenu.SetActive(activeState);
                break;
            case MenuType.Win:
                winMenu.SetActive(activeState);
                break;
            case MenuType.Lose:
                loseMenu.SetActive(activeState);
                break;
            case MenuType.Upgrade:
                upgradeMenu.SetActive(activeState);
                break;
            case MenuType.PlayerDamageFlash:
                playerFlashDamage.SetActive(activeState);
                break;
            default:
                break;

        }
    }

    public void UpdateEnemyCount(int amount)
    {
        enemyCount += amount;
        if (enemyCount <= 0)
        {
            ShowMenu(MenuType.Win, true);
            PauseGame();
        }
    }
}
