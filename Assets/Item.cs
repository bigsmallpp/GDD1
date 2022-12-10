using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item 
{
    public enum ItemType
    {
        tomato,
        potato,
        tomato_seed,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.potato: return ItemAssets.Instance.potato;
            case ItemType.tomato: return ItemAssets.Instance.tomato;
            case ItemType.tomato_seed: return ItemAssets.Instance.tomato_seed;
        }
    }

    public bool isStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.tomato:
            case ItemType.potato:
            case ItemType.tomato_seed:

                return true;
                
        }
    }
}
