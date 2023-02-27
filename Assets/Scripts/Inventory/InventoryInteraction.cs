using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class InventoryInteraction : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Item _item;
    [SerializeField] private PlayerController _player_controller;
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TextMeshProUGUI _itemCount;

    private bool right_clicked = false;
    private bool shift_down = false;

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

    public void SplitItem()
    {
        if (right_clicked && !shift_down)
        {
            if (_item.amount == 1)
            {
                return;
            }
            int half = _item.amount / 2;

            Item new_item = new Item();
            new_item.Duplicate(_item);
            new_item.amount = half;
            _item.amount -= half;

            _player_controller.GetPlayerInventory().CreateNewItemEntry(new_item);
            _player_controller.GetPlayerInventory().AddItemAfterSplit(new_item);
            _player_controller.GetPlayerInventory().UpdateItemStats(_item);
            _player_controller.GetPlayerInventory().UpdateItemStats(new_item);
            right_clicked = false;
        }
        else if (right_clicked && shift_down)
        {
            _player_controller.GetPlayerInventory().SumUpOccurences(_item);
            right_clicked = false;
            shift_down = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        right_clicked = eventData.button == PointerEventData.InputButton.Right;
        shift_down = Input.GetKey(KeyCode.LeftShift);
    }
}
