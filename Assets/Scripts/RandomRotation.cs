using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
   
   public Vector3 targetRotation;
    void Start()
    {
        StartCoroutine(LerpFunction(Quaternion.Euler(targetRotation), 5));
    }
    IEnumerator LerpFunction(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = transform.rotation;
        while (time < duration)
        {
            transform.rotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endValue;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Random.rotation;
    }
}
