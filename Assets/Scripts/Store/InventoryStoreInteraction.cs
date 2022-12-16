using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStoreInteraction : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private PlayerController _player_controller;
    private SellingHandler _selling_handler;

    private void Start()
    {
        _selling_handler = _player_controller.GetStore().GetComponentInChildren<SellingHandler>();
    }

    public void SetItem(Item item)
    {
        _item = item;
    }

    public void SendItemToStore()
    {
        if (GameObject.Find("UIStore") == null)
        {
            return;
        }
        
        Item new_item = new Item();
        new_item.Duplicate(_item);
        _player_controller.GetPlayerInventory().RemoveItem(_item);
        
        _item = new_item;
        _selling_handler.AddItemToSellingSection(_item);
    }
}
