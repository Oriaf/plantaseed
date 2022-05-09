using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject obj; // the layer game object that the rocks will orbit around
    public float speed; // speed of the borbiting rock
    public bool toogleDirection; 

    void Update()
    {
       changeDirection(); // change direction if the rocks collides with environment object 
    }

    void changeDirection()
    {
        if(!toogleDirection)
        {
            RollBack();
        }
        else 
        {
            RollForward();
        }
    }

    void RollForward()
    {
        toogleDirection = true; 
        transform.RotateAround(obj.transform.position, Vector3.forward, speed * Time.deltaTime);
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }

    void RollBack()
    {
        toogleDirection = false; 
        transform.RotateAround(obj.transform.position, Vector3.back, speed * Time.deltaTime);
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        if(GetComponent<Collider>().tag == "Enemy")
        {
             toogleDirection = toogleDirection ? false : true;
           // Debug.Log("ENEMY" + toogleDirection);
        }
        //toogleDirection = toogleDirection ? false : true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            toogleDirection = toogleDirection ? false : true;
            Debug.Log(toogleDirection);
        }
        ///Debug.Log(toogleDirection);
        
    }
}
