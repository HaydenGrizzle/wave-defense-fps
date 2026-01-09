using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDmg : MonoBehaviour
{
    public float damagePerSecond = 5f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) // make sure the player has the "Player" tag
        {
            // Damage every frame scaled by deltaTime
            other.GetComponent<PlayerHealth>().TakeLaserDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
