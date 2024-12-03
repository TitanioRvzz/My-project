using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Sonidosbotones : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    private void Start() 
    {
    audioSource.clip = audioClip;    
    }

    public void Reproducir()
    {
        audioSource.Play();
    }

}
