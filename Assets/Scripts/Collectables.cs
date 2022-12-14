using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI jumpText;
    [SerializeField] TextMeshProUGUI coinText;

    public int health;
    public int jump;
    public int coin;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip upgradeClip;

    float objectX;
    float objectY;
    float objectZ;
    
    private void Start()
    {
        objectX= gameObject.transform.position.x;
        objectY = gameObject.transform.position.y;
        objectZ = gameObject.transform.position.z;
    }

    private void Update()
    {
        StartCoroutine(Float());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(upgradeClip);
            GameManager.instance.playerScript.AddHp(health);
            GameManager.instance.playerScript.AddJumps(jump);
            GameManager.instance.playerScript.AddCoins(coin);
            
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine(ShowCollections());
        }
        
    }
    IEnumerator ShowCollections()
    {
        
        UI.SetActive(true);

        if (health > 0)
            healthText.text = "+" + health + " health";
        if (jump > 0)
            jumpText.text = "+" + jump + " jumps";
        if (coin > 0)
            coinText.text = "+" + coin + " coins";


        yield return new WaitForSeconds(3.0f);
        UI.SetActive(false);
        Destroy(gameObject);
    }
    IEnumerator Float()
    {
        gameObject.transform.position.Set(objectX, objectY + 1, objectZ);
        new WaitForSeconds(0.3f);
        gameObject.transform.position.Set(objectX, objectY, objectZ);
        new WaitForSeconds(0.3f);
        gameObject.transform.position.Set(objectX, objectY - 1, objectZ);
        new WaitForSeconds(0.3f);
        gameObject.transform.position.Set(objectX, objectY, objectZ);
        yield return new WaitForSeconds(0.3f);
    }
}
