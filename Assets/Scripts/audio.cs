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

    public AudioSource SFXGaviota, SFXGente, SFXMove, SFXGolpe;
    void Update()
    {
        audioMixer.SetFloat("Master", Master.value);
        audioMixer.SetFloat("SFX", SFXSlider.value);
        audioMixer.SetFloat("BGM", BGMSlider.value);

    }
    
}