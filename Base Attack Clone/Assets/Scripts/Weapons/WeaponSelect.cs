using UnityEngine;

public class WeaponSelect : MonoBehaviour
{
    public static WeaponSelect Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectWeapon(WeaponScriptableObject weapon)
    {
        WeaponManager.Instance.EquipWeapon(weapon);
    }
}
