using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private static Chest _instance = null;
    public static Chest Instance => _instance;

    [SerializeField] private ChestInventoryHandler _chestInventory;
    private PlayerController _player;

    private void Start()
    {
        if (!_instance)
        {
            _instance = this;
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenOrCloseChestUI()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void AddItem(GameObject obj)
    {
        _chestInventory.AddItemToChest(obj);
    }
    
    public void RemoveItem(GameObject obj)
    {
        _chestInventory.RemoveItemFromChest(obj);
    }

    public void SellItemsInChest()
    {
        int profit = _chestInventory.SellItems();
        _player.AddProfit(profit);
        Debug.Log("Made a profit of " + profit);
    }
}
