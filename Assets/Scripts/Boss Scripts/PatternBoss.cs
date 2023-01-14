using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternBoss : EnemyAI
{
    enum PatternBossState { Teleport, Fire }

    PatternBossState PBS;
    float PBSChangeRate = 15f;

    [SerializeField]
    GameObject[] teleportLocations;

    public IEnumerator MakeDecision()
    {
        //Randomly choose bhs state
        yield return new WaitForSeconds(PBSChangeRate);
        int rand = Random.Range(0, 4);
        if (rand == 0)
        {
            PBS = PatternBossState.Teleport;
        }
         else
        {
            PBS = PatternBossState.Fire;
        }
    }

    void Shoot()
    {
        for (int i = 0; i < 16; i++)
            Instantiate(bulletPrefab, shootPos.position, transform.rotation * Quaternion.Euler(0, 45/2 * i, 0));
    }

    public override IEnumerator shoot()
    {

        isShooting = true;
        animator.SetTrigger("Shoot");

        switch (PBS)
        {
            case PatternBossState.Fire:
                Shoot();
                break;
            case PatternBossState.Teleport:
                //Teleport to a teleport location game object at random
                int rand = Random.Range(0, teleportLocations.Length);
                transform.position = teleportLocations[rand].transform.position;
                Shoot();
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    public override void Update()
    {
        base.Update();
        StartCoroutine(MakeDecision());
    }

}
