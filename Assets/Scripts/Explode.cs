using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    int targetTime; 
    public GameObject[] stones; 
    public GameObject[] oldStones; 
    
    private int currentIndex = 0;

    // Update is called once per frame
     float elapsedTime = 0f; // Counts up to repeatTime
     float repeatTime = 10f; // Time taken to repeat in seconds
    // Start is called before the first frame update
    void Start()
    {
        
    }

     private void Update()
     {
         explode();
     }

     public void explode()
     {
         elapsedTime += Time.deltaTime;
         if (elapsedTime >= repeatTime)
         {
             // Do something here
              NewRandomObject();
             
             // Subtract repeat time
             elapsedTime -= repeatTime;
         }
     }
    
  

    public void NewRandomObject()
    {
        int newIndex = Random.Range(0, stones.Length);
        // Deactivate old gameobject
        stones[currentIndex].SetActive(false);
        // Activate new gameobject
        Debug.Log(currentIndex);
        currentIndex = newIndex;
        stones[currentIndex].SetActive(true);
        oldStones[currentIndex].SetActive(false);
    }
}
