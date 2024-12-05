using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TortugaAtaque : MonoBehaviour
{
    [Header("Gaviota Settings")]
    public GameObject gaviota; 
    public float tiempoSinMoverse; 
    public float velocidadAtaque;

    private Vector2 ultimaPosicion; 
    private Vector2 posicionAntesDeAtaque; 
    private bool estaSiendoAtacada = false;
    private float tiempoQuieto;

    void Start()
    {
        ultimaPosicion = transform.position; 
        tiempoQuieto = 0f;
    }

    void Update()
    {
       
        if ((Vector2)transform.position == ultimaPosicion)
        {
            tiempoQuieto += Time.deltaTime;
           

            if (tiempoQuieto >= tiempoSinMoverse && !estaSiendoAtacada)
            {
                posicionAntesDeAtaque = ultimaPosicion; 
                StartCoroutine(AtaqueGaviota());
                
            }
        }
        else
        {
            tiempoQuieto = 0f; 
        }

        ultimaPosicion = transform.position;
    }

    IEnumerator AtaqueGaviota()
    {
        estaSiendoAtacada = true;

        while (gaviota != null && Vector2.Distance(gaviota.transform.position, posicionAntesDeAtaque) > 0.7f)
        {
            gaviota.transform.position = Vector2.MoveTowards(gaviota.transform.position, posicionAntesDeAtaque, velocidadAtaque * Time.deltaTime);
            yield return null;
        }

        estaSiendoAtacada = false;
    }
}
