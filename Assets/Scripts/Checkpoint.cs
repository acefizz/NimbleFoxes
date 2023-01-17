using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] bool isStart;
    [SerializeField] bool isEnd;

    public string checkpointName;
    //can be used as a player spawner if player is set to that location originally
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isEnd)
        {
            GameManager.instance.Save();
            GameManager.instance.LoadLevel(GameManager.instance.ReturnScene() + 1);
        }
        else if (other.CompareTag("Player") /*&& isStart*/)
        {
            //Maybe used later
            //GameManager.instance.checkpoint = GameManager.instance.player.transform.position;
            //GameManager.instance.playerSpawnLocation = GameManager.instance.checkpoint;
            //GameManager.instance.checkpointName = checkpointName;

            GameManager.instance.Load();
            GameManager.instance.Save();

            Destroy(gameObject);
        }
    }
}
