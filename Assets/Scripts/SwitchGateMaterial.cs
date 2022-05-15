using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGateMaterial : MonoBehaviour
{
    private PlayerEnergy energyScript;
    private LayerSwitch layerScript;
    public Material prison;
    public Material tranparent;

    private void Start()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in player)
        {
            Component temp = p.GetComponent<PlayerEnergy>();
            if (temp != null) energyScript = (PlayerEnergy) temp;
        }

        layerScript = gameObject.GetComponentInParent<LayerSwitch>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(energyScript.GetEnergyLevel());
        if (energyScript.GetEnergyLevel() < layerScript.energyRequired)
        {
            gameObject.GetComponent<MeshRenderer>().material = prison;
            Debug.Log("Here its less then required");
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = tranparent;
            Debug.Log("Here its more");
        }
    }
}
