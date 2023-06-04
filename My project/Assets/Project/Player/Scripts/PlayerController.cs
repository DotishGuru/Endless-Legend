using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private float inputX;
    private bool isGrounded;
    private bool isJumpPreparation;
    private bool isJumping;
    private bool isRunning;
    private Vector2 movement;
    private Vector3 characterFlipVector = new Vector3(-1f, 1f, 1f);
    private float timeSinceLastCheck;
    private float deltaTime;
    public float jumpForce;
    public RPGEntity RPGEntity;
    public Animator anim;    
    public GroundCheck GroundCheck;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() { 

        if(!RPGEntity.IsDead)
        {
            isGrounded = GroundCheck.IsGrounded;
            Movement();
        }
        else
        {
            isJumping = false;
            isRunning = false;
            anim.SetBool("isJumping", isJumping);
            anim.SetBool("isRunning", isRunning);
        }        
    }

    void Movement()
    {
        CharacterStateControl();
        CharacterVelocityUpdate();
        FlipCharacter();     
    }

    private void CharacterVelocityUpdate()
    {
        
        if(!RPGEntity.IsAttacking && !RPGEntity.IsStaggered)
        {
            rb.velocity = new Vector2(inputX * RPGEntity.moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
            isRunning = false;
            anim.SetBool("isRunning", isRunning);
            isJumping = false;
            anim.SetBool("isJumping", isJumping);
        } 
    }

    private void FlipCharacter()
    {
        if(rb.velocity.x > 0f && transform.localScale != Vector3.one)
        {
            transform.localScale = Vector3.one;
        } 
        else if (rb.velocity.x < 0f && transform.localScale != characterFlipVector)
        {
            transform.localScale = characterFlipVector;
        }
    }

    private void CharacterStateControl()
    {      
        if(!RPGEntity.IsAttacking && !RPGEntity.IsStaggered)
        {
            if(isGrounded && isJumping)
            {
                isJumping = false;
                anim.SetBool("isJumping", isJumping);
            }

            if(inputX != 0 && isGrounded)
            {
                isRunning = true;
                anim.SetBool("isRunning", isRunning);
            }
            else
            {
                isRunning = false;
                anim.SetBool("isRunning", isRunning);
            }             

            if(rb.velocity.y > 0 && !isGrounded)
            {
                isJumping = true;
                anim.SetBool("isJumping", isJumping);
            }
            else if(rb.velocity.y <= 0 && !isGrounded)
            {
                isJumping = true;
                anim.SetBool("isJumping", isJumping);
                anim.SetFloat("velocityY", 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Void"))
        { 
            //GameManager.Instance.RestartLevel();    
        }
        else if (other.gameObject.CompareTag("NextLevel"))
        {
            Debug.Log("Next level load");
            GameManager.Instance.LoadNextLevel();
        }             
    }

    public void Move(InputAction.CallbackContext context)
    {            
        inputX = context.ReadValue<Vector2>().x;        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed && isGrounded && !RPGEntity.IsDead)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetFloat("velocityY", 1);
        }
    }    

    public void Attack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            RPGEntity.Attack();
        }
    }

    public void Exit(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            GameManager.Instance.ExitGame();
        }
    }
}
