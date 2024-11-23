using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GAVIOTA : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    private int currentWaypointIndex = 0;
    private bool isMovingForward = true;
    public Collider2D seagullCollider;
    public float chanceToActivateCollider = 0.5f;

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
                gameObject.GetComponent<SpriteRenderer>().flipX = false;

                currentWaypointIndex += 1;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = waypoints.Length - 2;
                    isMovingForward = false;
                }
            }
            else if (!isMovingForward)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

                currentWaypointIndex -= 1;
                if (currentWaypointIndex < 0)
                {
                    currentWaypointIndex = 1;
                    isMovingForward = true;
                }
            }

            //pasa que luego de rebotar 2 veces y no hacer el flip, parece funcionar bien

            ActivarColliderConProbabilidad();
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime); 

    }

    private void ActivarColliderConProbabilidad()
    {
        float randomValue = Random.value;
        if (randomValue <= chanceToActivateCollider)
        {
            seagullCollider.enabled = true;
            Debug.Log("Collider activado, la gaviota puede destruir la tortuga.");
        }
        else
        {
            seagullCollider.enabled = false;
            Debug.Log("Collider no activado, la gaviota no puede destruir la tortuga.");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (seagullCollider.enabled && collision.gameObject.CompareTag("Turtle"))
        {
            Destroy(collision.gameObject);
            seagullCollider.enabled = false;
            Debug.Log("Tortuga destruida.");
        }
    }
}