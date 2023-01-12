using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelfDestruct : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.GetComponent<CharacterController>())
        {
            GameManager.instance.playerScript.takeDamage(damage);

            parent.GetComponent<IDamage>().takeDamage(9999);
        }
    }
}
