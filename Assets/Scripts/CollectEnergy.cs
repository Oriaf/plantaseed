using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CollectEnergy : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public Text timeText;
    
    public AudioSource audioSource;
    void Start()
    {
        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        timeRemaining = 10;
        timerIsRunning = true;
        audioSource.Play();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

                
            }
            else
            {
                Debug.Log("Time has run out and you are dead");
                timeRemaining = 0;
                timerIsRunning = false;

                audioSource.Stop();
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        //timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText.text = seconds.ToString();
    }
}