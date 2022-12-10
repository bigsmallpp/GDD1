using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private int currentEnergy;
    public int energyCost = 5;
    public int TotalEnergy = 100;

    public GameObject Slider;
    
    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = TotalEnergy;

        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            EnergyChange();
        }
    }

    public void EnergyChange()
    {
        currentEnergy -= energyCost;

        if (Slider != null)
        {
            EnergySlider EnergySliderScript = Slider.GetComponent<EnergySlider>();
            EnergySliderScript.SetEnergy(currentEnergy);
        }
        

        
        



    }
}
