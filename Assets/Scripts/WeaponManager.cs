using UnityEngine;
using Unity;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponHolder;
    public Weapon CurrentWeapon { get; private set; }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (CurrentWeapon != null)
        {
            Destroy(CurrentWeapon.gameObject);
        }

        GameObject weaponInstance = Instantiate(weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);
        CurrentWeapon = weaponInstance.GetComponent<Weapon>();

        if (CurrentWeapon == null)
        {
            Debug.LogError("Equipped item is not a weapon!");
            Destroy(weaponInstance);
        }
    }

    public void UnequipWeapon()
    {
        if (CurrentWeapon != null)
        {
            Destroy(CurrentWeapon.gameObject);
            CurrentWeapon = null;
        }
    }
}