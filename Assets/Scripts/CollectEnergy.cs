using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectEnergy : MonoBehaviour
{
    public float maxHealth = 100.0f;
    private float playerHealth = 100.0f;
    public float damageUnit = 5.0f;
    public float damageDeltaTime = 3.0f;
    public Image healthImg; //Insert the health-bar for the green image that is changed in the script
    private float countdown;
    public AudioSource audioSource;
  
   void Start ()
    {
        countdown = damageDeltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Energy"))
        {
            //Destroy(other.gameObject); //Could be fun to have as part of the game. 
            playerHealth = maxHealth;
            audioSource.Play();
            UpdateHealth();
            countdown = damageDeltaTime;
        }

    }

    void Update()
    {
            countdown -= Time.deltaTime;
            if (countdown < 0.0f) 
            {
                playerHealth -= damageUnit;
                countdown = damageDeltaTime;
                UpdateHealth();
        }

        if (playerHealth < 0.0f)
            {
                //Debug.Log("Health = 0. DEAD");
                audioSource.Stop();
            }
    }

    void UpdateHealth()
    {
        healthImg.fillAmount = playerHealth / maxHealth;
    }
}