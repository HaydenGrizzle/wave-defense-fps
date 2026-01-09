using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDmg : MonoBehaviour
{
    public float dps = 20f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(dps * Time.deltaTime);
            }
        }
    }
}
