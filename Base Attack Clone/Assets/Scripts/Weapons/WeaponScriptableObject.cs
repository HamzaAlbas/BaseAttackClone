using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/WeaponData")]
public class WeaponScriptableObject : ScriptableObject
{
    public string weaponName;
    public int explosionRadius;
    public int explosionForce;
    public GameObject effectPrefab;
    public float weaponDamage;
}
