using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junk : MonoBehaviour
{
    public ParticleSystem pe;
    public Collider triggerCollider; // assign the trigger collider in Inspector

    public AudioSource poofSource;
    public AudioClip poofClip;

    [Range(0.8f, 1.2f)] public float minPitch = 0.9f;
    [Range(0.8f, 1.2f)] public float maxPitch = 1.1f;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Safer: looks in player and children
            poofSource = player.GetComponentInChildren<AudioSource>();
        }

        if (poofSource == null)
        {
            Debug.LogWarning("⚠️ Junk couldn't find Player's AudioSource!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }

            if (pe != null)
            {
                Instantiate(pe, transform.position, Quaternion.identity);
            }

            if (poofSource != null && poofClip != null)
            {
                poofSource.pitch = Random.Range(minPitch, maxPitch);
                poofSource.PlayOneShot(poofClip, 1.67f); // 67% louder 👅
            }

            transform.localScale = Vector3.one * 0.01f; // super tiny, almost invisible

            StartCoroutine(Shrink());
        }
    }

    IEnumerator Shrink()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
