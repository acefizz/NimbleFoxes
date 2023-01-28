using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionData
{
    public float[] options = new float[3];
    public float[] optionsSlider = new float[3];

    public OptionData(SoundManager sound)
    {
        for(int i = 0; i < options.Length; ++i)
        {
            options[i] = sound.options[i];
        }

        for (int i = 0; i < optionsSlider.Length; ++i)
        {
            optionsSlider[i] = sound.optionsSlider[i];
        }
    }
}
