using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : MonoBehaviour, IWeapon
{
    [SerializeField] GunSetup gun;

    public void Fire(int damage)
    {
        GameObject effect = GameManager.instance.playerScript.hitEffect;

        RaycastHit hit;
        for (int i = 0; i < gun.pellets; i++)
        {
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(Random.Range(0.4f, 0.6f), Random.Range(0.4f, 0.6f), 0.0f)), out hit, gun.shotDist))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage((gun.shotDamage + damage));
                }
                if (effect)
                    Instantiate(effect, hit.point, effect.transform.rotation);
            }
        }
    }
}
