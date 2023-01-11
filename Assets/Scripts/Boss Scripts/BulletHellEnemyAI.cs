using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHellEnemyAI : EnemyAI
{

    enum BulletHellState { FireIn8Directions, FireIn4Directions, FireIn2Directions, FireIn1Direction };

    public override IEnumerator shoot()
    {
        var BHS = BulletHellState.FireIn8Directions;

        //randomly choose bhs state
        int rand = Random.Range(0, 4);

        Debug.Log(rand);

        //Randomly cast to BHS
        BHS = (BulletHellState)rand;

        isShooting = true;
        animator.SetTrigger("Shoot");

        switch (BHS)
        {
            case BulletHellState.FireIn8Directions:
                for (int i = 0; i < 8; i++)
                {
                    Instantiate(bulletPrefab, shootPos.position, transform.rotation * Quaternion.Euler(0, 45*i, 0));
                }
                break;
            case BulletHellState.FireIn4Directions:
                for (int i = 0; i < 4; i++)
                {
                    Instantiate(bulletPrefab, shootPos.position, transform.rotation * Quaternion.Euler(0, 90 * i, 0));
                }
                break;
            case BulletHellState.FireIn2Directions:
                for (int i = 0; i < 2; i++)
                {
                    Instantiate(bulletPrefab, shootPos.position, transform.rotation * Quaternion.Euler(0, 180*i, 0));
                }
                break;
            case BulletHellState.FireIn1Direction:
                Instantiate(bulletPrefab, shootPos.position, transform.rotation);
                break;
            default:
                break;
        }


        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

}
