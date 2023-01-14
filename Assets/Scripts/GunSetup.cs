using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GunSetup : ScriptableObject
{
    public float shotRate;
    public float shotDamage;
    public int shotDist;
    public string gunName;

    public GameObject GunModel;
    public AudioClip gunShot;


    public float FieldOfView;
    public int pellets;
}
