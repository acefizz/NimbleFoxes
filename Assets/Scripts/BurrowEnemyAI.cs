using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BurrowEnemyAI : MonoBehaviour, IDamage
{
    [Header("---Components--")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject enemyDrop;
    [SerializeField] GameObject parent;

    [Header("---Enemy Stats---")]
    [SerializeField] float HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int sightAngle;
    [SerializeField] Transform headPos;
    [SerializeField] int roamDist;
    [SerializeField] float burrowSpeed;

    [Header("---Enemy Gun Stats---")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    [Header("---Enemy UI---")]
    [SerializeField] Image enemyHPBar;
    [SerializeField] GameObject enemyUI;

    bool isDying;
    float HPorg;
    bool isShooting;
    bool playerInRange;
    Vector3 playerDirection;
    float angleToPlayer;
    float stoppingDistOrig;
    Vector3 startPos;
    bool isBurrowing;


    // Start is called before the first frame update
    void Start()
    {
        isDying = false;
        HPorg = HP;
        startPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        UpdateEnemyHPBar();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.isStopped)
        {
            isBurrowing = false;
        }
        else
        {
            isBurrowing = true;
        }

        if (playerInRange && !isDying)
        {
            canSeePlayer();

            if(agent.remainingDistance <= agent.stoppingDistance * .5)
            {
                isBurrowing = true;
            }
            else if (agent.remainingDistance <= agent.stoppingDistance)
            {
                StartCoroutine(StopStartAgent());
            }
        }
        else if (agent.remainingDistance < 0.1f && agent.destination != GameManager.instance.player.transform.position && !isDying)
        {
            Roam();
        }

        Burrow();
        UpdateEnemyHPBar();
    }

    void canSeePlayer()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        playerDirection.y += 1;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

        agent.stoppingDistance = stoppingDistOrig;

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (!isShooting && HP > 0)
                {
                    StartCoroutine(shoot());
                }
            }
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            facePlayer();
        }

    }

    void Roam()
    {
        agent.stoppingDistance = 0;
        Vector3 randDir = Random.insideUnitSphere * roamDist;
        randDir += startPos;
        NavMeshHit hit;

        //Check if hit is valid
        if (NavMesh.SamplePosition(new Vector3(randDir.x, 0, randDir.z), out hit, 1, 1))
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(hit.position, path);
            agent.SetPath(path);
        }
    }

    void facePlayer()
    {
        playerDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        if (bullet.GetComponent<NavMeshAgent>() != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(shootPos.position, out hit, 10f, NavMesh.AllAreas))
            {
                Instantiate(bullet, hit.position, transform.rotation);
            }
        }
        else
        {
            Instantiate(bullet, shootPos.position, transform.rotation);
        }

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        model.material.color = Color.white;
    }

    IEnumerator ShowHP()
    {
        enemyUI.SetActive(true);

        yield return new WaitForSeconds(5f);

        enemyUI.SetActive(false);
    }

    void UpdateEnemyHPBar()
    {
        enemyHPBar.fillAmount = (float)HP / (float)HPorg;
    }

    public void takeDamage(float dmg)
    {
        HP -= dmg;
        UpdateEnemyHPBar();
        StartCoroutine(ShowHP());
        agent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(flashDamage());
        isBurrowing = true;

        if (HP <= 0)
        {
            enemyUI.SetActive(false);
            agent.isStopped = true;
            isDying = true;
            isShooting = false;

            if (enemyDrop != null)
            {
                Instantiate(enemyDrop, shootPos.position, transform.rotation);
            }
            StartCoroutine(Death());
           
        }
    }

    IEnumerator Death()
    {
        //animator.SetTrigger("Death");
        yield return new WaitForSeconds(3.0f);
       
        Destroy(parent);
    }

    public void Burrow()
    {
        if (isBurrowing)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, -1, 0), Time.deltaTime * burrowSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, 0), Time.deltaTime * burrowSpeed);
        }
    }

    IEnumerator StopStartAgent()
    {
        if (!agent.isStopped)
        {
            agent.isStopped = true;

            yield return new WaitForSeconds(burrowSpeed * 2);

            agent.isStopped = false;
        }
    }
}
