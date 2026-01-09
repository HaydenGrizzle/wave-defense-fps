using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaPickup : MonoBehaviour
{
    public GameObject gunToGive; // Rgun

    void OnTriggerEnter(Collider other)
    {
    if (other.CompareTag("Player"))
        {
            WeaponManager weaponManager = other.GetComponentInChildren<WeaponManager>();
            if (weaponManager != null)
            {
                foreach (GameObject gun in weaponManager.guns)
                {
                    if (gun.CompareTag("TeslaGun")) // tag your gun properly
                    {
                        weaponManager.SwitchGun(gun);
                        break;
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}