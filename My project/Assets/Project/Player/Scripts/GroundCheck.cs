using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{    
    private float timeSinceLastCheck;
    private float deltaTime;
    private bool isGrounded;

    public bool IsGrounded {get {return isGrounded;}}

    [Header("Main settings")]
    public float checkInterval = 0.02f;
    public LayerMask whatIsGround;
    public BoxCollider2D targetObjectCollider;

    [Header("Collider settings")]
    public float extraHeight = 0.1f;
    public Vector3 boxOffset = new Vector3(0f, -0.5f, 0f); 

    void FixedUpdate()
    {
        //deltaTime = Time.fixedDeltaTime * Time.timeScale;
        deltaTime = Time.fixedDeltaTime;
        timeSinceLastCheck += deltaTime;
        if (timeSinceLastCheck >= checkInterval)
        {
            isGrounded = CheckIfGrounded();
        }  
    }

    private bool CheckIfGrounded()
    {
        bool ret = true;

        RaycastHit2D raycastHit2D = Physics2D.BoxCast(targetObjectCollider.bounds.center + boxOffset, new Vector2(targetObjectCollider.bounds.size.x, 0.1f), 0f, Vector2.down, 0.1f, whatIsGround);
        timeSinceLastCheck = 0f;

        if(raycastHit2D.collider != null)
        {
            ret = true;           
        }
        else
        {
            ret = false;          
        }       

        return ret;
    }
}
