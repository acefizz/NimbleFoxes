using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [Header("___| Settings |___")]

    public int health;
    public int coin;
    public bool jump;
    public bool weapon;
    string weaponName;
    public bool ability;
    string abilityName;
    [Range(0, 1)][SerializeField] float pullSpeed;
    public float pullDistance;

    [Header("___| Audio Settings |___")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip upgradeClip;
    [Range(0, 1)][SerializeField] float effectVol;

    [Header("___| Effect Settings |___")]
    public float speed;
    public float height;

    private void Update()
    {
        Vector3 position = transform.position;
        float newY = Mathf.Sin(Time.time * speed);
        transform.position = new Vector3(position.x, Mathf.Abs(newY) * height + 1f, position.z) ;

        if(pullDistance > 0)
        {
            float tempDist = Vector3.Distance(transform.position, GameManager.instance.player.transform.position);
            if (tempDist <= pullDistance)
                transform.position = Vector3.Lerp(transform.position, GameManager.instance.player.transform.position, pullSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.AddHp(health);
            GameManager.instance.playerScript.AddCoins(coin);
            if (upgradeClip != null)
                audioSource.PlayOneShot(upgradeClip, effectVol);
            StartCoroutine(ShowCollections());
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            if (jump)
                GameManager.instance.playerScript.AddJumps(1);
            if (weapon)
                weaponName = GameManager.instance.playerScript.gunName;
            if (ability)
                abilityName = GameManager.instance.playerScript.abilityName;
                //TODO: ability name needs read from object
        }
        
    }
    IEnumerator ShowCollections()
    {
        if (health > 0)
            GameManager.instance.healthDisplay = "+ " + health +"health added";
        if (coin > 0)
            GameManager.instance.coinDisplay = "+ " + coin + " coin(s) added";
        if (weapon)
            GameManager.instance.weaponDisplay = GameManager.instance.playerScript.gunName + " added";
        if (ability)
            GameManager.instance.abiltyDisplay = GameManager.instance.playerScript.abilityName + " added";


        yield return new WaitForSeconds(3.0f);
        GameManager.instance.healthDisplay = "";
        GameManager.instance.coinDisplay = "";
        GameManager.instance.weaponDisplay = "";
        GameManager.instance.abiltyDisplay = "";



        Destroy(gameObject);
    }
}
