using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrackingBullet : MonoBehaviour
{
    [Header("---Components---")]
    [SerializeField] NavMeshAgent agent;

    [Header("---Bullet Stats---")]
    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int timer;

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = speed;
        Destroy(transform.parent.gameObject, timer);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(GameManager.instance.player.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.takeDamage(damage);
        }
        else if (other.CompareTag("Barrel"))
        {
            other.GetComponent<IDamage>().takeDamage(damage);
        }

        if (!other.CompareTag("Explosion"))
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
