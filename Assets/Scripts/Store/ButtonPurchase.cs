using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPurchase : MonoBehaviour
{
    [SerializeField] private PurchaseHandler _purchase_handler;
    [SerializeField] private GameObject _grid_view;
    [SerializeField] private PlayerController _player_inventory;
    [SerializeField] private PlayerController _player_controller;

    public void PurchaseSelection()
    {
        if(!CheckPurchaseable())
        {
            return;
        }
        foreach (ItemGridEntryPurchase entry in _grid_view.GetComponentsInChildren<ItemGridEntryPurchase>())
        {
            if (entry.GetItem().amount > 0)
            {
                // Pop Item from store in case it's a single purchase
                entry.CheckAndRemoveFromStore();
                
                if(entry.GetItem().itemType != Item.ItemType.chicken_upgrade)
                {
                    _player_controller.decreaseCurrentMoney(entry.GetItem().prize, entry.GetItem().amount);
                    _player_inventory.GetPlayerInventory().AddItem(entry.DuplicateItem());
                    entry.Reset();
                }
                else
                {
                    //Vector2 position = new Vector2(chickenPrefab.startPositionX,chickenPrefab.startPositionY);
                    //Instantiate(chickenPrefab, position, Quaternion.identity);
                    SceneLoader.Instance.setChickenState(true);
                    Destroy(entry.gameObject);
                    _player_controller.decreaseCurrentMoney(entry.GetItem().prize, entry.GetItem().amount);
                }
            }
        }
        _purchase_handler.ResetCurrentTotal();
    }
    private bool CheckPurchaseable()
    {
        int totalPrice = _purchase_handler.ParseCurrentPrice();
        // TODO Check for player money
        int currentMoney = _player_controller.getCurrentMoney();
       
        if (currentMoney >= totalPrice)
        {
            return true;
        }
        return false;
    }
}
