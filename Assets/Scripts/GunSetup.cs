using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunSetup : ScriptableObject
{
    public float shotRate;
    public int shotDamage;
    public int shotDist;

    public GameObject GunModel;
}
