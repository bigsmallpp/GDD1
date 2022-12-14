using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButtonSkript : MonoBehaviour
{
    [SerializeField] private GameObject SellPage;
    [SerializeField] private GameObject BuyPage;

    public void OnClickBuyButton()
    {
        SellPage.SetActive(false);
        BuyPage.SetActive(true);
    }
}
