using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleDespawn : MonoBehaviour
{
    [SerializeField] float despawnTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Despawn());
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);

        Destroy(gameObject);
    }
}
