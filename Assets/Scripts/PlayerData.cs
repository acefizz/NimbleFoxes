using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int lives;
    public int speed;
    public int coins;
    public int maxJumps;
    public int health;
    public int maxHealth;

    //Managing volume sliders
    public float sfxVolume;
    public float musicVolume;
    public float volume; //For the overall volume.

    public int[] guns;
    public int[] abilities;
    public string checkpointName;

    public PlayerData(PlayerController player)
    {
        lives = player.Lives();
        speed = player.GetSpeed();
        coins = player.coins;
        maxJumps = player.GetMaxJumps();
        health = player.GetOriginalHP();
        maxHealth = player.GetOriginalHP();
        guns = new int[player.ReturnGunList().Count];
        abilities = new int[player.ReturnAbilities().Count];
        if(player.checkpointToSpawnAt)
            checkpointName = player.checkpointToSpawnAt.name;

        float sfxVolume = player.sfxVolume;
        float musicVolume = player.musicVolume;
        float volume = player.volume;

        for(int i = 0; i < guns.Length; ++i)
        {
            guns[i] = player.ReturnGunList()[i].gunNum;
        }

        for (int i = 0; i < abilities.Length; ++i)
        {
            abilities[i] = player.ReturnAbilities()[i].abilityNum;
        }
    }
}
