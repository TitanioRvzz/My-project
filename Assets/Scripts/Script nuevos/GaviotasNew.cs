using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Gaviotas : MonoBehaviour
{
    public float speed = 5f; 
    public float waitTime = 2f; // Enfriamento de collider2D 



    public Vector3 startPosition; // Posici�n inicial de la gaviota
    private Vector3 targetPosition; 
    private bool isMoving = false; // Bandera para controlar el movimiento
    private BoxCollider2D boxCollider; 

    private void Start()
    {
        // Guardar la posici�n inicial al iniciar
        startPosition = transform.position;

        // Obtener el BoxCollider2D del objeto
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isMoving)
        {
            // Moverse hacia la posici�n objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Verificar si lleg� al objetivo
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;

                // Si el objetivo era el inicio, detener el movimiento
                if (targetPosition == startPosition)
                {
                    Debug.Log("Gaviota regres� a su posici�n inicial.");
                }
                else
                {
                    // Si lleg� a otro punto (como la tortuga), volver al inicio
                    SetTarget(startPosition);
                }
            }
        }
    }

    public void SetTarget(Vector3 newTargetPosition)
    {
        // Configurar la nueva posici�n objetivo y habilitar el movimiento
        targetPosition = newTargetPosition;
        isMoving = true;
    }

   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Turtle"))
        {
           
            Destroy(collision.gameObject);

            // Comenzar el regreso al punto inicial despu�s de eliminar la tortuga
           StartCoroutine(HandleAfterAttack());
        }
    }

    private IEnumerator HandleAfterAttack()
    {
        
        boxCollider.enabled = false;

       
        SetTarget(startPosition);

        
        yield return new WaitForSeconds(waitTime);

        
        boxCollider.enabled = true;
    }
}
