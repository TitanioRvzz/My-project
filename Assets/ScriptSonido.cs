using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ScriptSonido : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider BGMSlider, SFXSlider, Master;
    public AudioClip SfxMover;
    public AudioSource SFXGaviota, SFXGente;

    void Update()
    {
        audioMixer.SetFloat("Master", Master.value);
        audioMixer.SetFloat("SFX", SFXSlider.value);
        audioMixer.SetFloat("BGM", BGMSlider.value);
    }
}
