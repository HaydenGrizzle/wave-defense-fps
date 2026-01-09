using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonCode : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject homeUI;
    public GameObject gun;
    public GameObject waves;
    public Movement movement;

    void Start()
    {
        movement = FindObjectOfType<Movement>();
        homeUI.SetActive(true);
        gameUI.SetActive(false);
        gun.SetActive(false);
        waves.SetActive(false);
        movement.dead = true;
    }

    public void Begin()
    {
        homeUI.SetActive(false);
        gameUI.SetActive(true);
        gun.SetActive(true);
        waves.SetActive(true);
        movement.dead = false;
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // stops play mode in Editor
        #else
            Application.Quit(); // quits in build
        #endif
    }
}
