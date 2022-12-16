using System;
using System.Collections;
using System.Collections.Generic;
using Image = UnityEngine.UI.Image;
using TMPro;
using UnityEngine;

public class ItemGridEntrySelling : MonoBehaviour
{
    [Header("The Text Fields")]
    [SerializeField] private TextMeshProUGUI _text_price;
    
    [Header("The Sprite To Display")]
    [SerializeField] private Image _sprite;
    
    [Header("The Item")]
    [SerializeField] private Item _item;

    public void SetItem(Item item)
    {
        _item = item;
        _text_price.text = (item.prize * item.amount).ToString() + Utils.Constants.SHOP_PRICE_POSTFIX;
        _sprite.sprite = item.GetSprite();
    }

    public Item GetItem()
    {
        return _item;
    }
    
    public void SendItemBackToPlayerInventory()
    {
        GameObject.Find("SellSection").GetComponent<SellingHandler>().DecreaseTotal(_item.prize * _item.amount);
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().GetPlayerInventory().AddItem(_item);
        _item = null;
        Destroy(gameObject);
    }
}
