using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Velocidad de movimiento del NPC
    public float velocidadMovimiento = 2f;

    // Tiempo de espera antes de moverse a una nueva posición (en segundos)
    public float tiempoDeCambio = 5f;

    // Puntos de destino aleatorios donde el NPC se moverá
    public Vector2 areaDeMovimientoMin = new Vector2(-5f, -5f); // Limites del área en el eje X y Y
    public Vector2 areaDeMovimientoMax = new Vector2(5f, 5f); // Limites del área en el eje X y Y

    // Referencia al Rigidbody2D del NPC para moverlo
    private Rigidbody2D rb;

    // Tiempo que ha transcurrido desde el último movimiento
    private float tiempoTranscurrido = 0f;

    // Dirección hacia la que se mueve el NPC
    private Vector2 destino;

    void Start()
    {
        // Obtener el Rigidbody2D del NPC
        rb = GetComponent<Rigidbody2D>();

        // Establecer un destino inicial aleatorio
        GenerarNuevoDestino();
    }

    void Update()
    {
        // Contar el tiempo transcurrido
        tiempoTranscurrido += Time.deltaTime;

        // Si ha pasado el tiempo de cambio, generar una nueva posición y mover al NPC
        if (tiempoTranscurrido >= tiempoDeCambio)
        {
            GenerarNuevoDestino();
            tiempoTranscurrido = 0f; // Resetear el temporizador
        }

        // Mover al NPC hacia el destino
        MoverNPC();
    }

    void GenerarNuevoDestino()
    {
        // Generar un destino aleatorio dentro del área de movimiento definida
        float xDestino = Random.Range(areaDeMovimientoMin.x, areaDeMovimientoMax.x);
        float yDestino = Random.Range(areaDeMovimientoMin.y, areaDeMovimientoMax.y);
        destino = new Vector2(xDestino, yDestino);
    }

    void MoverNPC()
    {
        // Mover el NPC hacia el destino utilizando MoveTowards
        transform.position = Vector2.MoveTowards(transform.position, destino, velocidadMovimiento * Time.deltaTime);
    }

    // Detectar si la tortuga entra en el área de colisión con el NPC
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Turtle")) // Verificar si el tag es "Turtle"
        {
            // Detener el movimiento de la tortuga si entra en la zona del NPC
            col.GetComponent<Tortuga>().DetenerMovimiento();
        }
    }

    // Detectar si la tortuga sale del área de colisión con el NPC
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Turtle")) // Verificar si el tag es "Turtle"
        {
            // Reanudar el movimiento de la tortuga si sale del área del NPC
            col.GetComponent<Tortuga>().ReanudarMovimiento();
        }
    }
}