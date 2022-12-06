using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
  [Header("---Components--")]
  [SerializeField] Renderer model;
  [SerializeField] NavMeshAgent agent;

  [Header("---Enemy Stats---")]
  [SerializeField] int HP;
  [SerializeField] int playerFaceSpeed;
  [SerializeField] int sightAngle;
  [SerializeField] Transform headPos;

  [Header("---Enemy Gun Stats")]
  [SerializeField] float shootRate;
  [SerializeField] GameObject bullet;
  [SerializeField] Transform shootPos;

  int HPorg;
  bool isShooting;
  bool playerInRange;
  Vector3 playerDirection;
  float angleToPlayer;

  // Start is called before the first frame update
  void Start()
  {
    HPorg = HP;
    GameManager.instance.UpdateEnemyCount(1);
  }

  // Update is called once per frame
  void Update()
  {
    if (playerInRange)
    {
      canSeePlayer();
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

        if (!isShooting)
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
  public void takeDamage(int dmg)
  {
    HP -= dmg;
    agent.SetDestination(GameManager.instance.player.transform.position);
    StartCoroutine(flashDamage());
    if (HP <= 0)
    {

      GameManager.instance.playerScript.AddCoins(HPorg);
      GameManager.instance.UpdateEnemyCount(-1);
      Destroy(gameObject);
    }
  }
}
