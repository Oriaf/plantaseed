using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;
    [Header("Effect")]
    public GameObject afterEffect;
    private Light lightSource;
    private float defaultRange = 4.0f;
    private float duration = 2.0f;

    void Start(){
        lightSource = gameObject.GetComponentInChildren<Light>();
    }
    // Update is called once per frame
    void Update()
    {
        // Rotates the sphere of the energy ball
        transform.Rotate(rotation * speed * Time.deltaTime);

        // Incrament and decrament of the amplitude of range
        var amplitude = Mathf.PingPong(Time.time, duration);
        // Transform from 0..duration to 0.5..1 range.
        amplitude = amplitude / duration * 0.5f + 0.5f;

        // Set light range to make it flicker. 
        lightSource.range = defaultRange * amplitude;
    }

    public void SetAfterEffect(){
        Instantiate(afterEffect, transform.position, transform.rotation);
    }
}
