using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class InventoryInteraction : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private PlayerController _player_controller;
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TextMeshProUGUI _itemCount;

    private void Start()
    {
        _player_controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _itemSprite.sprite = _item.GetSprite();
    }

    public void SetItem(Item item)
    {
        _item = item;
        UpdateItem();
    }

    public Item GetItem()
    {
        return _item;
    }

    public void UpdateItem()
    {
        _itemCount.text = _item.amount.ToString();
        _itemSprite.sprite = _item.GetSprite();
    }
}
