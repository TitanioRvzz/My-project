using System.Collections;
using UnityEngine;

public class Gaviota : MonoBehaviour
{
    public float advertenciaDuracion = 2f;
    public float velocidadVuelo = 5f;
    public float velocidadPicada = 10f;
    public float elevacionVelocidad = 5f;
    public Animator animator;
    private Transform objetivoTortuga;
    private Vector3 direccionVuelo;

    void Start()
    {
        direccionVuelo = Random.value > 0.5f ? Vector3.right : Vector3.left;
    }

    void Update()
    {
        if (objetivoTortuga == null)
        {
            // Movimiento horizontal continuo
            transform.Translate(direccionVuelo * velocidadVuelo * Time.deltaTime);
        }
    }

    public void IniciarAtaque(Transform tortuga)
    {
        objetivoTortuga = tortuga;
        animator.SetTrigger("Advertencia");
        //tortuga.GetComponent<TurtleMovement>().ActivarAdvertencia(advertenciaDuracion, Ataque);
    }

    private void Ataque()
    {
        StartCoroutine(Picada());
    }

    private IEnumerator Picada()
    {
        Vector3 destino = objetivoTortuga.position;

        // Movimiento en picada
        while (Vector3.Distance(transform.position, destino) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidadPicada * Time.deltaTime);
            yield return null;
        }

        if (objetivoTortuga != null)
        {
            Destroy(objetivoTortuga.gameObject); // Destruir tortuga
        }

        // Movimiento de elevación
        Vector3 elevacion = transform.position + Vector3.up * 5f;
        while (Vector3.Distance(transform.position, elevacion) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, elevacion, elevacionVelocidad * Time.deltaTime);
            yield return null;
        }

        objetivoTortuga = null;
    }
}
