using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] ExplosionImplosion explosion;

    [SerializeField] int speed;


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            explosion.Detonate();
            Destroy(gameObject);
        }
    }
}
