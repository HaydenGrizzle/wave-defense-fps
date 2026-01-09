using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public GameObject rocket;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rocket.SetActive(true);
        }
    }
}
