using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WinCondition : MonoBehaviour
{
    public TowerHealth towerHealth;

    public BoxCollider swordCollider;

    public bool attacking = false;

    public float health = 50f;
    public float chaseRange = 1000f; // how close player has to be before chasing
    public float attackRange = 50f;

    public Transform tower;

    private NavMeshAgent agent;

    public ParticleSystem splat; // FX

    public Animator animator;

    public AudioSource audioSource;
    public AudioClip deathSFX;
    public AudioSource footstepAudioSource;
    public AudioClip footstep;
    public float stepInterval = 0.35f;

    private float stepTimer;

    public GameObject healthPot;

    public float swingDur;
    public float windUp;

    public float towerDmg;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (animator != null)
        {
            animator.Play("Walk", 0, Random.Range(0f, 1f)); // Animation Offset
        }

        stepTimer = stepInterval;

        if (footstepAudioSource != null)
        {
            footstepAudioSource.spatialBlend = 1f;          // fully 3D
            footstepAudioSource.rolloffMode = AudioRolloffMode.Linear;
            footstepAudioSource.minDistance = 1f;          // full volume nearby
            footstepAudioSource.maxDistance = 20f;         // fades out at distance
        }

        if (towerHealth == null)
        {
            towerHealth = FindObjectOfType<TowerHealth>();
        }
    }

    void Update()
    {
        if (attacking && tower != null)
        {
            Vector3 lookPos = tower.position - transform.position;
            lookPos.y = 0; // keep only horizontal rotation
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }

        //----------------
        // ROTATION STUFF
        //----------------

        if (tower == null) return;

        float distance = Vector3.Distance(transform.position, tower.position);

        if (distance <= chaseRange && !attacking)
        {
            Vector3 direction = (tower.position - transform.position).normalized;
            Vector3 targetPos = tower.position - direction * 3.3f; // 1 unit away from player
            agent.SetDestination(targetPos);
        }

        else
        {
            agent.ResetPath(); // stop moving if player is too far
        }


        if (distance <= attackRange && !attacking)
        {
            StartCoroutine(Attack());
        }

        HandleFootsteps();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(splat, transform.position, Quaternion.identity);


        // Create a temporary GameObject for the death sound
        GameObject tempAudio = new GameObject("DeathSound");
        tempAudio.transform.position = transform.position;
        AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
        tempSource.clip = deathSFX;
        tempSource.spatialBlend = 1f; // 3D sound
        tempSource.rolloffMode = AudioRolloffMode.Linear; // optional
        tempSource.minDistance = 1f; // sound starts fading at 1 unit
        tempSource.maxDistance = 20f; // sound inaudible after 20 units
        // Randomize pitch
        tempSource.pitch = Random.Range(0.8f, 1.2f); // tweak range to taste (0.8-1.25 for bigger variation)
        // Optional: randomize volume slightly
        // tempSource.volume = Random.Range(0.9f, 1f);
        tempSource.Play();
        // Destroy the temp object after the clip finishes at the changed pitch (or at least 3 seconds)
        float playDuration = Mathf.Max(deathSFX.length / tempSource.pitch, 3f);
        // Destroy the temp GameObject after the clip finishes or 3 seconds, whichever is longer
        Destroy(tempAudio, Mathf.Max(deathSFX.length, 3f));
        //--------------------
        // ^^ DEATH AUDIO ^^
        //-------------------

        PotChance();

        Destroy(gameObject);
    }

    IEnumerator Attack()
    {
        attacking = true;

        agent.isStopped = true;

        animator.SetTrigger("Attack");

        swordCollider.enabled = true;

        swordCollider.GetComponent<HurtBox>().ResetHit();

        yield return new WaitForSeconds(swingDur);
        towerHealth.TakeDmg(towerDmg);
        yield return new WaitForSeconds(windUp); 

        swordCollider.enabled = false;

        agent.isStopped = false;

        attacking = false;
    }

    void HandleFootsteps()
    {
        Vector3 horizontalVelocity = new Vector3(agent.velocity.x, 0, agent.velocity.z);
        float speed = horizontalVelocity.magnitude;

        if (speed > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval / Mathf.Clamp(speed / agent.speed, 0.1f, 1f);
            }
        }
        else
        {
            stepTimer = stepInterval;
        }
    }

    void PlayFootstep()
    {
        if (footstep != null && footstepAudioSource != null)
        {
            footstepAudioSource.pitch = Random.Range(0.55f, 0.65f);
            footstepAudioSource.PlayOneShot(footstep);
        }
    }

    void PotChance()
    {
        float chance = Random.Range(0f, 100f); // 0 <= chance < 100
    if (chance < 2f) // 50% chance
    {
        if (healthPot != null)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 1f;
            Quaternion randomRotation = Quaternion.Euler(
                Random.Range(0f, 360f),  // X rotation
                Random.Range(0f, 360f),  // Y rotation
                Random.Range(0f, 360f)   // Z rotation
            );
            Instantiate(healthPot, spawnPos, randomRotation);
        }
    }
    }
}