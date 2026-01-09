using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] guns;           // all player guns
    private GameObject currentGun;      // currently active gun

    // Expose currentGun for pickups to check
    public GameObject CurrentGun => currentGun;

    void Start()
    {
        if (guns.Length > 0)
        {
            currentGun = guns[0];
            currentGun.SetActive(true);

            // Disable all other guns
            for (int i = 1; i < guns.Length; i++)
                guns[i].SetActive(false);
        }
    }

    public void SwitchGun(GameObject newGun)
    {
        if (currentGun == newGun) return; // already holding this gun

        if (currentGun != null)
            currentGun.SetActive(false);

        currentGun = newGun;
        currentGun.SetActive(true);
    }
}