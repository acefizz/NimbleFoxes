using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShot : MonoBehaviour, IWeapon
{
    [SerializeField] GunSetup gun;

    public void Fire(int damage)
    {
        GameObject effect = GameManager.instance.playerScript.hitEffect;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, gun.shotDist))
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
