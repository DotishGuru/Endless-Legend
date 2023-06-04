using System.Collections;
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
    private bool _isFacingPatrolingTarget;
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
    private Vector2 chaseTargetPosition;
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Physics2D.IgnoreLayerCollision(player.layer, this.gameObject.layer, true);        

        _rb = GetComponent<Rigidbody2D>();
        _target = waypoints[0].position;
        _target.y = transform.position.y;            
        _isChasing = false;
        _isFacingPatrolingTarget = false;

        UpdateEnemyUI();
    }

    void FixedUpdate()
    {              
        deltaTime = Time.fixedDeltaTime;

        PerformAction();

        UpdateEnemyUI();
    }

    private void PerformAction()
    {
        if(!RPGEntity.IsDead)
        {
            CalculateDistanceToPlayer();

            if (distance <= chasingDistance)
            {
                if (RPGEntity.IsInsideAttackRange())
                {
                    PerformAttack();                                       
                }
                else
                {
                    ChasePlayer();
                }
            }
            else
            {
                if (_isChasing)
                {
                    _direction = _target - transform.position;
                    _direction.Normalize();
                    FaceTowards(_direction.x);
                }

                Patrol();
                _isChasing = false;
            }
        }
    }

    private void CalculateDistanceToPlayer()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        _direction = player.transform.position - transform.position;
        _direction.Normalize();
    }

    private void UpdateEnemyUI()
    {
        if(Healthbar != null)
        {
            Healthbar.SetHealth(RPGEntity.currentHealth, RPGEntity.maxHealth);
        }
    }

    private void ChasePlayer()
    {
        if(!RPGEntity.IsAttacking && !RPGEntity.IsStaggered)
        {
            _isChasing = true;
            chaseTargetPosition = new Vector2(player.transform.position.x, this.transform.position.y);
            _chasingVelocity = ((transform.position - _chasingLastPosition) / deltaTime);
            _chasingLastPosition = transform.position;
            RPGEntity.animator.SetFloat("speed", _chasingLastPosition.magnitude);
            FaceTowards(_direction.x);
            transform.position = Vector2.MoveTowards(this.transform.position, chaseTargetPosition, RPGEntity.moveSpeed * deltaTime);
        }
    }

    private void PerformAttack()
    {
        if(RPGEntity.CanAttack())
        {            
            _isFacingPatrolingTarget = false;
            _direction = player.transform.position - transform.position;
            _direction.Normalize();
            FaceTowards(_direction.x);
            RPGEntity.Attack(attackType);
        }   

        RPGEntity.animator.SetFloat("speed", 0f);
        _isChasing = false;      
    }

    private void Patrol()
    {
        if(!RPGEntity.IsStaggered && !RPGEntity.IsDead && !RPGEntity.IsAttacking)
        {    
            if(Vector2.Distance(transform.position, _target) > 0.001f)
            {                
                MoveEnemy();
            }
            else
            {
                SetPatrolingDestinationPoint();
            } 
        }
        else
        {
            StopMovement();
        }        
    }

    private void MoveEnemy()
    {
        _velocity = ((transform.position - _previousPosition) / deltaTime);

        RPGEntity.animator.SetFloat("speed", _velocity.magnitude);

        _target.y = transform.position.y;

        if(!_isFacingPatrolingTarget)
        {
            FaceTowards(_target - transform.position);
        }

        transform.position = Vector2.MoveTowards(transform.position, _target, RPGEntity.moveSpeed * deltaTime);
    }

    private void StopMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, transform.position, 0);
        _velocity = Vector3.zero;
        RPGEntity.animator.SetFloat("speed", 0f);
    }

    private void SetPatrolingDestinationPoint()
    {
        StopMovement();

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

    public IEnumerator SetTarget(Vector3 position)
    {
        StopMovement();

        yield return new WaitForSeconds(5f);

        _target = position;
        _target.y = transform.position.y;
        FaceTowards(position - transform.position);
        _isFacingPatrolingTarget = true;
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
