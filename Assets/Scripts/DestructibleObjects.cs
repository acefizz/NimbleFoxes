using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObjects : MonoBehaviour, IDamage
{
    [SerializeField] float health;
    [SerializeField] GameObject destroyedObject;

    public void takeDamage(float dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            Instantiate(destroyedObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
