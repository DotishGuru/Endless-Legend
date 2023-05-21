using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 100;
    int currentHealth;
    bool isDead;
    bool isHurt;
    public Animator animator;
    public HealthbarBehaviour Healthbar;


    public float speed;
    private Vector3 target;
    private Vector3 velocity;
    private Vector3 previousPosition;
    private bool flipped = true;
    private int index;

    public Transform[] waypoints;
    void Start()
    {
        currentHealth = maxHealth;
        isDead = false;
        isHurt = false;
        target = waypoints[0].position;
        Healthbar.SetHealth(currentHealth, maxHealth);
    }

    private void FixedUpdate() {
        //Movement();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead && !isHurt)
        {
            Movement();
        }
    }

    public void Movement()
    {
        velocity = ((transform.position - previousPosition) / Time.deltaTime);
        previousPosition = transform.position;

        animator.SetFloat("speed", velocity.magnitude);
        if(transform.position.x != target.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            if(target == waypoints[0].position)
            {
                if(flipped)
                {
                    flipped = !flipped;
                    StartCoroutine("SetTarget", waypoints[1].position);
                }
            }
            else
            {
                if(!flipped)
                {
                    flipped = !flipped;
                    StartCoroutine("SetTarget", waypoints[0].position);
                }
            }
        }  
    }

    public IEnumerator SetTarget(Vector3 position)
    {
        yield return new WaitForSeconds(5f);
        target = position;
        FaceTowards(position - transform.position);
    }

    public void FaceTowards(Vector3 direction)
    {
        if(direction.x < 0f)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void TakeDamage(int damage)
    {       
        currentHealth -= damage;

        if(currentHealth <= 0 && !isDead)
        {
            //Die();
            currentHealth = 0;
            isDead = true;
            animator.ResetTrigger("Hurt");
            animator.SetTrigger("isDead");
            StartCoroutine("Die");
        }
        else if (!isDead)
        {            
            if(!isHurt)
            {
                StartCoroutine("Hurt");
            }
        }        
    }

    public IEnumerator Hurt()
    {        
        animator.SetTrigger("Hurt");
        isHurt = true;
        yield return new WaitForSeconds(1f);
        isHurt = false;
        animator.ResetTrigger("Hurt");
    }

    public IEnumerator Die()
    {      
        yield return new WaitForSeconds(5f);
        Debug.Log("Enemies died"); 
        Destroy(this.gameObject);
    }
}
