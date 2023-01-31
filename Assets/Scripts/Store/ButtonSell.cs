using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonSell : MonoBehaviour
{
    [SerializeField] private PlayerController _player_controller;
    [SerializeField] private GameObject _grid_view;
    [SerializeField] private TextMeshProUGUI _total_profit;

    public void SellItems()
    {
        foreach (ItemGridEntrySelling item_slot in _grid_view.GetComponentsInChildren<ItemGridEntrySelling>())
        {
            // TODO Add profit here
            _player_controller.increaseCurrentMoney(item_slot.GetItem().prize, item_slot.GetItem().amount);
            // _player_controller.GetPlayerInventory().AddItem(item_slot.GetItem());
            Destroy(item_slot.gameObject);
        }

        _total_profit.text = "0" + Utils.Constants.SHOP_PRICE_POSTFIX;
    }
}
