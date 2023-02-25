using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MoneyDisplay : MonoBehaviour
{
    public PlayerController playerController;
    public TMP_Text moneyText;

    // Start is called before the first frame update
    void Start()
    {
        moneyText.text = playerController.currentMoney.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = playerController.currentMoney.ToString();
    }
}
