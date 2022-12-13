using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractSlider : MonoBehaviour
{
    public Slider progressSlider;
    public void SetProgress(float progress)
    {
        if (progressSlider != null)
        {
            progressSlider.value = progress;
        }
        
        

    }

    public void SetMaxProgress(float maxprogress)
    {
        progressSlider.maxValue = maxprogress;
    }
}
