using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySpawn : MonoBehaviour
{
    [SerializeField] GameObject[] energy;
    [SerializeField] int visibleAmount;

    private List<int> indexVisable = new List<int>();
    private int rand;

    private void Awake()
    {
        energy = GameObject.FindGameObjectsWithTag("Energy");
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject obj in energy)
        {
            obj.SetActive(false);
        }

        createRandEnergyList();

        foreach (int i in indexVisable)
        {
            energy[i].gameObject.SetActive(true);
        }
    }

    void createRandEnergyList()
    {
        rand = Random.Range(0, energy.Length - 1);

        while (indexVisable.Count < visibleAmount)
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
                indexVisable.Add(rand);
            }
        }

    }

}
