using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    //======= GAME MANAGER
    // Will or will not be used after deciding if saving should put you back at a checkpoint.
    //public float[] spawn = new float[3];

    public int scene;
    public GameData(GameManager data)
    {
        // Will or will not be used after deciding if saving should put you back at a checkpoint.
        //spawn[0] = data.playerSpawnLocation.x;
        //spawn[1] = data.playerSpawnLocation.y;
        //spawn[2] = data.playerSpawnLocation.z;

        scene = SceneManager.GetActiveScene().buildIndex;
    }
}