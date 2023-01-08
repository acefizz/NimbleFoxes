using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [Header("___| Settings |___")]

    public int health;
    public int coin;
    public bool weapon;
    public string weaponName;
    public bool ability;
    public string abilityName;

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
        }
        
    }
    IEnumerator ShowCollections()
    {
        if (health > 0)
            GameManager.instance.healthDisplay = $"+ {health} health added";
        if (coin > 0)
            GameManager.instance.coinDisplay = $"+ {coin} coin(s) added";
        if (weapon)
            GameManager.instance.weaponDisplay = $"{weaponName} added";
        if (ability)
            GameManager.instance.abiltyDisplay = $"{abilityName} added";


        yield return new WaitForSeconds(3.0f);
        GameManager.instance.healthDisplay = "";
        GameManager.instance.coinDisplay = "";
        GameManager.instance.weaponDisplay = "";
        GameManager.instance.abiltyDisplay = "";



        Destroy(gameObject);
    }
}
