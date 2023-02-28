using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private static Chest _instance = null;
    public static Chest Instance => _instance;

    [SerializeField] private ChestInventoryHandler _chestInventory;
    [SerializeField] private TextMeshProUGUI _profit;
    private PlayerController _player;

    private void Start()
    {
        SetProfit(0);
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenOrCloseChestUI()
    {
        if (UIHandler.Instance.CheckItemBeingDragged())
        {
            return;
        }
        
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

    public void SetPlayer(PlayerController player)
    {
        _player = player;
    }

    public void CloseChest()
    {
        if (UIHandler.Instance.CheckItemBeingDragged())
        {
            return;
        }
        
        gameObject.SetActive(false);
        _player.BlockMovement(false);
    }

    public void SetProfit(int amount)
    {
        _profit.text = "Profit: " + amount + "$";
    }
}
