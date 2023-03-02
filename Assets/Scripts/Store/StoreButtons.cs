using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreButtons : MonoBehaviour
{
    [SerializeField] private Store _store;
    [SerializeField] private ItemDescriptionBox _itemBox;
    [SerializeField] private AudioSource click_Sound;
    

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
        click_Sound.Play();
        SaveManager.Instance.UpdatePlayerMoneyAndInventory(_store.GetMoney());
        SaveManager.Instance.UpdateStoreData(_store.GetDataStore());
        SceneLoader.Instance.loadScene(6);
    }

    public void IncreaseItemAmount()
    {
        click_Sound.Play();
        _itemBox.IncreaseItemAmount();
    }
    
    public void DecreaseItemAmount()
    {
        click_Sound.Play();
        _itemBox.DecreaseItemAmount();
    }

    public void PurchaseItem()
    {
        click_Sound.Play();
        _itemBox.PurchaseItem();
    }
    
    public void playClickSound()
    {
        click_Sound.Play();
    }
}
