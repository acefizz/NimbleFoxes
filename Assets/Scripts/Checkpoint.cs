using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public string checkpointName;
    //can be used as a player spawner if player is set to that location originally
    private void OnTriggerEnter(Collider other)
    {
        
        //TODO:
        GameManager.instance.checkpoint = GameManager.instance.player.transform.position;
        GameManager.instance.playerSpawnLocation = GameManager.instance.checkpoint;
        GameManager.instance.checkpointName = checkpointName;
        //need a way to define levels
        //GameManager.instance.levelCheckpoint = 
        GameManager.instance.gunCheckpoint = GameManager.instance.playerScript.ReturnGunList();
        GameManager.instance.gunSelectCheckpoint = GameManager.instance.playerScript.ReturnSelectedGun();
        //List of anilities and current selection
        GameManager.instance.coinsCheckpoint = GameManager.instance.playerScript.coins;
        //Save the scene as it is (game objects, enemies)

        //need to write this stuff out to a file so I can read it later to load (continue)
        Destroy(gameObject);
    }
}
