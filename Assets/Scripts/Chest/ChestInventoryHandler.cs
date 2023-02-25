using System;
using System.Collections;
using System.Collections.Generic;
using ImmersiveVRTools.Runtime.Common.Extensions;
using UnityEngine;

public class ChestInventoryHandler : MonoBehaviour
{
    [SerializeField] private List<Item> _itemsInChest = new List<Item>();
    [SerializeField] private GameObject _contentOfChest;

    public void AddItemToChest(GameObject obj)
    {
        Item item = obj.GetComponent<InventoryInteraction>().GetItem();
        _itemsInChest.Add(item);
        obj.GetComponent<InventoryInteraction>().SetItem(item);

        obj.transform.parent = _contentOfChest.transform;
    }
    
    public void RemoveItemFromChest(GameObject obj)
    {
        Item item = obj.GetComponent<InventoryInteraction>().GetItem();
        _itemsInChest.Remove(item);
    }

    public int SellItems()
    {
        int profit = 0;
        foreach(InventoryInteraction inv in _contentOfChest.transform.GetComponentsInChildren<InventoryInteraction>())
        {
            Item item = inv.GetItem();
            profit += item.prize * item.amount;
            Destroy(inv.gameObject);
        }
        
        _itemsInChest.Clear();

        return profit;
    }
}
