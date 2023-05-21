using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    public RPGEntity RPGEntity;
    public HealthbarBehaviour Healthbar;
    public Transform[] waypoints;
    private Vector3 _target;
    private Vector3 _velocity;
    private Vector3 _previousPosition;
    private bool _flipped = true;
    private int _index;
    private Rigidbody2D _rb;
    public RPGEntity.AttackTypes attackType;
    float deltaTime;
    [Header("Player chase")]
    public GameObject player;
    public float chasingDistance;
    private float distance;
    private Vector3 _chasingLastPosition;
    private Vector3 _chasingVelocity;
    private Vector2 _direction;
    private bool _isChasing;
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        _rb = GetComponent<Rigidbody2D>();
        _target = waypoints[0].position;
        _target.y = transform.position.y;    
        if(Healthbar != null)
        {  
            Healthbar.SetHealth(RPGEntity.currentHealth, RPGEntity.maxHealth);
        }
        _isChasing = false;
    }

    void FixedUpdate()
    {              
        if(!RPGEntity.IsDead)
        {
            deltaTime = Time.fixedDeltaTime;

            distance = Vector2.Distance(transform.position, player.transform.position);
            _direction = player.transform.position - transform.position;
            _direction.Normalize();

            if(distance <= chasingDistance)
            {
                if(RPGEntity.IsInsideAttackRange())
                {
                    if(RPGEntity.CanAttack())
                    {
                        _direction = player.transform.position - transform.position;
                        _direction.Normalize();
                        FaceTowards(_direction.x);
                        RPGEntity.Attack(attackType);
                    }

                    RPGEntity.animator.SetFloat("speed", 0f);
                    _isChasing = false;
                }
                else
                {
                    ChasePlayer();
                }
            }
            else
            {
                if(_isChasing)
                {
                    _direction = _target - transform.position;
                    _direction.Normalize();
                    FaceTowards(_direction.x);
                }
                Movement();
                _isChasing = false;
            }      
        }

        if(Healthbar != null)
        {
            Healthbar.SetHealth(RPGEntity.currentHealth, RPGEntity.maxHealth);
        }
    }

    private void ChasePlayer()
    {
        if(!RPGEntity.IsAttacking && !RPGEntity.IsHurt)
        {
            _isChasing = true;
            Vector2 targetPosition = new Vector2(player.transform.position.x, this.transform.position.y);
            _chasingVelocity = ((transform.position - _chasingLastPosition) / deltaTime);
            _chasingLastPosition = transform.position;
            RPGEntity.animator.SetFloat("speed", _chasingLastPosition.magnitude);
            FaceTowards(_direction.x);
            transform.position = Vector2.MoveTowards(this.transform.position, targetPosition, RPGEntity.moveSpeed * deltaTime);
        }
    }

    private void CheckForAttack()
    {
        if(RPGEntity.CanAttack())
        {
            _direction = player.transform.position - transform.position;
            _direction.Normalize();
            FaceTowards(_direction.x);
            RPGEntity.Attack(attackType);
        }
    }

    private void Movement()
    {
        if(!RPGEntity.IsStaggered && !RPGEntity.IsDead && !RPGEntity.IsAttacking)
        {            
            _velocity = ((transform.position - _previousPosition) / deltaTime);

            _previousPosition = transform.position;

            RPGEntity.animator.SetFloat("speed", _velocity.magnitude);

            if(Vector2.Distance(transform.position, _target) > 0.001f)
            {                
                _target.y = transform.position.y;

                transform.position = Vector2.MoveTowards(transform.position, _target, RPGEntity.moveSpeed * deltaTime);
            }
            else
            {
                if(_target.x == waypoints[0].position.x)
                {
                    if(_flipped)
                    {
                        _flipped = !_flipped;
                        StartCoroutine("SetTarget", waypoints[1].position);
                    }
                }
                else
                {
                    if(!_flipped)
                    {
                        _flipped = !_flipped;
                        StartCoroutine("SetTarget", waypoints[0].position);
                    }
                }
            } 
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _previousPosition, 0);
            RPGEntity.animator.SetFloat("speed", _velocity.magnitude);
        }        
    }

    public IEnumerator SetTarget(Vector3 position)
    {
        yield return new WaitForSeconds(5f);
        _target = position;
        _target.y = transform.position.y;
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

    public void FaceTowards(int direction)
    {
        if(direction < 0)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void FaceTowards(float direction)
    {
        if(direction < 0.0f)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {        
        Gizmos.DrawLine((transform.position - (new Vector3(chasingDistance,0))), (transform.position + (new Vector3(chasingDistance, 0))));      
    }
}
