using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Respawn();
        }
    }
    public void Respawn()
    {
        GameManager.instance.playerScript.ReturnController().enabled= false;
        transform.position =  GameManager.instance.playerSpawnLocation;
        GameManager.instance.playerScript.ReturnController().enabled = true;
    }
}
