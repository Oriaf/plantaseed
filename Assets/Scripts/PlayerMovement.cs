using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public enum WorldState
    {
        Grounded, //on ground
        InAir, //in the air
    }

    [HideInInspector]
    public WorldState States;
    private Transform Cam;
    private Transform CamY;
    //public ControlsPivot AxisPivot;
    public Joystick joystick;

    private DetectCollision Colli;
    [HideInInspector]
    public Rigidbody Rigid;

    float delta;

    [Header("Physics")]
    public Transform[] GroundChecks;
    public float DownwardPush; //what is applied to the player when on a surface to stick to it
    public float GravityAmt;    //how much we are pulled downwards when we are on a wall
    public float GravityBuildSpeed; //how quickly we build our gravity speed
    private float ActGravAmt; //the actual gravity applied to our character

    public LayerMask GroundLayers; //what layers the ground can be
    public float GravityRotationSpeed = 10f; //how fast we rotate to a new gravity direction

    [Header("Stats")]
    public float Speed = 15f; //max speed for basic movement
    public float Acceleration = 4f; //how quickly we build speed
    public float turnSpeed = 2f;
    private Vector3 MovDirection, movepos, targetDir, GroundDir; //where to move to

    [Header("Jumps")]
    public float JumpAmt;
    private bool HasJumped;
    private CameraJoystick CamScript;

    [Header("Sounds")]
    public AudioClip gravityFlip;
    private PlayAudio audioScript;
    
    //Input
    [Header("Phone Input")]
    public GameObject FlipButton;
    public float GyroSensitivty = 2.0f;
    public float FlipCooldownTime = 0.5f;
    private Gyroscope gyroscope;
    private bool flip = false;
    private bool flipCooldown = false;
    private float currentColdownTime = 0.0f;

    // Start is called before the first frame update
    void Awake()
    {
        Rigid = GetComponentInChildren<Rigidbody>();
        Colli = GetComponent<DetectCollision>();
        GameObject Camera = GameObject.FindGameObjectWithTag("Camera");
        CamScript = Camera.GetComponent<CameraJoystick>();
        GroundDir = transform.up; //Get the y-axis (green axis) of the player
        SetGrounded(); //Start by being connected to the ground
        
        Cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        CamY = Cam.transform.parent.transform;
        
        //detatch rigidbody so it can move freely 
        Rigid.transform.parent = null;
        
        //Use the phone sensors
        if (SystemInfo.supportsGyroscope)
        {
            gyroscope = Input.gyro;
            gyroscope.enabled = true;
        }
        else
        {
            Debug.Log("Warning: Gyroscope not available on this device"); 
            gyroscope = null; 
            FlipButton.SetActive(true);
        }
        
        audioScript = GetComponent<PlayAudio>();
    }

    private void Update()   //inputs
    {
        //Update the position of the character
        transform.position = Rigid.position;

        //check for jumping
        if (gyroscope != null)
        {
            Vector3 rotAcceleration = new Vector3(0, 0, 0);
            rotAcceleration = gyroscope.rotationRateUnbiased;
            //gyroscopeAcceleration.text = rotAcceleration.ToString();
            if (rotAcceleration.x > GyroSensitivty && !flipCooldown)
            {
                flip = true;
                flipCooldown = true;
                currentColdownTime = 0.0f;
            }
            else if (flipCooldown)
            {
                currentColdownTime += Time.deltaTime;
                if (currentColdownTime > FlipCooldownTime)
                {
                    flipCooldown = false;
                }
            }
                
        }
        if (Input.GetButtonDown("Jump") || flip)
        {
            flip = false;
            SwitchGravity();
        }

    }

    // Update is called once per frame
    void FixedUpdate()  //world movement
    {
        //Time from last frame
        delta = Time.deltaTime;

        if (States == WorldState.Grounded)
        {
            //targetUp = transform.up;
            
            float Spd = Speed;

            bool noMoveLaptop = Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0;
            bool noMoveJoystick = joystick.Horizontal == 0 && joystick.Vertical == 0;
            if (noMoveLaptop && noMoveJoystick)
            {
                //we are not moving, linear interpolate to a walk speed
                Spd = 0f;
            }
           
            //Update position based on input and "ground gravity"
            MoveSelf(delta, Spd, Acceleration);

            //Check if the character is still on ground
            bool Ground = Colli.CheckGround(-GroundDir);
            if (!Ground)
            {
                SetInAir();
            }

        }
        else if (States == WorldState.InAir)
        {        
            if (HasJumped) //only return back to ground once jump state is over
                return;

            //Update the jump based on the current "ground gravity"
            FallingCtrl(delta, Speed, Acceleration);

            //Check if the character is still in air
            bool Ground = Colli.CheckGround(-GroundDir);
            if (Ground)
            {
                SetGrounded();
                CamScript.InAir(false);
            }
        }

        if(transform.position.x > 45 || 
        transform.position.x < -45 || 
        transform.position.y > 45 || 
        transform.position.y < -45 || 
        transform.position.z > 45 || 
        transform.position.z < -45)
        {
            PlayerEnergy script = GetComponent<PlayerEnergy>();
            script.induceDeath();
        }
    }

    public void SetFlip()
    {
        flip = true;
    }
    
    //transition to ground
    public void SetGrounded()
    {
        ActGravAmt = 5f; //our gravity is returned to normal

        States = WorldState.Grounded;
    }
    //transition to air
    void SetInAir()
    {
        States = WorldState.InAir;
    }
    //jump up
    IEnumerator JumpUp(float UpwardsAmt)
    {
        HasJumped = true;

        Rigid.velocity = Vector3.zero;

        SetInAir();

        //Get pushed relatively up by a force proportional to UpwardsAmt
        if (UpwardsAmt != 0)
            Rigid.AddForce((transform.up * UpwardsAmt), ForceMode.Impulse);

        ActGravAmt = 0;

        yield return new WaitForSecondsRealtime(0.3f);
        HasJumped = false;
    }

    /*
     * Switch the gravity of the character
     */
    void SwitchGravity()
    {
        PlayerEnergy energyScript = gameObject.GetComponent<PlayerEnergy>();
        if (energyScript.GetEnergyLevel() > 0) {
            audioScript.playClip(gravityFlip);
            Rigid.AddForce(transform.up * JumpAmt, ForceMode.Impulse);
            this.transform.RotateAround(this.transform.position, this.transform.right, 180);
            // energyScript.FlipCost();
            CamScript.InAir(true);
            //SetInAir();
        }
        else
        {
            //Player cant jump
            Debug.Log("Can't jump, too low energy");
        }

    }

    //check the angle of the floor we are stood on
    Vector3 FloorAngleCheck()
    {
        RaycastHit HitFront;
        RaycastHit HitCentre;
        RaycastHit HitBack;

        //Send out a ray from three specified points, downwards, to see where they hit the ground
        Physics.Raycast(GroundChecks[0].position, -GroundChecks[0].transform.up, out HitFront, 10f, GroundLayers);
        Physics.Raycast(GroundChecks[1].position, -GroundChecks[1].transform.up, out HitCentre, 10f, GroundLayers);
        Physics.Raycast(GroundChecks[2].position, -GroundChecks[2].transform.up, out HitBack, 10f, GroundLayers);
         
        //Align the direction to the ground based on the ray test
        Vector3 HitDir = transform.up;
        if (HitFront.transform != null)
        {
            HitDir += HitFront.normal;
        }
        if (HitCentre.transform != null)
        {
            HitDir += HitCentre.normal;
        }
        if (HitBack.transform != null)
        {
            HitDir += HitBack.normal;
        }

        //Debug.DrawLine(transform.position, transform.position + (HitDir.normalized * 5f), Color.red);

        return HitDir.normalized;
    }
    
    //move our character
    void MoveSelf(float d, float Speed, float Accel)
    {
        float laptopH = Input.GetAxis("Horizontal");
        float laptopV = Input.GetAxis("Vertical");
        float joyH = joystick.Horizontal;
        float joyV = joystick.Vertical;
        
        float _xMov = joyH != 0 ? joyH : laptopH;
        float _zMov = joyV != 0 ? joyV : laptopV;
        
        bool MoveInput = false;

        //Get the axis for the Cameras' coordinate system
        Vector3 screenMovementForward = CamY.transform.forward;
        Vector3 screenMovementRight = CamY.transform.right;

        //Calculate the character's speed in each direction of the camera
        Vector3 h = screenMovementRight * _xMov;
        Vector3 v = screenMovementForward * _zMov;

        //Calculate the resulting direction vector that the character will move along
        Vector3 moveDirection = (v + h).normalized;

        //Check for input and set targetDir
        if (_xMov == 0 && _zMov == 0)
        {
            targetDir = transform.forward;
        }
        else
        {
            targetDir = moveDirection;
            MoveInput = true;
        }

        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }

        //Create a Rotation transformation that rotates the character so that targetDir is forward
        Quaternion lookDir = Quaternion.LookRotation(targetDir);
        float TurnSpd = turnSpeed;

        //Find the desired direction vector to the ground and interpolate towards it
        Vector3 SetGroundDir = FloorAngleCheck();
        //Debug.Log("Ground Dir: " + SetGroundDir);
        GroundDir = Vector3.Lerp(GroundDir, SetGroundDir, d * GravityRotationSpeed);
        //Debug.Log("Ground Dir: " + GroundDir);

        //lerp mesh slower when not on ground
        RotateSelf(SetGroundDir, d, GravityRotationSpeed);
        RotateMesh(d, targetDir, TurnSpd);

        //move character
        float Spd = Speed;
        Vector3 curVelocity = Rigid.velocity;

        if (!MoveInput) //if we are not pressing a move input we move towards velocity //or are crouching
        {
            Spd = Speed * 0.8f; //less speed is applied to our character
            MovDirection = Vector3.Lerp(transform.forward, Rigid.velocity.normalized, 12f * d);
        }
        else
        {
            MovDirection = transform.forward;
        }

        Vector3 targetVelocity = MovDirection * Spd;

        //push downwards in downward direction of mesh ("gravity")
        targetVelocity -= SetGroundDir * DownwardPush;

        Vector3 dir = Vector3.Lerp(curVelocity, targetVelocity, d * Accel);
        Rigid.velocity = dir;
    }

    //move once we are in air
    void FallingCtrl(float d, float Speed, float Accel)
    {
        //control our direction slightly when falling
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 screenMovementForward = CamY.transform.forward;
        Vector3 screenMovementRight = CamY.transform.right;

        Vector3 h = screenMovementRight * _xMov;
        Vector3 v = screenMovementForward * _zMov;

        Vector3 moveDirection = (v + h).normalized;

        if (_xMov != 0 || _zMov != 0)
        {
            targetDir = moveDirection;
        }
        else
        {
            targetDir = transform.forward;
        }

        Quaternion lookDir = Quaternion.LookRotation(targetDir);

        Vector3 SetGroundDir = FloorAngleCheck();
        GroundDir = Vector3.Lerp(GroundDir, SetGroundDir, d * GravityRotationSpeed);

        //lerp mesh slower when not on ground
        RotateSelf(GroundDir, d, GravityRotationSpeed);
        RotateMesh(d, transform.forward, turnSpeed);

        //move character
        MovDirection = targetDir;
        float Spd = Speed;
        Vector3 curVelocity = Rigid.velocity;

        Vector3 targetVelocity = MovDirection;

        //fall from the air
        if (ActGravAmt < GravityAmt - 0.5f)
            ActGravAmt = Mathf.Lerp(ActGravAmt, GravityAmt, GravityBuildSpeed * d);

        //move either upwards or downwards with gravity
        targetVelocity -= GroundDir * ActGravAmt;

        Vector3 dir = Vector3.Lerp(curVelocity, targetVelocity, d * Accel);
        Rigid.velocity = dir;
    }
    //rotate the direction we face down
    void RotateSelf(Vector3 Direction, float d, float GravitySpd)
    {
        Vector3 LerpDir = Vector3.Lerp(transform.up, Direction, d * GravitySpd);
        transform.rotation = Quaternion.FromToRotation(transform.up, LerpDir) * transform.rotation;
    }
    //rotate the direction we face forwards
    void RotateMesh(float d, Vector3 LookDir, float spd)
    {
        Quaternion SlerpRot = Quaternion.LookRotation(LookDir, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, SlerpRot, spd * d);
    }
}
