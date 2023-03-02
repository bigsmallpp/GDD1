using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemEntry : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private Outline _outline;
    [SerializeField] private Item _item;

    [SerializeField] private Store _store;

    void Awake()
    {
        _item.SetPrice();
        _name.text = _item.GetName();
        _price.text = "(" + _item.prize + "$)";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImage(Sprite sprite)
    {
        _sprite = sprite;
    }

    public void SetItemSelected()
    {
        _store.DeselectAll();
        Select();
        
        _store.GetItemDescriptionBox().SetDescription(ItemDescriptions.GetDescriptionForStore(_item.itemType));
        _store.GetItemDescriptionBox().SetSprite(_sprite);
        _store.GetItemDescriptionBox().SetItemReference(_item);
        _store.GetItemDescriptionBox().EnablePurchaseButton();
    }

    public void Deselect()
    {
        _outline.enabled = false;
    }

    public void Select()
    {
        _outline.enabled = true;
    }
}
