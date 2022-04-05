using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public float bottomOffset;
    public float frontOffset;
    public float collisionRadius;
    public LayerMask GroundLayer;

    /*
     * Checks if their is a ground object at an offset in the given direction
     */
    public bool CheckGround(Vector3 Direction)
    {
        //The position at the offset in the given direction
        Vector3 pos = transform.position + (Direction * bottomOffset);
        //Check if any object tagged as being "Ground" is colliding with the calculated point
        Collider[] hitColliders = Physics.OverlapSphere(pos, collisionRadius, GroundLayer);

        if (hitColliders.Length > 0) //If at least one object was collided with
        {
            return true;
        }

        return false;
    }

    /*
     * Visualizes the hitbox for this check
     */
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Vector3 Pos = transform.position + (-transform.up * bottomOffset);
        Gizmos.DrawSphere(Pos, collisionRadius);
    }
}
