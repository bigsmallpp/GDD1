using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PurchaseHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text_total_price;
    [SerializeField] private GameObject _prefab_grid_entry;
    [SerializeField] private GameObject _grid_view;

    public void IncreaseTotalPrice(int amount)
    {
        int current_price = ParseCurrentPrice();
        _text_total_price.text = (current_price + amount).ToString() + Utils.Constants.SHOP_PRICE_POSTFIX;
    }
    
    public void DecreaseTotalPrice(int amount)
    {
        int current_price = ParseCurrentPrice();
        _text_total_price.text = (current_price - amount).ToString() + Utils.Constants.SHOP_PRICE_POSTFIX;
    }

    private int ParseCurrentPrice()
    {
        String price = _text_total_price.text.Split(' ')[0];

        return int.Parse(price);
    }

    public void SpawnItemEntry(Item item)
    {
        GameObject obj = Instantiate(_prefab_grid_entry, _grid_view.transform);
        obj.GetComponent<ItemGridEntryPurchase>().SetItemEntry(item);
        foreach (ItemAddRemoveHandler handler in obj.GetComponentsInChildren<ItemAddRemoveHandler>())
        {
            handler.SetParent(this);
        }
    }

    public void ResetStore()
    {
        _text_total_price.text = "0" + Utils.Constants.SHOP_PRICE_POSTFIX;
        foreach (Transform child in _grid_view.transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void ResetCurrentTotal()
    {
        _text_total_price.text = "0" + Utils.Constants.SHOP_PRICE_POSTFIX;
    }
}
