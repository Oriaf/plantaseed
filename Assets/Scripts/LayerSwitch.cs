using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class LayerSwitch : MonoBehaviour
{
    [HideInInspector]
    private Collider layerCollider;

    [Header("Level")]
    public int energyRequired = 8;
    public bool useGameOrder = false;
    public string nextScenePath = "Scenes/StartMenu";
    private PlayerEnergy energyScript;
    private bool animate = false;
    private bool Loading = false;

    [Header("Animation")]
    public float TurnSpeed = 1.0f; 
    public float MoveSpeed = 1.0f;
    public float SinkSpeed = 1.0f;
    public Camera view;
    private Transform playerTransform;
    private Rigidbody playerRigid;
    private PlayerMovement moveScript;
    private CameraJoystick camScript;
    private Vector3 target;
    private int stage = 0;


    // Start is called before the first frame update
    void Start()
    {
        layerCollider = GetComponentInParent<MeshCollider>();
        
        //Find the player so we can check relevant conditions for level completion
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        playerTransform = player[0].transform; //TODO: Potential race condition
        foreach (GameObject p in player)
        {
            Component temp = p.GetComponent<PlayerEnergy>();
            if (temp != null) energyScript = (PlayerEnergy) temp;
            temp = p.GetComponent<Rigidbody>();
            if (temp != null) playerRigid = (Rigidbody) temp;
            temp = p.GetComponent<PlayerMovement>();
            if (temp != null) moveScript = (PlayerMovement) temp;
        }

        GameObject cam = GameObject.FindGameObjectWithTag("Camera");
        camScript = cam.GetComponent<CameraJoystick>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Game logic update (Animation)
    private void FixedUpdate()
    {
        float delta = Time.deltaTime;

        /*if (animate)
        {
            switch (stage)
            {
                case 0:
                    turnTowardsGate(delta);
                    break;
                case 1:
                    walkToMiddle(delta);
                    break;
                case 2:
                    sinkIntoGround(delta);
                    break;
                default:
                    levelTransition();
                    break;
            } 
        }*/
    }

    //Triggered when a collider enters the layer
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !animate)
        {
            //Debug.Log(this.name + ": " + other.gameObject.name + " tagged with " + other.tag);

            Debug.Log(energyScript.GetKeyLevel() + ", " + energyRequired);
            if (energyScript.GetKeyLevel() >= energyRequired)
            {
                animate = true;
                stage = 0;
                camScript.enabled = false;
                moveScript.enabled = false;
                playerRigid.velocity = new Vector3(0, 0, 0);
                target = Vector3.negativeInfinity;
                levelTransition();
            }
        }
    }

    private void turnTowardsGate(float delta)
    {
        //Find the direction in the forward-right plane of the player to the center of the trigger
        Vector3 targetForward = Vector3.Normalize(Vector3.ProjectOnPlane(transform.position - playerTransform.position, transform.up));
        //Debug.Log(targetForward + ", " + playerTransform.forward + ", " + delta);
        
        //Check if we are sufficiently close to looking at the center of the trigger
        if (Vector3.SqrMagnitude(targetForward - playerTransform.forward) < 0.00001)
        {
            stage++;
            target = Vector3.negativeInfinity;
            return;
        }

        //Rotate with a constant speed towards the center of the trigger
        Vector3 forward = Vector3.RotateTowards(playerTransform.forward, targetForward, TurnSpeed * delta, 0.0f);
        playerTransform.rotation = Quaternion.LookRotation(forward, playerTransform.up);
    }

    private void walkToMiddle(float delta)
    {
        //Calculate where the center of the trigger is
        Vector3 dir = (transform.position - playerTransform.position);
        Vector3 verticalOffset = dir * Mathf.Sin(Vector3.SignedAngle(dir, playerTransform.forward, playerTransform.up));
        target = transform.position + verticalOffset;
        
        //Check if the player has reached the middle of the trigger
        if (playerTransform.position == target) 
        {
            stage++;
            target = Vector3.negativeInfinity;
            return;
        }
       
        //Move to the center of the trigger
        playerTransform.position = Vector3.MoveTowards(playerTransform.position, target, MoveSpeed * delta);
    }

    private void sinkIntoGround(float delta)
    {
        //Calculate the point beneath the trigger to move to
        if (target.Equals(Vector3.negativeInfinity))
        {
            target = playerTransform.position - playerTransform.up * 1.5f;
        }
        
        
        //Check if the player has reached the middle of the trigger
        if (playerTransform.position == target) 
        {
            stage++;
            target = Vector3.negativeInfinity;
            return;
        }
        
        //Move to the center of the trigger
        playerTransform.position = Vector3.MoveTowards(playerTransform.position, target, SinkSpeed * delta);
    }
    private void levelTransition()
    {
        //Make sure we only load the next level once
        if (Loading) return;
        Loading = true;
        
        //Transition to the next level
        if (useGameOrder)
        {
            //Load the next scene as specified by the game order (looping around at the end)
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadSceneAsync((current.buildIndex + 1) % SceneManager.sceneCount);
        }
        else
        {
            //Load the specified custom scene
            SceneManager.LoadSceneAsync(nextScenePath);
        }
    }
}
