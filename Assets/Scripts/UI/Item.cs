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
        egg,
        wheat_seed,
        cauliflower_seed,
        carrot_seed,
        cow_upgrade,
        sheep_upgrade,
        wheat,
        carrot,
        cauliflower,
        lamp,
        bucket,
        scissor
    }

    public enum ItemPrices
    {
        cauliflower_seed = 125,
        carrot_seed = 100,
        wheat_seed = 75,
        cauliflower = 300,
        carrot = 225,
        wheat = 125,
        chicken = 500,
        cow = 1500,
        sheep = 2000,
        lamp = 800,
        bucket = 550,
        egg = 250,
        scissors = 600
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
            case ItemType.wheat_seed: return ItemAssets.Instance.tomato_seed;
            case ItemType.carrot: return ItemAssets.Instance.carrot;
            case ItemType.cauliflower: return ItemAssets.Instance.cauliflower;
            case ItemType.wheat: return ItemAssets.Instance.wheat;
            
            default:
                return ItemAssets.Instance.chicken_upgrade;
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
            case ItemType.carrot:
            case ItemType.carrot_seed:
            case ItemType.cauliflower:
            case ItemType.cauliflower_seed:
            case ItemType.wheat:
            case ItemType.wheat_seed:
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

    public void SetPrice()
    {
        switch(itemType)
        {
            case ItemType.carrot_seed:
                prize = (int) ItemPrices.carrot_seed;
                break;
            case ItemType.cauliflower_seed:
                prize = (int) ItemPrices.cauliflower_seed;
                break;
            case ItemType.chicken_upgrade:
                prize = (int) ItemPrices.chicken;
                break;
            case ItemType.cow_upgrade:
                prize = (int) ItemPrices.cow;
                break;
            case ItemType.egg:
                prize = (int)ItemPrices.egg;
                break;
            case ItemType.sheep_upgrade:
                prize = (int) ItemPrices.sheep;
                break;
            case ItemType.wheat_seed:
                prize = (int) ItemPrices.wheat_seed;
                break;
            case ItemType.wheat:
                prize = (int) ItemPrices.wheat;
                break;
            case ItemType.carrot:
                prize = (int) ItemPrices.carrot;
                break;
            case ItemType.cauliflower:
                prize = (int) ItemPrices.cauliflower;
                break;
            case ItemType.lamp:
                prize = (int) ItemPrices.lamp;
                break;
            case ItemType.bucket:
                prize = (int) ItemPrices.bucket;
                break;
            case ItemType.scissor:
                prize = (int) ItemPrices.scissors;
                break;
            default:
                Debug.LogError("Unknown ItemType");
                break;
        }
    }

    public string GetName()
    {
        string name = "";
        switch(itemType)
        {
            
            case ItemType.carrot_seed:
                name = "Carrot Seed";
                break;
            case ItemType.cauliflower_seed:
                name = "Cauliflower Seed";
                break;
            case ItemType.chicken_upgrade:
                name = "Chicken";
                break;
            case ItemType.cow_upgrade:
                name = "Cow";
                break;
            case ItemType.egg:
                name = "Egg";
                break;
            case ItemType.sheep_upgrade:
                name = "Sheep";
                break;
            case ItemType.wheat_seed:
                name = "Wheat Seed";
                break;
            case ItemType.wheat:
                name = "Wheat";
                break;
            case ItemType.carrot:
                name = "Carrot";
                break;
            case ItemType.cauliflower:
                name = "Cauliflower";
                break;
            case ItemType.bucket:
                name = "Bucket";
                break;
            case ItemType.scissor:
                name = "Scissors";
                break;
            case ItemType.lamp:
                name = "Lamp";
                break;
            default:
                Debug.LogError("Unknown ItemType");
                break;
        }

        return name;
    }
}
