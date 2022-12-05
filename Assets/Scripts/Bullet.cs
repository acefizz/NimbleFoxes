using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rig;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int timer;


    // Start is called before the first frame update
    void Start()
    {
        rig.velocity = transform.forward * speed;
        Destroy(gameObject, timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        //    GameManager.instance.playerScript.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
