using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPurchase : MonoBehaviour
{
    [SerializeField] private PurchaseHandler _purchase_handler;
    [SerializeField] private GameObject _grid_view;
    [SerializeField] private PlayerController _player_inventory;

    public void PurchaseSelection()
    {
        // TODO Check for player money
        foreach (ItemGridEntryPurchase entry in _grid_view.GetComponentsInChildren<ItemGridEntryPurchase>())
        {
            if (entry.GetItem().amount > 0)
            {
                // _items_to_add.Add(entry.GetItem());
                _player_inventory.GetPlayerInventory().AddItem(entry.DuplicateItem());
                entry.Reset();
            }
        }
        
        _purchase_handler.ResetCurrentTotal();
    }
}
