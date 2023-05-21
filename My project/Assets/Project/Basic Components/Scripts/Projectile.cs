using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int projectileDamage;
    float deltaTime;
    private RPGEntity targetRPGEntity;

    private void FixedUpdate()
    {
        deltaTime = Time.fixedDeltaTime;

        transform.Translate(Vector2.right * speed * deltaTime); 
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            targetRPGEntity = other.gameObject.GetComponent<RPGEntity>();

            if(targetRPGEntity != null)
            {
                targetRPGEntity.TakeDamage(projectileDamage);
            }
        }

        Destroy(this.gameObject);    
    }
}
