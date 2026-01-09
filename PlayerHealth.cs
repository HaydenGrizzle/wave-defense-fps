using System.Collections;
using System.Collections.Generic;
using MagicPigGames; // AHHHHHHH
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour
{
    public float maxHP = 300;
    public float hp = 300;
    public float armor = 1f; // lower the better
    private bool alive = true;

    public UnityEngine.UI.Image hurtFlash;
    private float flashAlpha;
    public float flashSpeed;

    public ProgressBar progressBar;

    public AudioSource audioSource;
    public AudioClip hurt;

    //-------------
    // UI STUFF
    //-------------
    public GameObject normUI;
    public GameObject gameOverUI;
    CamController camController;
    Movement movement;
    public Rigidbody rb;
    public Gun gun;
    public RageGun rageGun;
    public PekaGun pekaGun;
    public TeslaGun teslaGun;

    void Start()
    {
        //camController = FindObjectOfType<CamController>();
        movement = FindObjectOfType<Movement>();
    }

    void Update()
    {
        if (hurtFlash != null)
        {
            flashAlpha = Mathf.Lerp(flashAlpha, 0f, Time.deltaTime * flashSpeed);
            Color c = hurtFlash.color;
            c.a = flashAlpha;
            hurtFlash.color = c;
        }

        if (progressBar != null)
        {
            float progress = hp / maxHP;
            progressBar.SetProgress(progress);
        }

        if (hp <= 0 && alive)
        {
            Die();
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!alive) return;

        hp -= armor * dmg;
        flashAlpha = 0.7f; // <-- trigger flash when health is lost

        audioSource.pitch = Random.Range(0.90f, 1f);
        audioSource.PlayOneShot(hurt);

        if (hp <= 0)
        {
            Die();
        }
    }

    public void TakeLaserDamage(float dmg)
    {
        if (!alive) return;

        hp -= armor * dmg;
        flashAlpha = 0.7f; // <-- trigger flash when health is lost

        if (hp <= 0)
        {
            Die();
        }
    }

    public void Pay(float cost)
    {
        if (!alive) return;

        hp -= cost;
        flashAlpha = 0.7f;

        audioSource.pitch = Random.Range(0.90f, 1f);
        audioSource.PlayOneShot(hurt);
    }

    public void Die()
    {
        camController = FindObjectOfType<CamController>();

        Debug.Log("DEAD");
        alive = false;
        movement.dead = true;
        normUI.SetActive(false);
        gameOverUI.SetActive(true);
        camController.SetCursorLock(false);

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
        }

        if (gun != null)
        {
            gun.enabled = false;
        }

        if (rageGun != null)
        {
            rageGun.enabled = false;
        }

        if (pekaGun != null)
        {
            pekaGun.enabled = false;
        }

        if (teslaGun != null)
        {
            teslaGun.enabled = false;
        }
    }
}
