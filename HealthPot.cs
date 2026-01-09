using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPot : MonoBehaviour
{
    public AudioSource playerAudio;
    public AudioClip crush;
    public AudioClip up;
    public Collider box;

    private PlayerHealth playerHealth;

    void Start()
    {
        // Automatically find the PlayerHealth component in the scene
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(playerHealth.hp == playerHealth.maxHP) { return; }
            box.enabled = false;

            if (playerHealth.hp <= 250)
            {
                playerHealth.hp += 50;
            }
            else
            {
                playerHealth.hp = playerHealth.maxHP;
            }

            playerAudio.PlayOneShot(crush);

            // Shrink to invisible
            transform.localScale = Vector3.zero;

            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.15f);
        playerAudio.PlayOneShot(up);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
