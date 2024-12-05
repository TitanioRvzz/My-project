using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GaviotaNew : MonoBehaviour
{
    public float speed;
    public float waitTime; // Enfriamiento del collider2D

    private Vector3 startPosition; // Posición inicial de la gaviota
    private Vector3 targetPosition;
    private bool isMoving = false; // Bandera para controlar el movimiento
    private BoxCollider2D boxCollider;

    public GameManager gameManager;
    public TortugaOF tortuga;

    public AudioSource comida;
    public AudioClip clip;

    private void Start()
    {
        // Guardar la posición inicial al iniciar
        startPosition = transform.position;

        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isMoving)
        {
            // Moverse hacia la posición objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Verificar si llegó al objetivo
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;

                // Si el objetivo era el inicio, detener el movimiento
                if (targetPosition == startPosition)
                {
                    SetTarget(startPosition);

                    Debug.Log("Gaviota regresó a su posición inicial.");
                }
            }
        }
    }

    public void SetTarget(Vector3 newTargetPosition)
    {
        // Configurar la nueva posición objetivo y habilitar el movimiento
        targetPosition = newTargetPosition;
        isMoving = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TortugaOF>())
        {
            tortuga.CloneObjectWithProbability();
            Destroy(collision.gameObject);
            comida.clip = clip;
            comida.Play();
            print("tortuga atacada");
            gameManager.EliminarTortuga();

            // Comenzar el regreso al punto inicial después de eliminar la tortuga
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