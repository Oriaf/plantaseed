using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 force;
    void Start()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.AddForce(force, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
