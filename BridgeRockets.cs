using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeRockets : MonoBehaviour
{
    public Transform player;
    public GameObject playerBody;
    public GameObject rocketBody;
    public ParticleSystem explode;
    public float launchSpeed = 20f;
    public float delayBeforeLaunch = 5f;
    public float riseHeight = 1f;
    public float riseDuration = 5f;

    private Rigidbody rb;
    private bool isChasing = false;
    PlayerHealth playerHealth;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        StartCoroutine(RiseThenChase());
    }

    private IEnumerator RiseThenChase()
    {
        float actualRiseTime = Mathf.Min(riseDuration, delayBeforeLaunch);
        Vector3 startPos = rb.position;
        Vector3 targetPos = startPos + Vector3.up * riseHeight;

        rb.isKinematic = true;
        float elapsed = 0f;
        while (elapsed < actualRiseTime)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / actualRiseTime);
            rb.MovePosition(Vector3.Lerp(startPos, targetPos, t));
            elapsed += Time.deltaTime;
            yield return null;
        }
        rb.MovePosition(targetPos);
        rb.isKinematic = false;

        float remainingWait = delayBeforeLaunch - actualRiseTime;
        if (remainingWait > 0f)
            yield return new WaitForSeconds(remainingWait);

        isChasing = true;
    }

    void FixedUpdate()
    {
        if (!isChasing || player == null)
            return;

        // 1. Calculate direction to player
        Vector3 direction = (player.position - rb.position).normalized;

        // 2. Set velocity toward player
        rb.velocity = direction * launchSpeed;

        // 3. Rotate rocket to face player smoothly
        if (direction != Vector3.zero)
        {
            // Target rotation pointing forward (Z axis) toward the player
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate from current rotation to target rotation
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 5f));
            // The "5f" controls turn speed; higher = faster rotation
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            explode.Play();
        }
        playerHealth.Die();

        Destroy(rocketBody);
        Destroy(playerBody);

        this.enabled = false;
    }
}
