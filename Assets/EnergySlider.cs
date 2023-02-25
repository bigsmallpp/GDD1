using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergySlider : MonoBehaviour
{
    public Slider energySlider;
    
    
    public void SetEnergySlider(int energy)
    {
        if(energy != null)
        {
            energySlider.value = energy;
        }
        
    }

    public void SetMaxEnergy(int energy)
    {
  
       energySlider.maxValue = energy;
       energySlider.value = energy;
        

    }
    
}
