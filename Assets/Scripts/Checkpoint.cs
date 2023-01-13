using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public string checkpointName;
    //can be used as a player spawner if player is set to that location originally
    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.checkpoint = GameManager.instance.player.transform.position;
        GameManager.instance.playerSpawnLocation = GameManager.instance.checkpoint;
        GameManager.instance.checkpointName = checkpointName;

        GameManager.instance.SaveScene();
        Destroy(gameObject);
    }
}
