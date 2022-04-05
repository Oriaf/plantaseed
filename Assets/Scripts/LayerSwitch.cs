using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSwitch : MonoBehaviour
{
    private SphereCollider layerCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        layerCollider = GetComponentInParent<SphereCollider>();
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
            Debug.Log(this.name + ": " + other.gameObject.name + " tagged with " + other.tag);
            layerCollider.enabled = false;
        }
    }
}
