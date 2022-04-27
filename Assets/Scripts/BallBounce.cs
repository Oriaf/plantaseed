using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 force;
    public Vector3 rotation;
    private Rigidbody rigid;
    private Vector3 rand;
    void Start()
    {
       rigid = GetComponent<Rigidbody>();
       rand = new Vector3(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        rigid.AddForce(Vector3.Scale(force, rand), ForceMode.Impulse);
        transform.Rotate(rotation);
    }
}
