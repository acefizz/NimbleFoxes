using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilitySetup : ScriptableObject
{
    public float abilityCoolDown;
    public float abilityDamage;
    public string abilityName;

    public GameObject abilityProjectile;
    public AudioClip abilitySound;
}
