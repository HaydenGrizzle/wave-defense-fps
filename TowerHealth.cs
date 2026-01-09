using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
{
    PlayerHealth playerHealth;

    public float maxHealth;
    public float health;

    public Image healthFill;

    void Start()
    {
        health = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDmg(float dmg)
    {
        health -= dmg;
        health = Mathf.Clamp(health, 0, maxHealth); // prevent negative health
        UpdateHealthBar();

        if (health <= 0)
        {
            Debug.Log("Tower Down");
            playerHealth.Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthFill != null)
        {
            // 0 = empty, 1 = full
            healthFill.fillAmount = health / maxHealth;
        }
    }
}
