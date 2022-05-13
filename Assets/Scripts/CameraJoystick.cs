using UnityEngine;

public class CameraJoystick : MonoBehaviour
{
    //public bool FollowTargetRotation;
    [Header("FollowSpeed")]
    public float FollowRotSpeed = 0.5f;
    private Vector3 LookDirection;

    public Transform target;
    public Transform FollowTarget;

    public float MinDistanceFromWall = 0.25f;
    public Transform camTransform;
    private Camera CamUnit;
    public Joystick joystick;

    [Header("Mouse Speeds")]
    public float MouseSpeed = 2;
    public float turnSmoothing = 0.1f;
    public float minAngle = 5;
    public float maxAngle = 35;

    public float DistanceFromPlayer;
    private float CurrentDis;

    float smoothX;
    float smoothXvelocity;
    float smoothY;
    float smoothYvelocity;

    private float xInverse = -1.0f; //1.0f yes, -1.0f no
    private float yInverse = 1.0f; //1.0f yes, -1.0f no

    [Header("Out of Bounds Check")]
    public LayerMask GroundLayer;

    float delta;

    [Header("Camera Switch")]
    public Camera camUp;

    private float pitch = 0;
    
    //Gravity Flip
    private bool GravityFlipping;

    //setup objects
    void Awake()
    {
        transform.parent = null; //Detach the coordinates of the parent of the Camera

        CurrentDis = DistanceFromPlayer;

        CamUnit = GetComponentInChildren<Camera>();

        pitch = 90 - Vector3.Angle(target.up, -transform.forward);
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
        float h = xInverse * joystick.Horizontal;
        float v = -yInverse * joystick.Vertical;
        float rotateSpeed = MouseSpeed;

        float oldPitch = pitch;
        if(!GravityFlipping) HandleInput(d, v, h, rotateSpeed);
        else
        {
            //Force the cam to remain stable at default pitch
            float rotAmount = Mathf.Min((FollowRotSpeed * 30f) * d, 90 - pitch);
            transform.RotateAround(transform.position, transform.right, rotAmount);
            pitch += rotAmount;
        }
        
        //Check if there is ground inbetween the camera and the player
        if (checkOutOfBounds())
        {
            //If the player is still trying to move the camera downwards, stop them
            pitch = 90 - Vector3.Angle(target.up, -transform.forward);
            if (oldPitch - pitch > 0)
            {
                transform.RotateAround(transform.position, transform.right, oldPitch - pitch);
            }
        }
            

        //Apply force to restore the camera
        Vector3 LerpDir = Vector3.Lerp(transform.up, target.up, d * FollowRotSpeed);
        transform.rotation = Quaternion.FromToRotation(transform.up, LerpDir) * transform.rotation;
        
        //Clamp the final pitch of the camera to be within the accepted interval
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
        
        //Debug.DrawLine(target.position, camTransform.position, Color.yellow, 0.0f, true);
        //Debug.Log("Pitch: " + pitch);
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
    }

    public void InAir(bool currently)
    {
        //camUp.enabled = currently;
        //CamUnit.enabled = !currently;
        GravityFlipping = currently;
    }

    public void setXInverse(bool yes)
    {
        xInverse = yes ? 1.0f : -1.0f;
    }
    
    public void setYInverse(bool yes)
    {
        yInverse = yes ? 1.0f : -1.0f;
    }
}