using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] ExplosionImplosion explosion;

    [SerializeField] int speed;
    [SerializeField] float detonateTime;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= detonateTime)
        {
            explosion.Detonate();
            Destroy(gameObject);
        }
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
