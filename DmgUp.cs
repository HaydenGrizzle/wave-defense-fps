using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DmgUp : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Gun gun;
    public RageGun rageGun;
    public PekaGun pekaGun;
    public TeslaGun teslaGun;
    public float price = 45f;
    public float reward = 2f;

    public void IncreaseDmg()
    {
        Debug.Log("Dmg increased");
        playerHealth.Pay(price);
        gun.dmgBuff += reward;
        rageGun.dmgBuff += reward;
        pekaGun.dmgBuff += reward;
        teslaGun.dmgBuff += reward;
    }
}
