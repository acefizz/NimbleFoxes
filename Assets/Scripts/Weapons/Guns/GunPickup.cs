using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [Header("___| Components |___")]
    [SerializeField] GunSetup gunSetup;
    [SerializeField] AbilitySetup abilitySetup;
    [SerializeField] bool isAbility;

    
    public void OnTriggerEnter(Collider other)
    {
        if(!GameManager.instance.playerScript)
            return;
        if (other.CompareTag("Player") && isAbility)
        {
            GameManager.instance.playerScript.AbilityPickup(abilitySetup);
        }
        else if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.GunPickup(gunSetup);
            //Destroy(gameObject);
        }
    }
}
