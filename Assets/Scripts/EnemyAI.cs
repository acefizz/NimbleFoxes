using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("---Components--")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject enemyDrop;
    [SerializeField] Animator animator;

    [Header("---Enemy Stats---")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int sightAngle;
    [SerializeField] Transform headPos;
    [SerializeField] int roamDist;

    [Header("---Enemy Gun Stats")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    [Header("___| Enemy UI |___")]
    [SerializeField] Image enemyHPBar;
    [SerializeField] GameObject enemyUI;

    int HPorg;
    bool isDying = false;
    bool isShooting;
    bool playerInRange;
    Vector3 playerDirection;
    float angleToPlayer;
    float stoppingDistOrig;
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        HPorg = HP;
        startPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        UpdateEnemyHPBar();
        GameManager.instance.UpdateEnemyCount(1);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.normalized.magnitude);
        if (playerInRange && !isDying)
        {
            canSeePlayer();
            if(!isShooting) { StartCoroutine(shoot()); }
            
        }
        else if (agent.remainingDistance < 0.1f && agent.destination != GameManager.instance.player.transform.position && !isDying)
        {
            Roam();
        }
    }
    void canSeePlayer()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDirection, Color.yellow);

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
        NavMesh.SamplePosition(new Vector3(randDir.x, 0, randDir.z), out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();
        if(hit.position != null)
            agent.CalculatePath(hit.position, path);
        agent.SetPath(path);
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
        animator.SetTrigger("Shoot");
        Instantiate(bullet, shootPos.position, transform.rotation);
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
    public void takeDamage(int dmg)
    {
        HP -= dmg;
        UpdateEnemyHPBar();
        StartCoroutine(ShowHP());
        agent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(flashDamage());
        if (HP <= 0)
        {
            agent.isStopped = true;
            isDying = true;
            isShooting = false;
            if (enemyDrop != null)
            {
                Instantiate(enemyDrop, shootPos.position, transform.rotation);
            }
            
            GameManager.instance.UpdateEnemyCount(-1);
            StartCoroutine(Death());
        }
    }
    IEnumerator Death()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(3.0f);
        if (GameManager.instance.enemyCount <= 0)
        {
            GameManager.instance.ShowMenu(GameManager.MenuType.Win, true);
        }
        Destroy(gameObject);
    }
}
