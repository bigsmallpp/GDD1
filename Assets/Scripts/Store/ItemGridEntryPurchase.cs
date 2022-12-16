using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class ItemGridEntryPurchase : MonoBehaviour
{
    [Header("The Text Fields")]
    [SerializeField] private TextMeshProUGUI _text_amount;
    [SerializeField] private TextMeshProUGUI _text_add;
    [SerializeField] private TextMeshProUGUI _text_remove;
    [SerializeField] private TextMeshProUGUI _text_price;

    [Header("The Sprite To Display")]
    [SerializeField] private Image _sprite;

    private Item _item;

    private void Start()
    {
        _text_amount.text = "0";
    }

    public void SetItemEntry(Item item)
    {
        _text_price.text = item.prize.ToString() + Utils.Constants.SHOP_PRICE_POSTFIX;
        _sprite.sprite = item.GetSprite();
        _item = item;
    }

    public Item GetItem()
    {
        return _item;
    }

    public Item DuplicateItem()
    {
        Item new_item = new Item();
        new_item.Duplicate(_item);

        return new_item;
    }

    public void Reset()
    {
        _item.amount = 0;
        _text_amount.text = "0";
    }
}
