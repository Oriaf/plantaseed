using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergy : MonoBehaviour
{
    
    public float maxHealth;
    private float playerHealth = 1.0f; // Health to display on the health bar
    [SerializeField] private float startHealth;
    [SerializeField] private float damageCost; //The amount of dammage for each hit
    [SerializeField] private float energyGain; //The amount added when collecting energy
    [SerializeField] private float flipCost; 
    public Image healthImg; //Insert the health-bar for the green image that is changed in the script
    public AudioSource audioSource;

    private void Awake()
    {
        playerHealth = startHealth;
        UpdateHealth();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (other.gameObject.CompareTag("Energy"))
        {
            other.GetComponentInParent<EnergySpawn>().spawnNewEnergy(other);

            if(playerHealth >= maxHealth)
            {
                audioSource.Play();
            }
            else
            {
                playerHealth += energyGain; // Add one energy for now
                audioSource.Play();
                UpdateHealth();
            }

        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            playerHealth -= damageCost;
            UpdateHealth();
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

    public void FlipCost()
    {
        playerHealth -= energyGain;
        UpdateHealth();
    }

    public float GetEnergyLevel()
    {
        return playerHealth;
    }

    void UpdateHealth()
    {
        healthImg.fillAmount = playerHealth / maxHealth; // Update health bar on the canvas.
    }
}