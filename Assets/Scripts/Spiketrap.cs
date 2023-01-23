using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiketrap : MonoBehaviour
{
    int damage = 1;

    public Animator spikeTrap;

    // Start is called before the first frame update
    void Start()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            spikeTrap = GetComponent<Animator>();

            StartCoroutine(TriggeredTrap());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        spikeTrap = null;

    }
    public void TrapUp()
    {
            spikeTrap.SetTrigger("open");
            GameManager.instance.playerScript.takeDamage(damage);
        
    }
            
    public void TrapDown()
    {
        spikeTrap.SetTrigger("close"); 
    }
    IEnumerator TriggeredTrap()
    {
      //  yield return new WaitForSeconds(.8f);
        TrapUp();
        //yield return new WaitForSeconds(.05f);
        TrapDown();
        yield return new WaitForSeconds(1);

    }
  
}
