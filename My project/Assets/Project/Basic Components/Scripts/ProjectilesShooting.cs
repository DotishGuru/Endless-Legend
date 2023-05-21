using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesShooting : MonoBehaviour
{
    public Transform player;
    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
            
    }

    public void Shoot()
    {
        Instantiate(projectile, transform.position, transform.rotation);
    }
}
