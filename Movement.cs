using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Movement : MonoBehaviour
{
    public Animator animator;

    public AudioSource stepSource;
    public AudioClip footstep;
    public float stepInterval = 0.5f;
    private float stepTimer;

    public float moveSpeed = 5f;
    public Transform cameraTransform; // assign in inspector or it'll use Camera.main
    Rigidbody rb;

    public bool dead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Movement requires a Rigidbody on the same GameObject.");
            enabled = false;
            return;
        }

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        stepTimer = stepInterval;
    }

    void FixedUpdate()
    {
        if (dead == false) //TODO
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 forward = (cameraTransform != null) ? cameraTransform.forward : Vector3.forward;
            Vector3 right = (cameraTransform != null) ? cameraTransform.right : Vector3.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 dir = forward * moveZ + right * moveX;
            if (dir.sqrMagnitude > 1f) dir.Normalize();

            Vector3 targetHorizVel = dir * moveSpeed;
            rb.velocity = new Vector3(targetHorizVel.x, rb.velocity.y, targetHorizVel.z);


            if (dir.sqrMagnitude > 0.01f)
            {
                animator.SetBool("Running", true);
                HandleFootsteps(dir.magnitude);
            }
            else
            {
                animator.SetBool("Running", false);
                stepTimer = stepInterval;
            }
        }
    }

    void HandleFootsteps(float speedFactor)
    {
        stepTimer -= Time.fixedDeltaTime;
        if (stepTimer <= 0f)
        {
            PlayFootstep();
            stepTimer = stepInterval / Mathf.Clamp(speedFactor, 0.1f, 1f);
        }
    }

    void PlayFootstep()
    {
        if (stepSource != null && footstep != null)
        {
            stepSource.pitch = Random.Range(0.75f, 0.8f);
            stepSource.PlayOneShot(footstep);
        }
    }
}
