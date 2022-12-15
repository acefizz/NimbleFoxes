using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamage
{
    [SerializeField] ExplosionImplosion explosion;
    [SerializeField] MeshCollider collider;
    [SerializeField] float health;

    float maxhealth;
    // Start is called before the first frame update
    void Start()
    {
        maxhealth = (int)health;
    }

    public void takeDamage(float dmg)
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
