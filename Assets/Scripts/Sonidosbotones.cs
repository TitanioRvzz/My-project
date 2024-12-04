using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Sonidosbotones : MonoBehaviour
{
[Header("Volumen")]
public AudioSource audioSource;
public AudioClip audioClip;
public AudioMixer Mixer;
public GameObject panel;


public void camiarvolumentmaster(float v)
{
    Mixer.SetFloat("SFX", v);
}
public void controlpanel(GameObject panell)
{
playsoundButton();
}
public void playsoundButton()

{
    audioSource.PlayOneShot(audioClip);
}

}
