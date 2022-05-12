using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour

{
    public GameObject ControllsIns = null;
    public GameObject EntranceIns = null;
    public GameObject GravityIns = null;
    public GameObject EnergyIns = null;

    public void Start()
    {
        ControllsIns.SetActive(false);
        EntranceIns.SetActive(false);
        GravityIns.SetActive(false);
        EnergyIns.SetActive(false);

        StartCoroutine(showTextFuntion());
    }
    

    IEnumerator showTextFuntion()
{
        yield return new WaitForSeconds(1);
        ControllsIns.SetActive(true);
        yield return new WaitForSeconds(5);
        ControllsIns.SetActive(false);

        yield return new WaitForSeconds(2);
        EntranceIns.SetActive(true);
        yield return new WaitForSeconds(5);
        EntranceIns.SetActive(false);

        yield return new WaitForSeconds(2);
        GravityIns.SetActive(true);
        yield return new WaitForSeconds(5);
        GravityIns.SetActive(false);

        yield return new WaitForSeconds(2);
        EnergyIns.SetActive(true);
        yield return new WaitForSeconds(5);
        EnergyIns.SetActive(false);
        

}
}


