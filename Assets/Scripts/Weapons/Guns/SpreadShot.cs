using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : MonoBehaviour, IWeapon
{
    [SerializeField] GunSetup gun;

    public void Fire(int damage)
    {
        GameObject muzzle = GameManager.instance.playerScript.muzzlePos;
        ParticleSystem muzzleFlash = GameManager.instance.playerScript.muzzleFlash;
        GameObject effect = GameManager.instance.playerScript.hitEffect;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, gun.shotDist))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().takeDamage((gun.shotDamage + damage));
            }
        }

        for (int i = 0; i < gun.pellets; i++)
        {
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(Random.Range(0.4f, 0.6f), Random.Range(0.4f, 0.6f), 0.0f)), out hit, gun.shotDist))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage((gun.shotDamage + damage));
                }
                if (effect)
                {
                    ParticleSystem tempMuzzle = Instantiate(muzzleFlash, muzzle.transform.position, muzzleFlash.transform.rotation);
                    tempMuzzle.Play();
                    GameObject bullet = Instantiate(effect, hit.point, effect.transform.rotation);
                    Destroy(bullet, 2f);
                    Destroy(tempMuzzle, 1f);
                }
                   

            }
        }
    }
}
