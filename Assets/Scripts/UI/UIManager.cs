using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMPro.TMP_Text GeldUI;
    

    

    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject thePlayer = GameObject.FindWithTag("Player");
            //playerScript.currentMoney += 10;
            //GeldUI.text = playerScript.currentMoney.ToString();
            if (thePlayer != null)
            {
                PlayerController playerScript = thePlayer.GetComponent<PlayerController>();
                playerScript.currentMoney += 10;
                GeldUI.text = playerScript.currentMoney.ToString();
            }


        }
    }

    
}
