using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Seeker : MonoBehaviour, IDamage
{
    [SerializeField] GameObject seekers;
    public int toSpawn;
    int spawned = 0;
    public float HP;
    float hpOrig;
    [SerializeField] GameObject enemyUI;
    [SerializeField] Image enemyHPBar;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    //public Animator animator;

    private void Start()
    {
        model.enabled = false;
        GameManager.instance.UpdateEnemyCount(1);
        hpOrig = HP;
    }
    private void OnTriggerEnter(Collider other)
    {
        model.enabled = true;
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SpawnEnemies());
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        StartCoroutine(SpawnEnemies());
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        model.enabled = false;
        if (other.CompareTag("Player"))
            spawned = 0;
        StopCoroutine(SpawnEnemies());
    }
    IEnumerator SpawnEnemies()
    {
        if (spawned <= 0)
        {
            for (int i = 0; i < toSpawn; i++)
            {
                yield return new WaitForSeconds(3.0f);
                Instantiate(seekers, gameObject.transform.position, gameObject.transform.rotation);
                spawned++;
            }
        }
        //yield return new WaitForSeconds(3.0f);
        //Instantiate(seekers, gameObject.transform.position, gameObject.transform.rotation);
    }
    public virtual void takeDamage(float damage)
    {
        HP -= damage;
        UpdateEnemyHPBar();
        StartCoroutine(ShowHP());
        StartCoroutine(flashDamage());
        //spawn the enemy in a different place
        if (HP <= 0)
        {
            enemyUI.SetActive(false);
            agent.isStopped = true;
            StartCoroutine(Death());
            GameManager.instance.UpdateEnemyCount(-1);

        }

    }
    public virtual IEnumerator Death()
    {
        //animator.SetTrigger("Death");
        yield return new WaitForSeconds(3.0f);
        if (GameManager.instance.enemyCount <= 0)
        {
            GameManager.instance.ShowMenu(GameManager.MenuType.Win, true);
        }
        Destroy(gameObject);
    }
    public virtual IEnumerator ShowHP()
    {
        enemyUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        enemyUI.SetActive(false);
    }
    public virtual void UpdateEnemyHPBar()
    {
        enemyHPBar.fillAmount = (float)HP / (float)hpOrig;

    }
    public virtual IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;
    }
}
