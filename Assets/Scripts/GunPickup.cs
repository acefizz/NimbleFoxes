using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [Header("___| Components |___")]
    [SerializeField] GunSetup gunSetup;

    [Header("___| Effect Settings |___")]
    public float speed;
    public float height;

    private void Update()
    {
        if(speed != 0 && height != 0)
        {
            Vector3 position = transform.position;
            float newY = Mathf.Sin(Time.time * speed);
            transform.position = new Vector3(position.x, Mathf.Abs(newY) * height + 1f, position.z);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.GunPickup(gunSetup);
            Destroy(gameObject);
        }
    }
}
