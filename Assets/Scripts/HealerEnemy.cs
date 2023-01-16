using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealerEnemy : MonoBehaviour, IDamage
{
    [SerializeField] SphereCollider healCollider;
    [SerializeField] GameObject effect;

    private void OnTriggerEnter(Collider other)
    {
        if (healCollider.bounds.Contains(other.transform.position) && other.CompareTag("Enemy"))
        {
            //Instantiate(effect, gameObject.transform);
            if (other.GetComponent<EnemyAI>().ReturnHP() <= (other.GetComponent<EnemyAI>().ReturnOrigHp() - 5))
            {
                other.gameObject.GetComponent<EnemyAI>().AddHP(5);
            }
            else if (other.GetComponent<EnemyAI>().ReturnHP() >= (other.GetComponent<EnemyAI>().ReturnOrigHp() - 5))
            {
                other.gameObject.GetComponent<EnemyAI>().AddHP((other.GetComponent<EnemyAI>().ReturnOrigHp() - other.GetComponent<EnemyAI>().ReturnHP()));
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (healCollider.bounds.Contains(other.transform.position) && other.CompareTag("Enemy"))
        {
            StartCoroutine(HealEnemy(other));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (healCollider.bounds.Contains(other.transform.position) && other.CompareTag("Enemy"))
        {
            StopCoroutine(HealEnemy(other));
        }
    }
    IEnumerator HealEnemy(Collider other)
    {
        yield return new WaitForSeconds(10.0f);
        //Instantiate(effect, gameObject.transform);
        if (other.GetComponent<EnemyAI>().ReturnHP() <= (other.GetComponent<EnemyAI>().ReturnOrigHp()-5))
        {
            other.gameObject.GetComponent<EnemyAI>().AddHP(5);
        }
        else if (other.GetComponent<EnemyAI>().ReturnHP() >= (other.GetComponent<EnemyAI>().ReturnOrigHp() - 5))
        {
            other.gameObject.GetComponent<EnemyAI>().AddHP((other.GetComponent<EnemyAI>().ReturnOrigHp() - other.GetComponent<EnemyAI>().ReturnHP()));
        }
    }
    public virtual void takeDamage(float dmg)
    {
        gameObject.GetComponent<EnemyAI>().takeDamage(dmg);
    }
    
}
