using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("---Components--")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject enemyDrop;
    public Animator animator;

    [Header("---Enemy Stats---")]
    [SerializeField] float HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int sightAngle;
    public Transform headPos;
    [SerializeField] int roamDist;

    [Header("---Enemy Gun Stats")]
    public float shootRate;
    public GameObject bulletPrefab;
    public Transform shootPos;

    [Header("___| Enemy UI |___")]
    [SerializeField] Image enemyHPBar;
    [SerializeField] GameObject enemyUI;
    [SerializeField] TextMeshProUGUI bossNameText;
    public string bossName;

    bool isDying;
    float HPorg;
    public bool isShooting;
    bool playerInRange;
    Vector3 playerDirection;
    float angleToPlayer;
    float stoppingDistOrig;
    Vector3 startPos;

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
        animator.SetFloat("Speed", agent.velocity.normalized.magnitude);
        if (playerInRange && !isDying)
        {
            CanSeePlayer();
            if (!isShooting) { StartCoroutine(shoot()); }

        }
        else if (agent.remainingDistance < 0.1f && agent.destination != GameManager.instance.player.transform.position && !isDying)
        {
            Roam();
        }
        UpdateEnemyHPBar();


    }
    public virtual void FacePlayer()
    {
        playerDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
    }

    public virtual void CanSeePlayer()
    {
        playerDirection = GameManager.instance.player.transform.position - headPos.position;
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

                if (!isShooting && HP > 0)
                {
                    StartCoroutine(shoot());
                }
            }
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            FacePlayer();
        }
    }

    public virtual void Roam()
    {
        agent.stoppingDistance = 0;
        Vector3 randDir = Random.insideUnitSphere * roamDist;
        randDir += startPos;
        NavMeshHit hit;
        NavMesh.SamplePosition(new Vector3(randDir.x, 0, randDir.z), out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();
        //Check if hit is valid
        if (hit.position != null)
        {
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
    public virtual IEnumerator shoot()
    {
        isShooting = true;
        animator.SetTrigger("Shoot");
        if (bulletPrefab.GetComponent<NavMeshAgent>() != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(shootPos.position, out hit, 10f, NavMesh.AllAreas))
            {
                Instantiate(bulletPrefab, hit.position, transform.rotation);
            }
        }
        else
        {
            Instantiate(bulletPrefab, shootPos.position, transform.rotation);
        }
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
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
    public void AddHP(float amount)
    {
        HP += amount;
        UpdateEnemyHPBar();
    }
    public float ReturnHP()
    {
        return HP;
    }
    public float ReturnOrigHp()
    {
        return HPorg;
    }
    public virtual void UpdateEnemyHPBar()
    {
        enemyHPBar.fillAmount = (float)HP / (float)HPorg;

    }
    public virtual void takeDamage(float dmg)
    {
        HP -= dmg;
        if (CompareTag("Boss"))
        {
            enemyUI.SetActive(true);
            bossNameText.text = bossName;
        }
        else
        {
            StartCoroutine(ShowHP());
        }
        UpdateEnemyHPBar();
        agent.SetDestination(GameManager.instance.player.transform.position);
        StartCoroutine(flashDamage());
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
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(3.0f);
        //if (GameManager.instance.enemyCount <= 0)
        //{
        //    GameManager.instance.ShowMenu(GameManager.MenuType.Win, true);
        //}

        Destroy(gameObject);
    }
}
