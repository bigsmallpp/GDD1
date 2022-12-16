using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPurchase : MonoBehaviour
{
    [SerializeField] private PurchaseHandler _purchase_handler;
    [SerializeField] private GameObject _grid_view;
    [SerializeField] private PlayerController _player_inventory;
    public AnimalScript chickenPrefab;

    public void PurchaseSelection()
    {
        // TODO Check for player money
        foreach (ItemGridEntryPurchase entry in _grid_view.GetComponentsInChildren<ItemGridEntryPurchase>())
        {
            if (entry.GetItem().amount > 0)
            {
                // Pop Item from store in case it's a single purchase
                entry.CheckAndRemoveFromStore();
                
                if(entry.GetItem().itemType != Item.ItemType.chicken_upgrade)
                {
                    _player_inventory.GetPlayerInventory().AddItem(entry.DuplicateItem());
                    entry.Reset();
                }
                else
                {
                    Vector2 position = new Vector2(chickenPrefab.startPositionX,chickenPrefab.startPositionY);
                    Instantiate(chickenPrefab, position, Quaternion.identity);
                    Destroy(entry.gameObject);
                }
            }
        }
        
        _purchase_handler.ResetCurrentTotal();
    }
}
