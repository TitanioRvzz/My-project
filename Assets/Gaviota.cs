using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaviota : MonoBehaviour
{
    public Transform[] waypoints;  
    public float moveSpeed = 3f; 
    private int currentWaypointIndex = 0;  
    private bool isMovingForward = true;  
    public Collider2D ColliderGaviota;  
    public float chanceToActivateCollider = 0.5f;

    void Start()
    {

    }
    void Update()
    {
        MoverGaviota();  
    }

    private void MoverGaviota()
    {
       if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            if (isMovingForward)
            {
                currentWaypointIndex += 1;  
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = waypoints.Length - 2; 
                    isMovingForward = false;  
                }
            }
            else
            {
                currentWaypointIndex -= 1;  
                if (currentWaypointIndex < 0)
                {
                    currentWaypointIndex = 1;  
                    isMovingForward = true;  
                }
            }
            ActivarColliderConProbabilidad();
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);
    }

    private void ActivarColliderConProbabilidad()
    {
        float randomValue = Random.value;  
        if (randomValue <= chanceToActivateCollider)
        {
            ColliderGaviota.enabled = true; 
          
        }
        else
        {
            ColliderGaviota.enabled = false;  
           
        }
    }
    private void  OnCollisionEnter2D(Collision2D collision)
    {
        if (ColliderGaviota.enabled && collision.gameObject.CompareTag("Turtle"))
        {
            Destroy(collision.gameObject);  
            ColliderGaviota.enabled = false;  
           
        }
    }
}