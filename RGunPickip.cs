using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGunPickip : MonoBehaviour
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
                    if (gun.CompareTag("RGun")) // tag your gun properly
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
