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


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.AddHp(health);
            GameManager.instance.playerScript.AddJumps(jump);
            GameManager.instance.playerScript.AddCoins(coin);
            
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine(ShowCollections());
        }
        
    }
    IEnumerator ShowCollections()
    {
        audioSource.PlayOneShot(upgradeClip);
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
}
