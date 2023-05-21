using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private float checkInterval = 0.02f;
    private float timeSinceLastCheck;
    private float deltaTime;
    public float jumpForce;
    public RPGEntity RPGEntity;
    public Animator anim;    
    public GroundCheck GroundCheck;

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate() { 
        isGrounded = GroundCheck.IsGrounded;
        Movement();
    }

    void Movement()
    {
        CharacterVelocityUpdate();
        FlipCharacter();
        CharacterStateControl();
    }

    private void CharacterVelocityUpdate()
    {
        if(!RPGEntity.IsAttacking)
        {
            rb.velocity = new Vector2(inputX * RPGEntity.moveSpeed, rb.velocity.y);
            //movement = new Vector2(inputX * RPGEntity.moveSpeed, rb.velocity.y);
        }
        else
        {
            Debug.Log("Player cant move");
            rb.velocity = new Vector2(0, rb.velocity.y);
           // movement = Vector2.zero;
        }  

        //rb.AddForce(movement * RPGEntity.moveSpeed);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered");
        if(other.gameObject.CompareTag("Void"))
        {
            //StartCoroutine(RPGEntity.Death());  
            GameManager.Instance.RestartLevel();    
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
        if(context.performed && isGrounded)
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
}
