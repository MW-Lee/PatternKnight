using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public AudioSource BGM;

    public Slider Slider;

    static public float EffectVolume = 1;
    static public float BGMVolume = 1;

    private void Start()
    {
        Slider = GetComponent<Slider>();
    }

    public void OnChange()
    {
        BGM.volume = Slider.value;
        BGMVolume = Slider.value;
    }

    public void OnEffect()
    {
        EffectVolume = Slider.value;
    }
}
