using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] int timer;
    [SerializeField] Transform spawnPos;
   
    bool isSpawning;
    bool playerInRange; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !isSpawning)
        {
            StartCoroutine(spawn());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

        }
    }
    IEnumerator spawn()
    {
        isSpawning = true;
       
        Instantiate(enemy, spawnPos.position, enemy.transform.rotation);
       
        yield return new WaitForSeconds(timer);    
        isSpawning = false;
    }
}
