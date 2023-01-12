using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public GameObject player;
    public PlayerController playerScript;
    //public GameObject playerSpawnPos;
    public GameObject playerFlashDamage;
    public Image playerHpBar;
    
    

    public GameObject reticle;

    internal bool isPaused = false;
    float timeScaleOriginal;

    [Header("--- UI Menus ---")]
    [SerializeField] AudioSource menuMusic;

    public GameObject welcomeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject upgradeMenu;
    public GameObject optionsMenu; //TODO: This menu needs to be able to be opened and closed //Buttons are already set up, the functionality just needs applied

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

    [Header("--- Icons ---")]
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public GameObject weapon4;
    public GameObject weapon5;
    public GameObject weapon6;
    public string[] gunNames = new string[6];
    public GameObject ability1;
    public GameObject ability2;
    public GameObject ability3;
    public GameObject ability4;
    public GameObject ability5;
    public GameObject ability6;
    public string[] abilityNames = new string[6];

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

    [Header("--- Checkpoint info - Don't change ---")]
    public Vector3 playerSpawnLocation;
    public Vector3 checkpoint;
    public string checkpointName;
    public int levelCheckpoint;
    public List<GunSetup> gunCheckpoint;
    public int gunSelectCheckpoint;
    //List of Abilities and the current selection
    public int coinsCheckpoint;
    //Save the scene as it is (game objects, enemies)

    //An enum to enforce menu types.
    public enum MenuType { WelcomeMenu, Pause, Win, Lose, Upgrade, PlayerDamageFlash, CloseAll }

    private void Awake()
    {
        if (instance == null)
            instance = this;


        player = GameObject.FindGameObjectWithTag("Player");
        
        if (!player )
            Debug.LogError("Player not found in scene or tagged as Player or named Player");

        playerScript = player.GetComponent<PlayerController>();

        playerSpawnLocation = playerScript.ReturnStartCheckpoint();

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

        //TODO: see if a menu is active and if so, play the clip on attached on game manager
        
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
