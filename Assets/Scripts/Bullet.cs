using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int timer;
    [SerializeField] bool isBossBullet;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, GameManager.instance.player.transform.position - transform.position, out hit) && !isBossBullet)
        {
            direction = GameManager.instance.player.transform.position - transform.position;
            direction.y += 1;
            rb.velocity = direction;
        }
        else
        {
            rb.velocity = transform.forward * speed;
        }

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
            GameManager.instance.playerScript.takeDamage(damage); // this is also where it tells me there is an error im assuming because of the player controller error
        }
        else if (other.CompareTag("Barrel"))
        {
            other.GetComponent<IDamage>().takeDamage(damage);
        }

        if (!other.isTrigger || other.CompareTag("Flame Wall"))
        {
            Destroy(gameObject);
        }
    }
}