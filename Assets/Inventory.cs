using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory 
{
    private List<Item> itemList;
    private List<Item> merchantItemList;

    public event EventHandler OnItemListChanged;
    public Inventory()
    {
        itemList = new List<Item>();
        merchantItemList = new List<Item>();
    }

    
    public void AddItem(Item item)
    {
        if (item.isStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach(Item inventoryItem in itemList)
            {
                if(inventoryItem.itemType == item.itemType) {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        } else
        {
            itemList.Add(item);
        }
        
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }


    // Item aus Inventar abziehen 
    public void RemoveItem(Item item)
    {
        
        if(item.isStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;

                }
                
                
            }
            if(itemInInventory.amount <= 0 && itemInInventory != null)
            {
                itemList.Remove(itemInInventory);
                
            }
            
            
        } else
        {
            itemList.Remove(item);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public List<Item> GetMerchantItemList()
    {
        return merchantItemList;
    }
    

    
}


