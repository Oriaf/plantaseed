using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectEnergy : MonoBehaviour
{

    public float maxHealth = 100.0f;
    private float playerHealth = 100.0f; // Health to display on the health bar
    public float damageUnit = 5.0f; //The amount of dammage for each delta time 
    public float damageDeltaTime = 3.0f;
    public Image healthImg; //Insert the health-bar for the green image that is changed in the script
    private float countdown; // Variable that changes with time. 
    public AudioSource audioSource;
  
   void Awake ()
    {
        countdown = damageDeltaTime;       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Energy"))
        {
            other.GetComponentInParent<EnergySpawn>().spawnNewEnergy(other);
            //Destroy(other.gameObject); //Could be fun to have as part of the game. 
            playerHealth = maxHealth; // TODO: Now it is set to max when energy is collected. Maybe change later?
            audioSource.Play(); 
            UpdateHealth();
            countdown = damageDeltaTime;
        }

    }

    void Update()
    {
            
            countdown -= Time.deltaTime; // Change depending on time that passed
            if (countdown < 0.0f) 
            {
                playerHealth -= damageUnit;
                countdown = damageDeltaTime;
                UpdateHealth();
        }

        if (playerHealth < 0.0f)
            {
                //Debug.Log("Health = 0. DEAD");
                audioSource.Stop(); // stop audio source if dead. 
            }
    }

    void UpdateHealth()
    {
        healthImg.fillAmount = playerHealth / maxHealth; // Update health bar on the canvas.
    }
}