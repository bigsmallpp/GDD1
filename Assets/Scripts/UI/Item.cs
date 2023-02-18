using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item 
{

    public Item(int type, int amount, int price)
    {
        itemType = (ItemType) type;
        this.amount = amount;
        prize = price;
    }
    
    public Item(ItemType type, int amount, int price)
    {
        itemType = type;
        this.amount = amount;
        prize = price;
    }

    public Item()
    {
        
    }
    
    public enum ItemType
    {
        tomato,
        potato,
        tomato_seed,
        chicken_upgrade,
        egg
    }

    public ItemType itemType;
    public int amount;
    public int prize;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            case ItemType.potato: return ItemAssets.Instance.potato;
            case ItemType.tomato: return ItemAssets.Instance.tomato;
            case ItemType.tomato_seed: return ItemAssets.Instance.tomato_seed;
            case ItemType.chicken_upgrade: return ItemAssets.Instance.chicken_upgrade;
            case ItemType.egg: return ItemAssets.Instance.egg;
            
            default:
                throw new NotImplementedException();
        }
    }

    public bool isStackable()
    {
        switch (itemType)
        {
            case ItemType.tomato:
            case ItemType.potato:
            case ItemType.tomato_seed:
            case ItemType.egg:
                return true;
            
            default:
                return false;
        }
    }

    public void Duplicate(Item item)
    {
        itemType = item.itemType;
        amount = item.amount;
        prize = item.prize;
    }
}
