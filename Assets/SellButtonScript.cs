using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButtonScript : MonoBehaviour
{
    
    [SerializeField] Inventory inventory;
    [SerializeField] UIMerchant uiMerchant;

    public void OnPressed(Item item)
    {
        //inventory.RemoveItem(new Item {itemType = Item.ItemType.potato, amount = 1, prize = 10});
        Debug.Log("Pressed!");
    }

}
