using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
        
        if (CheckIsSeed(new_item))
        {
            UIHandler.Instance.UpdateSeedsInToolbar(new_item.itemType, CheckIsPresentInInventory(new_item.itemType));
        }
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

        if (CheckIsSeed(item))
        {
            UIHandler.Instance.UpdateSeedsInToolbar(item.itemType, CheckIsPresentInInventory(item.itemType));
        }
    }

    private bool CheckIsSeed(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.carrot_seed:
            case Item.ItemType.cauliflower_seed:
            case Item.ItemType.wheat_seed:
                return true;
            
            default:
                return false;
        }
    }
    
    public void RemoveItem(Item item)
    {
        // Only use this if item is not destroyed right after
        _items.Remove(item);
        _itemReferences.Remove(item);
        
        if (CheckIsSeed(item))
        {
            UIHandler.Instance.UpdateSeedsInToolbar(item.itemType, CheckIsPresentInInventory(item.itemType));
        }
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
                PruneEntries(itemInInventory);
            }
        }
        else
        {
            PruneEntries(item);
        }
        
        if (CheckIsSeed(item))
        {
            UIHandler.Instance.UpdateSeedsInToolbar(item.itemType, CheckIsPresentInInventory(item.itemType));
        }
    }

    public void PruneEntries(Item item)
    {
        List<Item> items_to_remove = new List<Item>();
        foreach (Item i in _items)
        {
            if (i.itemType == item.itemType)
            {
                items_to_remove.Add(i);
            }
        }

        foreach (Item i in items_to_remove)
        {
            _items.Remove(i);
            GameObject obj = _itemReferences[i];
            _itemReferences.Remove(i);
            Destroy(obj);
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

    public void CreateNewItemEntry(Item item)
    {
        GameObject obj = Instantiate(_itemSlotTemplate, _itemContents.transform);
        obj.GetComponent<InventoryInteraction>().SetItem(item);
        
        // Might be redundant
        obj.GetComponent<ItemDrag>().SetPreviousParent(_itemContents.transform);
        obj.GetComponent<ItemDrag>().SetInInventory(true);
        obj.SetActive(_itemContents.activeSelf);
        _itemReferences.Add(item, obj);
    }

    public void UpdateItemStats(Item item)
    {
        InventoryInteraction inv = _itemReferences[item].GetComponent<InventoryInteraction>();
        inv.UpdateItem();
    }

    public void SumUpOccurences(Item item)
    {
        int total = 0;
        foreach(Item inventoryItem in _items)
        {
            total += inventoryItem.amount;
        }

        int amount_stacks = total / Utils.Constants.MAX_STACKS;
        if (amount_stacks == 0) amount_stacks = 1;

        List<Item> new_items = new List<Item>();
        for (int i = 0; i < amount_stacks; i++)
        {
            Item temp = new Item();
            temp.Duplicate(item);
            temp.amount = i == amount_stacks - 1 ? total : Utils.Constants.MAX_STACKS;
            total -= temp.amount;
            
            new_items.Add(temp);
        }
        
        PruneEntries(item);
        foreach (Item i in new_items)
        {
            _items.Add(i);
            CreateNewItemEntry(i);
            UpdateItemStats(i);
        }
    }

    public void AddItemAfterSplit(Item item)
    {
        _items.Add(item);
    }

    private bool CheckIsPresentInInventory(Item.ItemType type)
    {
        foreach(Item inventoryItem in _items)
        {
            if (inventoryItem.itemType == type)
            {
                return true;
            }
        }

        return false;
    }
}
