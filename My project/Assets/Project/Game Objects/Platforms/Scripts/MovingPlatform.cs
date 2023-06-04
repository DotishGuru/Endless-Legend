using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    float deltaTime;
    public float speed;
    public int startingPoint;
    public Transform[] points;

    private int index;
    void Start()
    {        
        transform.position = points[startingPoint].position;
    }

    private void FixedUpdate() 
    {
        deltaTime = Time.fixedDeltaTime;
        MovePlatform();
    }

    private void MovePlatform()
    {
        if(Vector2.Distance(transform.position, points[index].position) < 0.02f)
        {
            index++;
            if(index == points.Length)
            {
                index = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[index].position, speed * deltaTime);
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        other.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D other) {
        other.transform.SetParent(null);
    }
}
