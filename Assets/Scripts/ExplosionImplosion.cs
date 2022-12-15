using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplosionImplosion : MonoBehaviour
{
    [Header("___| Components |___")]
    [SerializeField] ParticleSystem hitEffect;
    //[SerializeField] GameObject source;
    [SerializeField] List<GameObject> targets;
    [SerializeField] SphereCollider collider;

    [Header("___| Push/Pull Effect |___")]
    [Range(0, 10)] [SerializeField] int pushbackAmount;
    [SerializeField] bool push;

    [Header("___| Damage |___")]
    [Range(0, 10)] [SerializeField] int damage;

    Vector3 pushBack;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet") && !other.CompareTag("Untagged") && !other.isTrigger && !other.GetComponent<CharacterController>())
        {
            targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        targets.Remove(other.gameObject);
    }

    public void Detonate()
    {
        foreach(GameObject target in targets)
        {
            if (target != null)
            {
                Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.yellow);

                RaycastHit hit;

                if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        if (push)
                            GameManager.instance.playerScript.PushbackInput((target.transform.position - transform.position) * pushbackAmount);
                        else
                            GameManager.instance.playerScript.PushbackInput((transform.position - target.transform.position) * pushbackAmount);

                        GameManager.instance.playerScript.takeDamage(damage);
                    }
                    else if (hit.collider.GetComponent<IDamage>() != null)
                    {
                        hit.collider.GetComponent<IDamage>().takeDamage(damage);

                        if (hit.collider.GetComponent<Rigidbody>() != null)
                        {
                            if (push)
                                hit.collider.GetComponent<Rigidbody>().AddExplosionForce(pushbackAmount * 100, transform.position, collider.radius);
                            else
                                hit.collider.GetComponent<Rigidbody>().AddExplosionForce(-pushbackAmount * 100, transform.position, collider.radius);
                        }
                        else if (hit.collider.GetComponent<NavMeshAgent>() != null)
                        {
                            if (push)
                                hit.collider.GetComponent<NavMeshAgent>().velocity = (target.transform.position - transform.position) * pushbackAmount;
                            else
                                hit.collider.GetComponent<NavMeshAgent>().velocity = (transform.position - target.transform.position) * pushbackAmount;
                        }
                    }
                }
            }
        }

        Instantiate(hitEffect, transform.position, transform.rotation);
    }
}
