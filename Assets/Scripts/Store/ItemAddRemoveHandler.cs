using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemAddRemoveHandler : MonoBehaviour
{
    [SerializeField] private PurchaseHandler _parent;
    [SerializeField] private TextMeshProUGUI _amount;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private ItemGridEntryPurchase _grid_entry;

    public void IncreaseAmount()
    {
        int current_amount = int.Parse(_amount.text);
        if (CheckSinglePurchaseMaximum(current_amount) || current_amount == 9)
        {
            return;
        }
        
        _amount.text = (current_amount + 1).ToString();
        
        _parent.IncreaseTotalPrice(int.Parse(_price.text.Replace(" $", "")));
        _grid_entry.GetItem().amount = current_amount + 1;
    }
    
    public void DecreaseAmount()
    {
        int current_amount = int.Parse(_amount.text);
        if (current_amount == 0)
        {
            return;
        }

        _amount.text = (current_amount - 1).ToString();
        _parent.DecreaseTotalPrice(int.Parse(_price.text.Replace(" $", "")));
        _grid_entry.GetItem().amount = current_amount - 1;
    }

    public void SetParent(PurchaseHandler handler)
    {
        _parent = handler;
        _grid_entry = transform.parent.GetComponent<ItemGridEntryPurchase>();
    }

    private bool CheckSinglePurchaseMaximum(int current_amount)
    {
        return _grid_entry._is_single_purchase && current_amount == 1;
    }
}
