using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueGaviota : MonoBehaviour
{
    public GameObject tortuga; // Referencia al objeto de la tortuga
    public Animator animator; // Referencia al Animator de la gaviota
    public float distanciaAtaque = 2f; // Distancia máxima para atacar
    public float tiempoEntreAtaques = 2f; // Tiempo entre cada ataque

    private float timer = 0f;

    void Update()
    {
        // Verificar si la tortuga está quieta (ajusta esta condición según tu lógica de movimiento)
        if (tortuga.GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f)
        {
            // Verificar si la distancia a la tortuga es menor a la distancia de ataque
            if (Vector2.Distance(transform.position, tortuga.transform.position) < distanciaAtaque)
            {
                timer += 3f * Time.deltaTime;
                if (timer >= tiempoEntreAtaques)
                {
                    // Reproducir la animación de ataque
                    animator.SetTrigger("Atacar");
                    timer = 0f;
                }
            }
        }
    }
}
