using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySpawn : MonoBehaviour
{
    [SerializeField] GameObject[] energy;
    [SerializeField] int visibleAmount;

    private List<int> indexVisable = new List<int>();
    private int rand;
    // Start is called before the first frame update
    void Start()
    {
        //energy = gameObject.GetComponentsInChildren<Collider>(true).gameObject;
        if (visibleAmount >= energy.Length)
        {
            createRandEnergyList();

            foreach (int i in indexVisable)
            {
                energy[i].SetActive(true);
            }
        }
        else
        {
            Debug.Log("Visable amount is larger than amount of energy points");
        }



    }

    void createRandEnergyList()
    {
        rand = Random.Range(0, energy.Length - 1);
        for (int i = 0; i < visibleAmount; i++)
        {
            if (indexVisable.Contains(rand))
            {
                rand = Random.Range(0, energy.Length - 1);
            }
            if (!indexVisable.Contains(rand))
            {
                indexVisable.Add(rand);
                rand = Random.Range(0, energy.Length - 1);
            }
        }
    }

    public void spawnNewEnergy(Collider hit)
    {
        GameObject hitObj = hit.gameObject;
        int hitIndex = System.Array.IndexOf(energy, hitObj);
        //hitObj.SetActive(false);
        rand = Random.Range(0, energy.Length - 1);

        while(hitObj.activeSelf) {
            if (indexVisable.Contains(rand))
            {
                rand = Random.Range(0, energy.Length - 1);
            }
            else
            {
                hitObj.SetActive(false);
                indexVisable.Remove(hitIndex);
                energy[rand].SetActive(true);
                Debug.Log("hej");
                indexVisable.Add(rand);
            }
        }

    }

}
