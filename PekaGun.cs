using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PekaGun : MonoBehaviour
{
    public TextMeshProUGUI ammoText;

    public int maxBullets = 10;
    public int bullets = 10;
    private bool isReloading;

    float[] pitchOptions = new float[] { 0.75f, 0.80f, 0.85f, 0.90f, 0.95f, 1f };
    float[] pitchOptions2 = new float[] { 0.75f, 0.80f, 0.85f, 0.90f, 0.95f, 1f  };


    public AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioSource audioScource3;

    public AudioClip gunClick;
    public AudioClip gunShot;
    public AudioClip reload;

    public Animator reloadAnimator;
    public Animator slide;

    public float dmg = 10f;
    public float dmgBuff = 0f;
    public float range = 10000f;
    public float fireRate = 5f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public ParticleSystem muzzleFlash2;
    public GameObject flash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    void OnEnable()
    {
        StartCoroutine(ReloadGun());
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && bullets > 0 && isReloading == false) //check this
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if(Input.GetButton("Fire1") && Time.time >= nextTimeToFire && bullets == 0) //check this
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            audioSource.PlayOneShot(gunClick);
        }

        if (Input.GetKeyDown(KeyCode.R)) //Check
        {
            StartCoroutine(ReloadGun());
        }
    }

    void Shoot()
    {
        bullets -= 1;
        ammoText.text = bullets + "/" + maxBullets;

        float randomPitch = pitchOptions[Random.Range(0, pitchOptions.Length)];
        audioSource.pitch = randomPitch;
        audioSource.PlayOneShot(gunShot);
        StartCoroutine(PlayParticleTail(muzzleFlash));

        slide.SetTrigger("Shoot");

        StartCoroutine(PlayParticleTail(muzzleFlash2));
        StartCoroutine(Wait());



        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(dmg + dmgBuff);
            }

            RangedEnemy rangedEnemy = hit.transform.GetComponent<RangedEnemy>();
            if (rangedEnemy != null)
            {
                rangedEnemy.TakeDamage(dmg + dmgBuff);
            }

            WinCondition winCondition = hit.transform.GetComponent<WinCondition>();
            if (winCondition != null)
            {
                winCondition.TakeDamage(dmg + dmgBuff);
            }

            ExplosiveEnemy explosiveEnemy = hit.transform.GetComponent<ExplosiveEnemy>();
            if (explosiveEnemy != null)
            {
                explosiveEnemy.TakeDamage(dmg + dmgBuff);
            }

            Divine divine = hit.transform.GetComponent<Divine>();
            if (divine != null)
            {
                divine.TakeDamage(dmg + dmgBuff);
            }

            KingsVessle kingsVessle = hit.transform.GetComponent<KingsVessle>();
            if (kingsVessle != null)
            {
                kingsVessle.TakeDamage(dmg + dmgBuff);
            }

            DmgUp dmgUp = hit.transform.GetComponent<DmgUp>();
            if (dmgUp != null)
            {
                dmgUp.IncreaseDmg();
            }

            DefUp defUp = hit.transform.GetComponent<DefUp>();
            if (defUp != null)
            {
                defUp.IncreaseDef();
            }

            BoxDrop boxDrop = hit.transform.GetComponent<BoxDrop>();
            if (boxDrop != null)
            {
                boxDrop.DropItem();
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
        //------------------
        // ^ Shoot Detect ^
        //------------------
    }

    IEnumerator Wait()
    {
        flash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        flash.SetActive(false);
    }

    IEnumerator PlayParticleTail(ParticleSystem ps)
    {
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        // jump to 0.9s into its simulation
        ps.Simulate(0.9f, true, true);

        // play the remaining time
        ps.Play();

        // wait for the last 0.1s of its lifetime
        yield return new WaitForSeconds(0.1f);

        ps.Stop();
    }

    IEnumerator ReloadGun()
    {
        if (isReloading) yield break;

        reloadAnimator.SetTrigger("Reload");
        float randomPitch2 = pitchOptions2[Random.Range(0, pitchOptions.Length)];
        audioScource3.pitch = randomPitch2;
        audioScource3.PlayOneShot(reload);

        isReloading = true;
        yield return new WaitForSeconds(1f);
        ammoText.text = maxBullets + "/" + maxBullets;
        bullets = maxBullets;
        isReloading = false;
    }
}
