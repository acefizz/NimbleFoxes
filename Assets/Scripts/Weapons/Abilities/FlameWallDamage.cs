using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameWallDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IDamage>() != null && !other.isTrigger)
        {
            other.GetComponent<IDamage>().takeDamage(transform.parent.GetComponent<FlameWall>().damage);
        }
    }
}
