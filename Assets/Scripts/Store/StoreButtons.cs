using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreButtons : MonoBehaviour
{
    [SerializeField] private Store _store;
    [SerializeField] private ItemDescriptionBox _itemBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeaveStore()
    {
        SaveManager.Instance.UpdatePlayerMoneyAndInventory(_store.GetMoney());
        SaveManager.Instance.UpdateStoreData(_store.GetDataStore());
        SceneLoader.Instance.loadScene(6);
    }

    public void IncreaseItemAmount()
    {
        _itemBox.IncreaseItemAmount();
    }
    
    public void DecreaseItemAmount()
    {
        _itemBox.DecreaseItemAmount();
    }

    public void PurchaseItem()
    {
        _itemBox.PurchaseItem();
    }
}
