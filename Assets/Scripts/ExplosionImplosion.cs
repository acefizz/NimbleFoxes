using Palmmedia.ReportGenerator.Core.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionImplosion : MonoBehaviour
{
    [SerializeField] int pushbackAmount;
    [SerializeField] bool push;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] GameObject source;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (push)
                GameManager.instance.playerScript.PushbackInput((other.transform.position - source.transform.position) * pushbackAmount);
            else
                GameManager.instance.playerScript.PushbackInput((source.transform.position - other.transform.position) * pushbackAmount);
            Instantiate(hitEffect, source.transform.position, source.transform.rotation);
        }
        Destroy(gameObject);
    }
}
