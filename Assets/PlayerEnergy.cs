using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] public int currentEnergy;
    
    public int TotalEnergy = 100;

    public TMP_Text energyText;
    public GameObject Slider;

    

    [Header("EnergyCost per Action")]
    [SerializeField] public int EnergyCostPlowing = 5;
    [SerializeField] public int EnergyCostSeeding = 2;
    [SerializeField] public int EnergyCostHarvesting = 10;

    public static List<int> EnergyCostList = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = TotalEnergy;

        energyText.text = TotalEnergy.ToString() + "/" + TotalEnergy.ToString();

        EnergyCostList.Add(EnergyCostPlowing);
        EnergyCostList.Add(EnergyCostSeeding);
        EnergyCostList.Add(EnergyCostHarvesting);
    }

    
    
    // on every EnergyChange the Slider and the Display in the Inventory gets updated
    public void EnergyChange(int i)
    {
        if (currentEnergy >= EnergyCost(i))
        {
            currentEnergy -= EnergyCost(i);

            //Updating both Energy Displays
            if (Slider != null)
            {
                EnergySlider EnergySliderScript = Slider.GetComponent<EnergySlider>();
                EnergySliderScript.SetEnergySlider(currentEnergy);

            }

            energyText.text = currentEnergy.ToString() + "/" + TotalEnergy.ToString();
        }
        else
        {
            Debug.Log("Not enough Energy to do the desired Action!");
        }
     
    }

    public void SetEnergyTo(int energy)
    {
        currentEnergy = energy;

        if (Slider != null)
        {
            EnergySlider EnergySliderScript = Slider.GetComponent<EnergySlider>();
            EnergySliderScript.SetEnergySlider(energy);

        }
        energyText.text = currentEnergy.ToString() + "/" + TotalEnergy.ToString();
    }

    public int EnergyCost(int i)
    {
        //i stands for the action type (1 = plowing, 2 = seeding, 3 = harvesting)
        return  EnergyCostList[i];
        

    }
}
