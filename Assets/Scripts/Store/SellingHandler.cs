using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SellingHandler : MonoBehaviour
{
    [SerializeField] private GameObject _grid_view;
    [SerializeField] private GameObject _entry_prefab;
    [SerializeField] private TextMeshProUGUI _total_price;

    private void Start()
    {
        _total_price.text = "0" + Utils.Constants.SHOP_PRICE_POSTFIX;
    }

    public void AddItemToSellingSection(Item item)
    {
        _total_price.text = (ParseCurrentPrice() + (item.prize * item.amount)).ToString() + Utils.Constants.SHOP_PRICE_POSTFIX;
        GameObject obj = Instantiate(_entry_prefab, _grid_view.transform);
        obj.GetComponent<ItemGridEntrySelling>().SetItem(item);
    }

    private int ParseCurrentPrice()
    {
        return int.Parse(_total_price.text.Replace(Utils.Constants.SHOP_PRICE_POSTFIX, ""));
    }

    public void DecreaseTotal(int amount)
    {
        _total_price.text = (ParseCurrentPrice() - amount).ToString() + Utils.Constants.SHOP_PRICE_POSTFIX;
    }
}
