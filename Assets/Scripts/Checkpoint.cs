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

        //need to write this stuff out to a file so I can read it later to load (continue)
        Destroy(gameObject);
    }
}
