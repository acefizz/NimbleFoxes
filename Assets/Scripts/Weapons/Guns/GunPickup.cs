using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [Header("___| Components |___")]
    [SerializeField] GunSetup gunSetup;

    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.GunPickup(gunSetup);
            //Destroy(gameObject);
        }
    }
}
