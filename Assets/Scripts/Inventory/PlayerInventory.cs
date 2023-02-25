using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<Item> _items;
    [SerializeField] private GameObject _itemContents;
    [SerializeField] private GameObject _itemSlotTemplate;
    [SerializeField] private Dictionary<Item, GameObject> _itemReferences = new Dictionary<Item, GameObject>();

    private bool isActive = false;
    
    public PlayerInventory()
    {
        _items = new List<Item>();
    }

    public PlayerInventory(List<Item> inventory)
    {
        _items = inventory;
    }

    public void TransferItem(GameObject obj)
    {
        Item new_item = obj.GetComponent<InventoryInteraction>().GetItem();
        _items.Add(new_item);
        
        _itemReferences.Add(new_item, obj);
        obj.GetComponent<InventoryInteraction>().SetItem(new_item);
        obj.transform.parent = _itemContents.transform;
    }

    public void AddItem(Item item)
    {
        if (item.isStackable())
        {
            foreach(Item inventoryItem in _items)
            {
                if (item.amount == 0)
                {
                    return;
                }
                
                if(inventoryItem.itemType == item.itemType && inventoryItem.amount < Utils.Constants.MAX_STACKS)
                {
                    int free_space = Utils.Constants.MAX_STACKS - inventoryItem.amount;
                    if (item.amount > free_space)
                    {
                        inventoryItem.amount += free_space;
                        item.amount -= free_space;
                        UpdateItemStats(inventoryItem);
                        continue;
                    }
                    
                    inventoryItem.amount += item.amount;
                    UpdateItemStats(inventoryItem);
                    return;
                }
            }
        }

        _items.Add(item);
        CreateNewItemEntry(item);
        UpdateItemStats(item);
    }
    
    public void RemoveItem(Item item)
    {
        // Only use this if item is not  destroyed right after
        _items.Remove(item);
        _itemReferences.Remove(item);
    }

    public void DecreaseItem(Item item, int amount)
    {
        if(item.isStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in _items)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= amount;
                    itemInInventory = inventoryItem;
                    UpdateItemStats(itemInInventory);
                }
            }
            if(itemInInventory != null && itemInInventory.amount <= 0)
            {
                _items.Remove(itemInInventory);
                _itemReferences.Remove(itemInInventory);
            }
        }
        else
        {
            _items.Remove(item);
            _itemReferences.Remove(item);
        }
    }

    public List<Item> GetItems()
    {
        return _items;
    }
    
    public void SetActiveAlternativly()
    {
        if (UIHandler.Instance.CheckItemBeingDragged())
        {
            return;
        }
        
        if (isActive)
        {
            gameObject.SetActive(false);
            isActive = false;
        }
        else
        {
            gameObject.SetActive(true);
            isActive = true;
        }
    }

    public void SetActive(bool state)
    {
        if (isActive == state || UIHandler.Instance.CheckItemBeingDragged())
        {
            return;
        }
        
        gameObject.SetActive(state);
        isActive = state;
    }

    private void CreateNewItemEntry(Item item)
    {
        GameObject obj = Instantiate(_itemSlotTemplate, _itemContents.transform);
        obj.GetComponent<InventoryInteraction>().SetItem(item);
        
        // Might be redundant
        obj.GetComponent<ItemDrag>().SetPreviousParent(_itemContents.transform);
        obj.GetComponent<ItemDrag>().SetInInventory(true);
        _itemReferences.Add(item, obj);
    }

    private void UpdateItemStats(Item item)
    {
        InventoryInteraction inv = _itemReferences[item].GetComponent<InventoryInteraction>();
        inv.UpdateItem();
    }
}
