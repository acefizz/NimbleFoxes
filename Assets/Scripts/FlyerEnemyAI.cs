using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FlyerEnemyAI : MonoBehaviour, IDamage
{
    [Header("---Components--")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    //public Animator animator;

    [Header("---Enemy Stats---")]
    [SerializeField] float HP;
    [SerializeField] int sightAngle;
    public Transform headPos;
    [SerializeField] int roamDist;
    [SerializeField] float diveSpeed;

    [Header("___| Enemy UI |___")]
    [SerializeField] Image enemyHPBar;
    [SerializeField] GameObject enemyUI;

    bool isDying;
    float HPorg;
    bool playerInRange;
    Vector3 playerDirection;
    float angleToPlayer;
    float stoppingDistOrig;
    Vector3 startPos;
    bool isDiving;

    // Start is called before the first frame update
    public virtual void Start()
    {
        isDying = false;
        HPorg = HP;
        startPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        UpdateEnemyHPBar();
      
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Dive();

        //animator.SetFloat("Speed", agent.velocity.normalized.magnitude);

        if (playerInRange && !isDying)
        {
            CanSeePlayer();
        }
        else if (agent.remainingDistance < 0.1f && agent.destination != GameManager.instance.player.transform.position && !isDying)
        {
            Roam();
        }

        UpdateEnemyHPBar();
    }

    public virtual void CanSeePlayer()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
        playerDirection.y += 1;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(headPos.position, playerDirection, Color.yellow);
        agent.stoppingDistance = stoppingDistOrig;

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                agent.SetDestination(GameManager.instance.player.transform.position);

                if(agent.remainingDistance <= 5)
                {
                    isDiving = true;
                }
                else
                {
                    isDiving = false;
                }
            }
        }
    }

    public virtual void Roam()
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

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public virtual IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;
    }

    public virtual IEnumerator ShowHP()
    {
        enemyUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        enemyUI.SetActive(false);
    }

    public virtual void UpdateEnemyHPBar()
    {
        enemyHPBar.fillAmount = (float)HP / (float)HPorg;

    }

    public virtual void takeDamage(float dmg)
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
            StartCoroutine(Death());
     

        }

    }

    public virtual IEnumerator Death()
    {
        //animator.SetTrigger("Death");
        yield return new WaitForSeconds(3.0f);
       
        Destroy(transform.parent.gameObject);
    }

    void Dive()
    {
        if (isDiving)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 1, 0), Time.deltaTime * diveSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 9.5f, 0), Time.deltaTime * diveSpeed);
        }
    }
}
