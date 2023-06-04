using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGEntity : MonoBehaviour
{
    #region Private variables        
    
    private bool _isDead;
    private bool _isStaggered;
    private bool _isAttacking;
    private float _nextStaggerTime;
    private float _nextAttackTime;

    #endregion

    #region Public variables  
    
    [Header ("Objects' references")]
    public Animator animator;
    public BarsUIBehaviour BarsUIBehaviour; 

    [Header ("Stats")]
    public int maxHealth;
    public int currentHealth;
    public int strength;
    public int defense;
    public int magic;
    public int magicDefense;
    public int level;
    public int currentExperience;
    public int levelUpExperienceThreshold;
    public float moveSpeed;
    public float staggerCooldown = 3f;

    [Header ("Attack mechanic")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public GameObject rangedProjectile;
    public Transform projectileSpawnPoint;
    public LayerMask enemyLayers;
    public float attackRate; 

    #endregion

    #region Properties        
    
    public bool IsDead { get {return _isDead;}}
    public bool IsStaggered {get {return _isStaggered;}}
    public bool IsAttacking {get {return _isAttacking;}}

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentHealth       = maxHealth;
        _isDead             = false;
        _isStaggered        = false;
        _isAttacking        = false;
        _nextStaggerTime    = 0.0f;
        _nextAttackTime     = 0.0f;

        if(BarsUIBehaviour != null)
        {
            InitUIBars();
        }
    }

    public void InitUIBars()
    {
        BarsUIBehaviour.SetHealth(currentHealth, maxHealth);
        BarsUIBehaviour.SetExperience(currentExperience, levelUpExperienceThreshold);
    }
    
    public virtual void Attack()
    {
        if(CanAttack() && IsInsideAttackRange())
        {            
            _nextAttackTime = Time.time + attackRate / 1f;

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            if(hitEnemies.Length > 0)
            {
                MeleAttack(hitEnemies);
            }            
        }
    }

    public virtual void Attack(AttackTypes attackType)
    {
        if(CanAttack() && IsInsideAttackRange())
        {            
            _nextAttackTime = Time.time + attackRate / 1f;

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            if(hitEnemies.Length > 0)
            {
                switch (attackType)
                {
                    case AttackTypes.Melee:
                        MeleAttack(hitEnemies);
                        break;

                    case AttackTypes.Ranged:
                        RangedAttack();
                        break;

                    default:
                        break;
                }
            }            
        }
    }

    public void MeleAttack(Collider2D[] targets)
    {
        foreach (Collider2D target in targets)
        {
            RPGEntity entity = target.GetComponent<RPGEntity>();

            if(entity != null)
            {
                animator.SetTrigger("Attack");
                entity.TakeDamage(strength, this);
            }
        }
    }

    public void RangedAttack()
    {
        if(rangedProjectile != null)
        {
            animator.SetTrigger("Attack");
            Instantiate(rangedProjectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }
        else
        {
            Debug.Log("Ranged projectile was not selected!");
        }
    }

    public bool IsInsideAttackRange()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
        return hitEnemies.Length > 0;
    }

    public bool CanAttack()
    {     
        return (Time.time >= _nextAttackTime) && !_isAttacking && !_isStaggered && !_isDead;
    }

    public bool CanBeStaggered()
    {
        return Time.time >= _nextStaggerTime;
    }

    public void ModifyIsAttacking()
    {
        _isAttacking = !_isAttacking;
    }

    public void AttackStartEvent()
    {
        _isAttacking = true;
    }

    public void AttackEndEvent()
    {
        _isAttacking = false;
    }

    public void ModifyIsStaggered()
    {
        _isStaggered = !_isStaggered;
    }

    public virtual void TakeDamage(int damage, RPGEntity initiator = null)
    {
        if(!_isDead)
        {
            currentHealth -= (damage - defense);
            if(currentHealth <= 0 && !_isDead)
            {
                currentHealth = 0;        
                Debug.Log(initiator.gameObject.tag.ToString());    
                if(initiator != null && initiator.gameObject.tag == "Player")
                {
                    initiator.GainExperience(10);
                }
                StartCoroutine("Death");
            }
            else
            {
                if(CanBeStaggered())
                {
                    _nextStaggerTime = Time.time + staggerCooldown / 1f;
                    StartCoroutine("Stagger");
                }
            }

            if(BarsUIBehaviour != null)
            {
                BarsUIBehaviour.SetHealth(currentHealth, maxHealth);
            }
        }
    }

    public void GainExperience(int exp)
    {
        currentExperience += exp;
        if (currentExperience >= levelUpExperienceThreshold)
        {            
            LevelUp();
        }

        if(BarsUIBehaviour != null)
        {
            BarsUIBehaviour.SetExperience(currentExperience, levelUpExperienceThreshold);
        }
    }

    public void LevelUp()
    {
        currentExperience = currentExperience % levelUpExperienceThreshold;
        level++;
        strength += 2;
        defense += 2;
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        else
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }        
    }

    public IEnumerator Stagger()
    {
        _isAttacking = false;
        animator.SetTrigger("Stagger");
        yield return new WaitForSeconds(1f);
        animator.ResetTrigger("Stagger");
    }

    public void StaggerStartEvent()
    {
        _isStaggered = true;
    }    

    public void StaggerEndEvent()
    {
        _isStaggered = false;
    }

    public IEnumerator Death()
    {      
        _isDead = true;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(5f);
        if(this.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject.transform.parent.gameObject);
        }        
        else if(this.gameObject.tag == "Player")
        {
            GameManager.Instance.GameOver();
        }
        
    }

    public enum AttackTypes
    {
        Melee,
        Ranged
    }
}
