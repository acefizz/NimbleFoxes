using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{

    //======= PLAYER
    int HP;
    int lives;
    int playerSpeed;
    int maxJumps;
    int coins;
    //======= GAME MANAGER
    string scenePath;
    float spawnLocX; 
    float spawnLocY;
    float spawnLocZ;

    int scene;
    public GameData()
    {

    }
    public void SaveData()
    {
        HP = GameManager.instance.playerScript.ReturnHP();
        PlayerPrefs.SetInt("HP", HP);

        lives = GameManager.instance.playerScript.Lives();
        PlayerPrefs.SetInt("lives", lives);

        playerSpeed = GameManager.instance.playerScript.GetSpeed();
        PlayerPrefs.SetInt("playerSpeed", playerSpeed);

        maxJumps = GameManager.instance.playerScript.GetMaxJumps();
        PlayerPrefs.SetInt("maxJumps", maxJumps);

        coins = GameManager.instance.playerScript.coins;
        PlayerPrefs.SetInt("coins", coins);

        scenePath = GameManager.instance.scenePath;
        PlayerPrefs.SetString("scenePath", scenePath);

        spawnLocX = GameManager.instance.playerSpawnLocation.x;
        spawnLocY = GameManager.instance.playerSpawnLocation.y;
        spawnLocZ = GameManager.instance.playerSpawnLocation.z;
        PlayerPrefs.SetFloat("spawnX", spawnLocX);
        PlayerPrefs.SetFloat("spawnY", spawnLocY);
        PlayerPrefs.SetFloat("spawnZ", spawnLocZ);

        scene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("scene", scene);
        
    }
    public void LoadData()
    {
        HP = PlayerPrefs.GetInt("HP");
        GameManager.instance.playerScript.SetHP(HP);

        lives = PlayerPrefs.GetInt("lives");
        GameManager.instance.playerScript.SetLives(lives);

        playerSpeed = PlayerPrefs.GetInt("playerSpeed");
        GameManager.instance.playerScript.SetSpeed(playerSpeed);

        maxJumps = PlayerPrefs.GetInt("maxJumps");
        GameManager.instance.playerScript.SetMaxJumps(maxJumps);

        coins = PlayerPrefs.GetInt("coins");
        GameManager.instance.playerScript.coins = coins;

        scenePath = PlayerPrefs.GetString("scenePath");
        GameManager.instance.scenePath = scenePath;

        spawnLocX = PlayerPrefs.GetFloat("spawnX");
        spawnLocY = PlayerPrefs.GetFloat("soawnY");
        spawnLocZ = PlayerPrefs.GetFloat("spawnZ");
        GameManager.instance.playerSpawnLocation.x = spawnLocX;
        GameManager.instance.playerSpawnLocation.y = spawnLocY;
        GameManager.instance.playerSpawnLocation.z = spawnLocZ;

        scene = PlayerPrefs.GetInt("scene");

    }

}
