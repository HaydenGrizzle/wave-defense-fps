using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public float dmg;
    private bool hasHit = false;

    public void ResetHit()
    {
        hasHit = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit == true) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(dmg);
        }
    }

}
