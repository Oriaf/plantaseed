using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;
    [Header("Light")]
    public Light lightSource;
    private float defaultRange = 4.0f;
    private float duration = 2.0f;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * speed * Time.deltaTime);

        var amplitude = Mathf.PingPong(Time.time, duration);
        // Transform from 0..duration to 0.5..1 range.
        amplitude = amplitude / duration * 0.5f + 0.5f;

        // Set light range.
        lightSource.range = defaultRange * amplitude;
    }
}
