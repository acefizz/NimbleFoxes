using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShot : MonoBehaviour, IWeapon
{
    [SerializeField] GunSetup gun;
    
    public void Fire(int damage)
    {
        Transform Muzzle = GameManager.instance.playerScript.muzzlePos;
        ParticleSystem muzzleFlash = GameManager.instance.playerScript.muzzleFlash;
        GameObject effect = GameManager.instance.playerScript.hitEffect;
        
      //  tempMuzzle.Play();
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, gun.shotDist))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                ParticleSystem tempMuzzle = Instantiate(muzzleFlash, Muzzle.transform.position, muzzleFlash.transform.rotation);
                tempMuzzle.Play();
                hit.collider.GetComponent<IDamage>().takeDamage((gun.shotDamage + damage));
            }
            if (effect)
            {

                ParticleSystem tempMuzzle = Instantiate(muzzleFlash, Muzzle.transform.position, muzzleFlash.transform.rotation);
                tempMuzzle.Play();
                GameObject bullet = Instantiate(effect, hit.point, effect.transform.rotation);
                Destroy(bullet, 2f);
            }
               
        }
    }
}
