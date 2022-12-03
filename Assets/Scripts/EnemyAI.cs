using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour // ,IDamage        need to include IDamage.
{
    [Header("---Components--")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("---Enemy Stats---")]
    [SerializeField] int HP;

    [Header("---Enemy Gun Stats")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    int HPorg;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        HPorg = HP;
    }

    // Update is called once per frame
    void Update()
    {
      //  agent.SetDestination(GameManager.instance.player.transform.position);
      // need GameManager for this so that the AI can track the player.
        if (!isShooting)
        {
            StartCoroutine(shoot());
        }
    }
    IEnumerator shoot()
    { 
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;
    }
    public void takeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(flashDamage());
        if (HP <= 0)
        {
            //GameManager.instance.playerScript.addCoins(HPorg);
            //"Left this marked out because I need GameManager and uncomment if we need it for our upgrades"- FVF
            
        
            Destroy(gameObject);
        }
    }
}
