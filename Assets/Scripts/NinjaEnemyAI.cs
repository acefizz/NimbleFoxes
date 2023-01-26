using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NinjaEnemyAI : MonoBehaviour, IDamage
{
    [Header("---Components--")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject enemyDrop;
    [SerializeField] CapsuleCollider collider;
    [SerializeField] GameObject smokeBomb;
    //public Animator animator;

    [Header("---Enemy Stats---")]
    [SerializeField] float HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int sightAngle;
    [SerializeField] Transform headPos;
    [SerializeField] int roamDist;
    [SerializeField] float disappearSpeed;
    [SerializeField] float disappearTime;

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
    bool isDisappear;


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
        if (playerInRange && !isDying && !isDisappear)
        {
            canSeePlayer();

        }
        else if (agent.remainingDistance < 0.1f && agent.destination != GameManager.instance.player.transform.position && !isDying)
        {
            Roam();
        }

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
        if (NavMesh.SamplePosition(new Vector3(randDir.x, randDir.y, randDir.z), out hit, 1, 1))
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
        agent.SetDestination(GameManager.instance.player.transform.position);

        if(HP > 0)
        {
            StartCoroutine(StartDisappear());
        }

        if (HP <= 0 && !isDying)
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

    public virtual IEnumerator Death()
    {
        //animator.SetTrigger("Death");
        yield return new WaitForSeconds(3.0f);
       
        Destroy(transform.parent.gameObject);
    }

    IEnumerator Disappear()
    {
        enemyUI.SetActive(false);
        model.enabled = false;
        collider.enabled = false;
        isDisappear = true;

        yield return new WaitForSeconds(disappearTime);

        model.enabled = true;
        collider.enabled = true;
        isDisappear = false;
        StartCoroutine(ShowHP());
    }

    IEnumerator StartDisappear()
    {
        agent.isStopped = true;
        enemyUI.SetActive(true);
        Instantiate(smokeBomb, transform.parent.transform.position, transform.parent.transform.rotation, transform.parent.transform);

        yield return new WaitForSeconds(disappearSpeed);

        agent.isStopped = false;
        StartCoroutine(Disappear());
    }
}
