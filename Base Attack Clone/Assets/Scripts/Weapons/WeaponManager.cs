using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [HideInInspector] public WeaponScriptableObject weapon;
    public static WeaponManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void EquipWeapon(WeaponScriptableObject weaponData)
    {
        weapon = weaponData;
    }
}
