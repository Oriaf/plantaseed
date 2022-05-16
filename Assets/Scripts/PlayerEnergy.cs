using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerEnergy : MonoBehaviour
{
    
    public float maxHealth;
    private float playerHealth = 1.0f; // Health to display on the health bar
    private float keyHealth = 1.0f;
    [SerializeField] private float startHealth;
    [SerializeField] private float startKey;
    [SerializeField] private float damageCost; //The amount of dammage for each hit
    [SerializeField] private float energyGain; //The amount added when collecting energy
    [SerializeField] private float flipCost; 
    public Image healthImg; //Insert the health-bar for the green image that is changed in the script
    public Image keyImg;
    public AudioSource audioSource;

    private void Awake()
    {
        playerHealth = startHealth;
        keyHealth = startKey;
        UpdateHealth();
        UpdateKey();
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
                keyHealth += energyGain;
                audioSource.Play();
                UpdateHealth();
                UpdateKey();
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
            // start death animation
            playerHealth = startHealth;
            SceneManager.LoadScene("StartMenu");
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


    void UpdateKey()
    {
        keyImg.fillAmount = keyHealth / maxHealth; // Update health bar on the canvas.
    }
}