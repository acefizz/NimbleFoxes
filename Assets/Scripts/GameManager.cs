using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public PlayerController player;
    public PlayerController playerScript;
    public GameObject playerSpawnPos;

    public bool isPaused;
    float timeScaleOriginal;

    [Header("--- UI Menus ---")]
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject playerFlashDamage;

    [Header("--- Upgrade Costs ---")]
    [SerializeField]
    [Range(1, 3)] public int jumpCost = 1;

    //An enum to enforce menu types.
    public enum MenuType { Pause, Win, Lose, PlayerDamageFlash }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        timeScaleOriginal = Time.timeScale;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            PauseGame();
            ShowMenu(MenuType.Pause, isPaused);
        }
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
            case MenuType.PlayerDamageFlash:
                playerFlashDamage.SetActive(activeState);
                break;
            default:
                break;

                //Just in case
                /*
                case "pause":
                    pauseMenu.SetActive(activeState);
                    break;
                case "lose":
                    loseMenu.SetActive(activeState);
                    break;
                case "win":
                    winMenu.SetActive(activeState);
                    break;
                case "playerflash":
                    playerFlashDamageMenu.SetActive(activeState);
                    break;
                default:
                    Debug.LogError("Menu not found");
                    break;
                */

        }
    }
}
