using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LayerSwitch : MonoBehaviour
{
    [HideInInspector]
    private Collider layerCollider;

    [Header("Level")]
    public int energyRequired = 8;
    public bool useGameOrder = false;
    public string nextScenePath = "Scenes/StartMenu";

    // Start is called before the first frame update
    void Start()
    {
        layerCollider = GetComponentInParent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            //Debug.Log(this.name + ": " + other.gameObject.name + " tagged with " + other.tag);
            
            //Transition to the next level
            if (useGameOrder)
            {
                //Load the next scene as specified by the game order (looping around at the end)
                Scene current = SceneManager.GetActiveScene();
                SceneManager.LoadSceneAsync((current.buildIndex + 1) % SceneManager.sceneCount);
            }
            else
            {
                //Load the specified custom scene
                SceneManager.LoadSceneAsync(nextScenePath);
            }
        }
    }
}
