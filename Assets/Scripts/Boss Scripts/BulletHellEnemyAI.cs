using System.Collections;
using UnityEngine;

public class BulletHellEnemyAI : EnemyAI
{

    enum BulletHellState { FireIn8Directions, FireIn4Directions, FireIn2Directions, FireIn1Direction };
    BulletHellState BHS;
    [SerializeField]
    float BHSChangeRate = 5f;

    public IEnumerator MakeDecision()
    {
        //Makes decision on whether or not to change the BHS.
        //If it does change, it will change to a random state.'
        //If it doesn't change, it will continue to shoot in the same direction.


        //Randomly choose bhs state
        yield return new WaitForSeconds(BHSChangeRate);
        int rand = Random.Range(0, 4);
        if(rand == 0)
        {
            BHS = (BulletHellState)Random.Range(0, 4);
        }
    }

    public override IEnumerator shoot()
    {

        isShooting = true;
        animator.SetTrigger("Shoot");

        switch (BHS)
        {
            case BulletHellState.FireIn8Directions:
                for (int i = 0; i < 8; i++)
                {
                    Instantiate(bulletPrefab, shootPos.position, transform.rotation * Quaternion.Euler(0, 45*i, 0));
                    Debug.Log("Firing in 8 directions");
                }
                break;
            case BulletHellState.FireIn4Directions:
                for (int i = 0; i < 4; i++)
                {
                    Instantiate(bulletPrefab, shootPos.position, transform.rotation * Quaternion.Euler(0, 90 * i, 0));
                    Debug.Log("Firing in 4 directions");
                }
                break;
            case BulletHellState.FireIn2Directions:
                for (int i = 0; i < 2; i++)
                {
                    Instantiate(bulletPrefab, shootPos.position, transform.rotation * Quaternion.Euler(0, 180*i, 0));
                    Debug.Log("Firing in 2 directions");
                }
                break;
            case BulletHellState.FireIn1Direction:
                Instantiate(bulletPrefab, shootPos.position, transform.rotation);
                    Debug.Log("Firing in one direction");
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
