using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

//TODO: must have a way to switch abilities
//TODO: icon appears for ability, but not the name on pickup
//TODO: Cooldowns need functionality

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //public string scenePath;
    int scene;

    [Header("Player")]
    public GameObject player;
    public PlayerController playerScript;
    //public GameObject playerSpawnPos;
    public GameObject playerFlashDamage;
    public Image playerHpBar;

    public Button respawnButton;
    [SerializeField] TextMeshProUGUI respawnText;

    public GameObject reticle;

    internal bool isPaused = false;
    float timeScaleOriginal;

    [Header("--- UI Menus ---")]
    public AudioSource audioSource;

    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject upgradeMenu;
    public GameObject optionsMenu; 
    public GameObject tutorialPlayer;
    public GameObject tutorial;

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

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI coinsText;

    [Header("--- Upgrade Costs ---")]
    [SerializeField]
    [Range(1, 5)] public int jumpCost = 1;
    [SerializeField]
    [Range(1, 5)] public int damageCost = 1;
    [SerializeField]
    [Range(1, 5)] public int speedCost = 1;



    [Header("--- Ability Cooldowns ---")]
    List<float> coolDowns = new List<float>();
    List<float> coolDownTracker = new List<float>();

    [Header("--- Checkpoint info - Don't change ---")]
    public Vector3 playerSpawnLocation;
    public Vector3 checkpoint;
    public string checkpointName;
    public int levelCheckpoint;
    float musicVolume;

    //An enum to enforce menu types.
    public enum MenuType { Pause, Win, Lose, Upgrade, PlayerDamageFlash, OptionsMenu, Tutorial, CloseAll }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        timeScaleOriginal = Time.timeScale;
        audioSource = GetComponent<AudioSource>();
        musicVolume = audioSource.volume;
        
        //if (scenePath == null)
        //    scenePath = "Assets/Scenes/SavedScene.unity";

    }

    void Start()
    {

        //if (data == null)
        //{
        //    data = new GameData();
        //}

        GetComponent<SoundManager>().PlayMusic();

        if (playerScript != null)
        {
            playerSpawnLocation = playerScript.ReturnStartCheckpoint();
            playerScript.SetPlayerPos();
            Load();
        }

        scene = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        healthText.text = healthDisplay;
        coinText.text = coinDisplay;
        weaponText.text = weaponDisplay;
        abiltyText.text = abiltyDisplay;

        livesText.text = "Lives: " + playerScript.Lives().ToString();
        coinsText.text = "Coins: " + playerScript.coins.ToString();

        if (Input.GetButtonDown("Cancel") && (playerScript == null || !playerScript.isDead) && (/*SceneManager.GetActiveScene().buildIndex != 1 &&*/ SceneManager.GetActiveScene().buildIndex != 0))
        {
            isPaused = !isPaused;
            if (isPaused)
                ShowMenu(MenuType.Pause, true);
            else
                ShowMenu(MenuType.CloseAll);
        }
        if (isPaused)
            DoStats();
        /*
        if (!player || !playerScript)
        {
            Debug.Log("Still not found, searching again");
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<PlayerController>();
        }
        */

        if (!isPaused)
            StartCoroutine(FadeMusic(.5f));
        else if (isPaused)
            StartCoroutine(FadeMusic(1));

        IncreaseCoolDownTimer();

    }

    private void DoStats()
    {
        if (playerScript == null)
            return;
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
            case MenuType.Pause:
                pauseMenu.SetActive(true);
                upgradeMenu.SetActive(false);
                break;
            case MenuType.Win:
                GameManager.instance.playerScript.isDead = true; //lol
                winMenu.SetActive(activeState);
                break;
            case MenuType.Lose:
                pauseMenu.SetActive(false);
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
                optionsMenu.SetActive(false);
                tutorial.SetActive(false);
                isPaused = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case MenuType.OptionsMenu:
                pauseMenu.SetActive(false);
                optionsMenu.SetActive(activeState);
                break;
            case MenuType.Tutorial:
                pauseMenu.SetActive(false);
                tutorial.SetActive(activeState);
                break;
            default:
                break;

        }
    }
    public void SetRespawnText(string text)
    {
        respawnText.text = text;
    }

    public void Save()
    {
        GameDataSave.SaveGameData(instance);
        GameDataSave.SavePlayerData(playerScript);
        //scenePath = SceneManager.GetActiveScene().path;
        //EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), scenePath);
    }

    public void Load(/*int sceneNum*/)
    {
        PlayerData data = GameDataSave.LoadPlayerData();

        if (data != null && playerScript != null)
        {
            playerScript.PlayerLoad(data);
            playerScript.UpdatePlayerHPBar();
            playerSpawnLocation = playerScript.ReturnStartCheckpoint();
        }

        // Will or will not be used after deciding if saving should put you back at a checkpoint.
        //playerSpawnLocation.x = data.spawn[0];
        //playerSpawnLocation.y = data.spawn[1];
        //playerSpawnLocation.z = data.spawn[2];
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public int ReturnScene()
    {
        return scene;
    }

    void IncreaseCoolDownTimer()
    {
        for (int i = 0; i < coolDownTracker.Count; ++i)
        {
            coolDownTracker[i] += Time.deltaTime;
        }
    }

    public bool CheckCoolDown(int time)
    {
        bool temp = false;

        if (coolDownTracker[time] >= coolDowns[time])
        {
            temp = true;
            coolDownTracker[time] = 0;
        }

        return temp;
    }

    public void SetCoolDown(AbilitySetup ability)
    {
        coolDowns.Add(ability.abilityCoolDown);
        coolDownTracker.Add(ability.abilityCoolDown);
    }
    public IEnumerator FadeMusic(float targetVolume)
    {
        float time = 0;
        float volume = audioSource.volume;
        float duration = 5.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(volume, targetVolume, time / duration);
            yield return null;
        }
        yield break;
    }
}
