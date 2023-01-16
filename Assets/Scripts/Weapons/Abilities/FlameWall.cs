using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameWall : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    [SerializeField] AbilitySetup setup;
    public float damage;
    [Range(1,5)][SerializeField] float distance;
    [SerializeField] int speed;
    [Range(0, 1)] [SerializeField] float slowDown;
    [SerializeField] int timer;

    // Start is called before the first frame update
    void Start()
    {
        damage = setup.abilityDamage + GameManager.instance.playerScript.GetExtraDamage();
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, timer);
    }

    private void Update()
    {
        rb.velocity = rb.velocity * slowDown;
    }
}
