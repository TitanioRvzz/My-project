using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tortuga : MonoBehaviour
{
    public float velocidad = 2f;
    public bool enArenaMojada = false; // Indica si la tortuga está en arena mojada
    private bool puedeMoverse = false; // Indica si la tortuga puede moverse
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Si puede moverse, mueve la tortuga
        if (puedeMoverse && enArenaMojada)
        {
            MoverTortuga();
        }
        else
        {
            rb.velocity = Vector2.zero; // Detener movimiento si no puede moverse
        }
    }

    void MoverTortuga()
    {
        // Movimiento hacia la derecha (puedes cambiarlo por otro tipo de movimiento)
        Vector2 movimiento = new Vector2(velocidad, 0f);
        rb.velocity = movimiento;
    }

    // Detecta si está en arena mojada y comienza a moverse
    public void ActivarMovimiento()
    {
        enArenaMojada = true;
        puedeMoverse = true;
    }

    // Detiene el movimiento de la tortuga
    public void DetenerMovimiento()
    {
        puedeMoverse = false;
        rb.velocity = Vector2.zero; // Detener movimiento inmediatamente
    }

    // Reanuda el movimiento de la tortuga
    public void ReanudarMovimiento()
    {
        puedeMoverse = true;
    }

    // Cuando la tortuga está fuera del área de arena mojada, se detiene
    public void DesactivarMovimiento()
    {
        enArenaMojada = false;
        puedeMoverse = false;
    }
}