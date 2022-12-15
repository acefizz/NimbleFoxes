using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] int enemiesToSpawn;
    [SerializeField] int timer;
    [SerializeField] Transform spawnPos;
   
    bool isSpawning;
    bool playerInRange;
    int enemiesSpawned;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !isSpawning && enemiesSpawned < enemiesToSpawn)
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
        enemiesSpawned++;
        yield return new WaitForSeconds(timer);    
        isSpawning = false;
    }
}
