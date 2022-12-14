using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamage
{
    [SerializeField] ExplosionImplosion explosion;
    [SerializeField] MeshCollider collider;
    [SerializeField] int health;

    int maxhealth;
    // Start is called before the first frame update
    void Start()
    {
        maxhealth = health;
    }

    public void takeDamage(int dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            collider.enabled = false;
            explosion.Detonate();
            Destroy(gameObject);
        }
    }
}