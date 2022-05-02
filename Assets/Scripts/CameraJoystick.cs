using UnityEngine;

public class CameraJoystick : MonoBehaviour
{
    //public bool FollowTargetRotation;
    [Header("FollowSpeed")]
    public float FollowRotSpeed = 0.5f;
    public float FollowRotSpeedFlying = 10f;
    public float GravityFollowSpeed = 0.1f;
    private Vector3 LookDirection;

    public Transform target;
    public Transform FollowTarget;

    private Transform pivot;
    private Transform FollowRotationPivot;
    public float MinDistanceFromWall = 0.25f;
    public Transform camTransform;
    private Camera CamUnit;
    public Joystick joystick;

    private Vector3 LookAtPos;
    [Header("Mouse Speeds")]
    public float MouseSpeed = 2;
    public float turnSmoothing = 0.1f;
    public float minAngle = -35;
    public float maxAngle = 35;
    public float LookDirectionSpeed = 2f;

    public float DistanceFromPlayer;
    private float CurrentDis;

    float smoothX;
    float smoothXvelocity;
    float smoothY;
    float smoothYvelocity;
    private float lookAngle;
    private float tiltAngle;

    [Header("Out of Bounds Check")]
    public LayerMask GroundLayer;

    float delta;

    [Header("Camera Switch")]
    public Camera camUp;

    private float pitch = 0;

    //setup objects
    void Awake()
    {
        transform.parent = null; //Detach the coordinates of the parent of the Camera

        pivot = camTransform.parent;
        LookAtPos = target.position; //Look towards the target that the camera follows
        CurrentDis = DistanceFromPlayer;

        tiltAngle = 10f;

        //LookDirection = transform.forward;

        CamUnit = GetComponentInChildren<Camera>();

        //pitch = transform.localEulerAngles.x;
    }
    private void Update()
    {
        //Place the camera at the position of the FollowTarget at the begining of each frame
        transform.position = FollowTarget.position;

        //bool cameraLook = Input.GetAxis("CameraLook") != 0;
        //camUp.enabled = cameraLook;
        //CamUnit.enabled = !cameraLook;
    }

    private void FixedUpdate()
    {
        delta = Time.deltaTime;

        if (!target)
        {
            return;
        }

        Tick(delta);
    }

    public void Tick(float d)
    {
        float h = -joystick.Horizontal;
        float v = joystick.Vertical;
        float rotateSpeed = MouseSpeed;

        Debug.Log("Pitch: " + pitch);
        float oldPitch = pitch;
        HandleInput(d, v, h, rotateSpeed);
        if (checkOutOfBounds())
        {
            pitch = 90 - Vector3.Angle(target.up, -transform.forward);
            if (oldPitch - pitch > 0)
            {
                transform.RotateAround(transform.position, transform.right, oldPitch - pitch);
            }
        }

        //Look towards the player
        //Quaternion oldRotation = transform.rotation;
        LookAtPos = target.position;
        Vector3 LerpDir = Vector3.Lerp(transform.up, target.up, d * FollowRotSpeed);
        transform.rotation = Quaternion.FromToRotation(transform.up, LerpDir) * transform.rotation;
        //pitch -= d * FollowRotSpeed;
        //pitch = transform.localEulerAngles.x;

        pitch = 90 - Vector3.Angle(target.up, -transform.forward);
        if (pitch < minAngle)
        {
            transform.RotateAround(transform.position, transform.right, minAngle - pitch);
            pitch = minAngle;
        }
        else if (pitch > maxAngle)
        {
            transform.RotateAround(transform.position, transform.right, maxAngle - pitch);
            pitch = maxAngle;
        }
        pitch = 90 - Vector3.Angle(target.up, -transform.forward);
        
        Debug.DrawLine(target.position, camTransform.position, Color.yellow, 0.0f, true);
    }
    
    /*
     * Checks if their is a ground object at an offset in the given direction
     */
    RaycastHit collisionCheck(Vector3 pos, Vector3 direction, float distance)
    {
        RaycastHit closest = new RaycastHit();
        closest.distance = distance;
        closest.normal = new Vector3(0, 0, 0);
        if (distance == 0) return closest;
        
        //Debug.Log("Point: " + pos + ", Direction: " + direction + ", Max Dist: " + distance);
        //Check if any object tagged as being "Ground" is colliding with the calculated point
        //Check if any object tagged as being "Ground" is colliding with the calculated point
        //RaycastHit[] hits = Physics.RaycastAll(pos, direction, distance, GroundLayer);
        RaycastHit[] hits = Physics.SphereCastAll(pos, MinDistanceFromWall, direction, distance, GroundLayer);
        //Debug.Log(hits.Length);

        foreach (RaycastHit hit in hits)
        {
            if (hit.distance < closest.distance) closest = hit;
        }

        return closest;
    }

    /*
     * Push the camera behind the player by the specified distance
     */
    bool checkOutOfBounds()
    {
        float targetZ = DistanceFromPlayer;
        
        //Check if the camera would go out of bounds
        RaycastHit bounds = collisionCheck(transform.position, -transform.forward, Mathf.Abs(targetZ));

        if (bounds.distance == 0) return false;
        
        targetZ = Mathf.Sign(targetZ) * (bounds.distance);
        CurrentDis = Mathf.Lerp(CurrentDis, targetZ, delta * 5f);

        Vector3 tp = Vector3.zero;
        tp.z = CurrentDis;
        camTransform.localPosition = tp;

        return true;
    }

    /*
     * Rotate the camera as needed
     */
    void HandleInput(float d, float v, float h, float speed)
    {
        if (turnSmoothing > 0)
        {
            smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);
            smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing);
        }
        else
        {
            smoothX = h;
            smoothY = v;
        }
        
        
        //Rotate the camera around the Y axis (Rotating it around the object)
        if (smoothX != 0)
        {
            //transform.RotateAround(target.position, target.up, ((smoothX * speed) * 30f) * d);
            transform.RotateAround(target.position, transform.up, ((smoothX * speed) * 30f) * d);
        }
        
        //Rotate the camera around the X-axis (Tilting the camera up or down)
        if (smoothY != 0)
        {
            transform.RotateAround(target.position, transform.right, ((smoothY * speed) * 30f) * d);
        }

        /*
        tiltAngle -= smoothY * speed;
        tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
        pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

        lookAngle += smoothX * speed;
        if (lookAngle > 360)
            lookAngle = 0;
        else if (lookAngle < 0)
            lookAngle = 360;

        if (smoothX != 0)
        {
            transform.RotateAround(transform.position, transform.up, ((smoothX * speed) * 30f) * d);
        }
        */
    }
}