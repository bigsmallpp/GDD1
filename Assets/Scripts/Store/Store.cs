using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private List<Item> _items_in_store;
    
    [Header("Handlers For The Purchasing/Selling Sections")]
    [SerializeField] private PurchaseHandler _purchase_handler;
    [SerializeField] private SellingHandler _selling_handler;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenOrClose()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            ResetStore();
        }
    }

    private void ResetStore()
    {
        _purchase_handler.ResetStore();
        
        foreach (Item item in _items_in_store)
        {
            _purchase_handler.SpawnItemEntry(item);
        }
    }

    public void RemoveItem(Item item)
    {
        _items_in_store.Remove(item);
    }
}
