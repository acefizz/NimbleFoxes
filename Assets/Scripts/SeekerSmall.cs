using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class SeekerSmall : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] GameObject effect; 
    //public Animator animator;
    public float HP;
    private void Start()
    {
    }
    private void Update()
    {
        agent.destination = GameManager.instance.player.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Death());
            if (GameManager.instance.playerScript)
                GameManager.instance.playerScript.AddHp(-1);
        }
    }
    public virtual void takeDamage(float damage)
    {
        HP -= damage;
        StartCoroutine(flashDamage());
        if (HP <= 0)
        {
            agent.isStopped = true;
            StartCoroutine(Death());

        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;
    }
    IEnumerator Death()
    {
        //Explode animation
        effect.GetComponent<ParticleSystem>().Play();
        agent.isStopped = true;
        yield return new WaitForSeconds(3.0f);

        effect.GetComponent<ParticleSystem>().Stop();
        Destroy(gameObject);

    }

}
