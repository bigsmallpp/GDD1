using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    [SerializeField] private GameObject SellPage;
    [SerializeField] private GameObject BuyPage;

    
    public void OnClickSellButton()
    {
        SellPage.SetActive(true);
        BuyPage.SetActive(false);
    }
    
}
