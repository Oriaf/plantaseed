using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private LayerSwitch layerScript;
    public GameObject gameOverCanvas;

    private void Awake()
    {
        playerHealth = startHealth;
        keyHealth = startKey;
        UpdateHealth();
        UpdateKey();
        layerScript = gameObject.GetComponentInParent<LayerSwitch>();
    }

    private void checkDeath()
    {
        if(playerHealth < 0.1f)
        {
            gameOverCanvas.SetActive(true);
            audioSource.Stop(); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        if (other.gameObject.CompareTag("Energy"))
        {
            collectEnergy(other);
        }

        if (other.gameObject.CompareTag("Key"))
        {
            collectKey(other);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            takeDamage();
        }

        checkDeath();

    }

    void collectEnergy(Collider other)
    {
        other.GetComponentInParent<EnergySpawn>().spawnNewEnergy(other);

        if (playerHealth >= maxHealth)
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

    void collectKey(Collider other)
    {
        Destroy(other.gameObject);
        if (keyHealth > 0)
        {
            keyHealth += 1;
            UpdateKey();
        }

    }

    void takeDamage()
    {
        if (playerHealth > 0)
        {
            playerHealth -= damageCost;
            UpdateHealth(); }
    }
    public float GetEnergyLevel()
    {
        return playerHealth;
    }

    public float GetKeyLevel()
    {
        return keyHealth;
    }

    void UpdateHealth()
    {
        healthImg.fillAmount = playerHealth / maxHealth; // Update health bar on the canvas.
    }


    void UpdateKey()
    {
        float t = layerScript.energyRequired;
        t = t != 0 ? t : 1;
        keyImg.fillAmount = keyHealth / t; // Update health bar on the canvas.
    }

    public void FlipCost()
    {
        playerHealth -= 1;
        UpdateHealth();
        checkDeath();
    }
}