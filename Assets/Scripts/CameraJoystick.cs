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
    public Transform YPivot;

    private Transform pivot;
    private Transform FollowRotationPivot;
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

        HandleInput(d, v, h, rotateSpeed);
        checkOutOfBounds();

        //Look towards the player
        LookAtPos = target.position;
        Vector3 LerpDir = Vector3.Lerp(transform.up, target.up, d * FollowRotSpeed);
        transform.rotation = Quaternion.FromToRotation(transform.up, LerpDir) * transform.rotation;
    }
    
    /*
     * Checks if their is a ground object at an offset in the given direction
     */
    RaycastHit collisionCheck(Vector3 pos, Vector3 direction, float distance)
    {
        //Debug.Log("Point: " + pos + ", Direction: " + direction + ", Max Dist: " + distance);
        //Check if any object tagged as being "Ground" is colliding with the calculated point
        RaycastHit[] hits = Physics.RaycastAll(pos, direction, distance, GroundLayer);
        //Debug.Log(hits.Length);

        RaycastHit closest = new RaycastHit();
        closest.distance = distance;
        closest.normal = new Vector3(0, 0, 0);
        foreach (RaycastHit hit in hits)
        {
            if (hit.distance < closest.distance) closest = hit;
        }

        return closest;
    }

    /*
     * Push the camera behind the player by the specified distance
     */
    void checkOutOfBounds()
    {
        float targetZ = DistanceFromPlayer;
        
        //Check if the camera would go out of bounds
        RaycastHit bounds = collisionCheck(transform.position, -transform.forward, Mathf.Abs(targetZ));
        targetZ = Mathf.Sign(targetZ) * (bounds.distance - 1); //TODO: Remove the quick fix and make this better
        //Debug.Log("TargetZ: " + targetZ);

        CurrentDis = Mathf.Lerp(CurrentDis, targetZ, delta * 5f);

        Vector3 tp = Vector3.zero;
        tp.z = CurrentDis;
        camTransform.localPosition = tp;
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
            transform.RotateAround(transform.position, target.up, ((smoothX * speed) * 30f) * d);
        }
        
        //Rotate the camera around the X-axis (Tilting the camera up or down)
        if (smoothY != 0)
        {
            Transform newPosition = transform;
            newPosition.RotateAround(transform.position, target.right, ((smoothY * speed) * 30f) * d);
            Vector3 changeDir = newPosition.position - transform.position;
            RaycastHit bound = collisionCheck(transform.position, changeDir, changeDir.magnitude);
            Vector3 boundedDir = Vector3.ClampMagnitude(changeDir, bound.distance);
            transform.SetPositionAndRotation(transform.position + boundedDir, transform.rotation);
            
            //targetZ = Mathf.Sign(targetZ) * (bounds.distance - 1); //TODO: Remove the quick fix and make this better

            //transform.RotateAround(transform.position, target.right, ((smoothY * speed) * 30f) * d);
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