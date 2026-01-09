using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefUp : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float cost = 50f;
    public float gift = 0.002f;

    public void IncreaseDef()
    {
        Debug.Log("Armor increased");
        playerHealth.armor -= gift;
        playerHealth.armor = Mathf.Max(0.05f, playerHealth.armor); // never go below 0.05
        playerHealth.Pay(45f);
    }
}
