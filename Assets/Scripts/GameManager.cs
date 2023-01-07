using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public GameObject player;
    public PlayerController playerScript;
    // public GameObject playerSpawnPos;
    public GameObject playerFlashDamage;
    public Image playerHpBar;
    
    

    public GameObject reticle;

    internal bool isPaused = false;
    float timeScaleOriginal;

    [Header("--- UI Menus ---")]
    public GameObject welcomeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject upgradeMenu;

    [Header("--- UI Pickups ---")]
    public GameObject Pickups;
    [SerializeField] TextMeshProUGUI healthText;
    public string healthDisplay;
    [SerializeField] TextMeshProUGUI coinText;
    public string coinDisplay;
    [SerializeField] TextMeshProUGUI weaponText;
    public string weaponDisplay;
    [SerializeField] TextMeshProUGUI abiltyText;
    public string abiltyDisplay;

    [Header("--- UI Upgrade Text ---")]
    //textmeshpro
    public TextMeshProUGUI speedCount;
    public TextMeshProUGUI jumpCount;
    public TextMeshProUGUI damageCount;
    public TextMeshProUGUI playerCoins;
    public TextMeshProUGUI enemiesLeft;

    [Header("--- Upgrade Costs ---")]
    [SerializeField]
    [Range(1, 3)] public int jumpCost = 1;
    [SerializeField]
    [Range(1, 3)] public int damageCost = 1;
    [SerializeField]
    [Range(1, 3)] public int speedCost = 1;

    public int enemyCount;

    //An enum to enforce menu types.
    public enum MenuType { WelcomeMenu, Pause, Win, Lose, Upgrade, PlayerDamageFlash, CloseAll }

    private void Awake()
    {
        if (instance == null)
            instance = this;


        //player = GameObject.FindGameObjectWithTag("Player");
        
        if (!player )
            Debug.LogError("Player not found in scene or tagged as Player or named Player");

        //playerScript = player.GetComponent<PlayerController>();

        //playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        

        timeScaleOriginal = Time.timeScale;

    }

    void Start()
    {
        ShowMenu(MenuType.WelcomeMenu, true);
        
    }

    private void Update()
    {
        healthText.text = healthDisplay;
        coinText.text = coinDisplay;
        weaponText.text = weaponDisplay;
        abiltyText.text = abiltyDisplay;

        if (Input.GetButtonDown("Cancel") && !playerScript.isDead)
        {
            isPaused = !isPaused;
            if (isPaused)
                ShowMenu(MenuType.Pause, true);
            else
                ShowMenu(MenuType.CloseAll);
        }
        if (isPaused)
            DoStats();

        if (!player || !playerScript)
        {
            Debug.Log("Still not found, searching again");
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<PlayerController>();
        }
    }

    private void DoStats()
    {
        jumpCount.text = "Jumps : " + playerScript.GetMaxJumps();
        damageCount.text = "Damage : " + playerScript.GetDamage();
        speedCount.text = "Speed : " + playerScript.GetSpeed();
        playerCoins.text = "Coins: " + playerScript.coins.ToString();
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
            Cursor.visible = activeState;
            Time.timeScale = activeState ? 0 : timeScaleOriginal;
        }

        switch (menu)
        {
            case MenuType.WelcomeMenu:
                welcomeMenu.SetActive(activeState);
                break;
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
                welcomeMenu.SetActive(false);
                isPaused = false;
                break;
            default:
                break;

        }
    }

    public void UpdateEnemyCount(int amount)
    {
        enemyCount += amount;
        enemiesLeft.text = enemyCount.ToString("F0");
    }

}
