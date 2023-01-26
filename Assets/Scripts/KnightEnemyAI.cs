using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class KnightEnemyAI : MonoBehaviour, IDamage
{
    [Header("---Components--")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject enemyDrop;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject parent;

    [Header("---Enemy Stats---")]
    [SerializeField] float HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int sightAngle;
    [SerializeField] Transform headPos;
    [SerializeField] int roamDist;

    [Header("---Enemy Weapon Stats---")]
    [SerializeField] float swingSpeed;

    [Header("---Enemy UI---")]
    [SerializeField] Image enemyHPBar;
    [SerializeField] GameObject enemyUI;

    bool isDying;
    float HPorg;
    bool playerInRange;
    Vector3 playerDirection;
    float angleToPlayer;
    float stoppingDistOrig;
    Vector3 startPos;
    bool isBashing;
    Vector3 shieldStartPos;

    // Start is called before the first frame update
    void Start()
    {
        isDying = false;
        HPorg = HP;
        startPos = transform.position;
        shieldStartPos = shield.transform.localPosition;
        stoppingDistOrig = agent.stoppingDistance;
        UpdateEnemyHPBar();
      
    }

    // Update is called once per frame
    void Update()
    {
        Bash();

        if (playerInRange && !isDying)
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

                if (!agent.isStopped && HP > 0 && agent.remainingDistance <= agent.stoppingDistance)
                {
                    StartCoroutine(SwingDown());
                }
            }
        }

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.isStopped)
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
        parent.transform.rotation = Quaternion.Lerp(parent.transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
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

        if (HP <= 0)
        {
            enemyUI.SetActive(false);
            agent.isStopped = true;
            isDying = true;

            if (enemyDrop != null)
            {
                Instantiate(enemyDrop, transform.position, transform.rotation);
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

    IEnumerator SwingDown()
    {
        isBashing = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(swingSpeed);

        isBashing = false;
        agent.isStopped = false;
    }

    void Bash()
    {
        if (isBashing)
        {
            shield.transform.localPosition = Vector3.Lerp(shield.transform.localPosition, new Vector3(0, 1.5F, 3), Time.deltaTime * swingSpeed);
        }
        else
        {
            shield.transform.localPosition = Vector3.Lerp(shield.transform.localPosition, new Vector3(0, 1.5F, .25F), Time.deltaTime * swingSpeed);
        }
    }
}
