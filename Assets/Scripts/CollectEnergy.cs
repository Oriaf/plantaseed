using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectEnergy : MonoBehaviour
{

    public float maxHealth;
    private float playerHealth = 1.0f; // Health to display on the health bar
    public float damageUnit; //The amount of dammage for each delta time 
    public float healthUnit;
    public Image healthImg; //Insert the health-bar for the green image that is changed in the script
    public AudioSource audioSource;

    private void Awake()
    {
        UpdateHealth();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Energy"))
        {
            other.GetComponentInParent<EnergySpawn>().spawnNewEnergy(other);

            if(playerHealth >= maxHealth)
            {
                audioSource.Play();
                Debug.Log("Over maxhealth");
            }
            else
            {
                playerHealth += healthUnit; // Add one energy for now
                audioSource.Play();
                UpdateHealth();
                Debug.Log("Add one energy");
            }

        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            playerHealth -= damageUnit;
            UpdateHealth();
            Debug.Log("Hit by enemy");
        }

    }

    void Update()
    {            
        //Death
        if (playerHealth < 0.0f)
        {
            audioSource.Stop(); // stop audio source if dead. 
        }
    }

    void UpdateHealth()
    {
        healthImg.fillAmount = playerHealth / maxHealth; // Update health bar on the canvas.
    }
}