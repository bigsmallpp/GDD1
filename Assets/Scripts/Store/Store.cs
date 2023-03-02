using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] private PlayerInventory _player_inventory;
    [SerializeField] private int _money;
    [SerializeField] private ItemDescriptionBox _itemDescriptionBox;
    [SerializeField] private GameObject _itemEntries;
    [SerializeField] private List<GameObject> _items;
    [SerializeField] private AudioSource purchase_Sound;
    [SerializeField] private TextMeshProUGUI money_text;
    
    private StoreDataStore _storeData;

    enum ItemIndices
    {
        chicken = 3,
        cow = 4,
        sheep = 5,
        bucket = 6,
        lamp = 7,
        scissors = 8
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _storeData = SaveManager.Instance.LoadStoreData();
        _player_inventory = UIHandler.Instance.GetPlayerInventory();
        _money = SaveManager.Instance.GetPlayerMoney();
        money_text.text = _money + "$";
        DeselectAll();
        InitFirstItemEntry();
        CheckDisablePermanentItems();
    }

    public StoreDataStore GetDataStore()
    {
        return _storeData;
    }

    public int GetMoney()
    {
        return _money;
    }

    private void CheckDisablePermanentItems()
    {
        if (!_storeData.chickenAvailable)
        {
            _items[(int) ItemIndices.chicken].gameObject.SetActive(false);
        }

        if (!_storeData.cowAvailable)
        {
            _items[(int) ItemIndices.cow].gameObject.SetActive(false);
        }

        if (!_storeData.sheepAvailable)
        {
            _items[(int) ItemIndices.sheep].gameObject.SetActive(false);
        }

        if (!_storeData.lampAvailable)
        {
            _items[(int) ItemIndices.lamp].gameObject.SetActive(false);
        }

        if (!_storeData.bucketAvailable)
        {
            _items[(int) ItemIndices.bucket].gameObject.SetActive(false);
        }

        if (!_storeData.scissorAvailable)
        {
            _items[(int) ItemIndices.scissors].gameObject.SetActive(false);
        }
    }

    public ItemDescriptionBox GetItemDescriptionBox()
    {
        return _itemDescriptionBox;
    }

    public void DeselectAll()
    {
        foreach (ItemEntry entry in _itemEntries.GetComponentsInChildren<ItemEntry>())
        {
            entry.Deselect();
        }
    }

    public void InitFirstItemEntry()
    {
        _itemEntries.GetComponentsInChildren<ItemEntry>()[0].SetItemSelected();
    }

    public void AddItemToPlayer(Item item_reference)
    {
        Item new_item = new Item();
        new_item.Duplicate(item_reference);
        
        _player_inventory.AddItem(new_item);
        _money -= new_item.prize * new_item.amount;
        money_text.text = _money + "$";
    }

    public void HidePermanentItem(Item.ItemType type)
    {
        switch (type)
        {
            case Item.ItemType.bucket:
                _storeData.bucketAvailable = false;
                break;
            case Item.ItemType.lamp:
                _storeData.lampAvailable = false;
                break;
            case Item.ItemType.scissor:
                _storeData.scissorAvailable = false;
                break;
            case Item.ItemType.chicken_upgrade:
                _storeData.chickenAvailable = false;
                SceneLoader.Instance.setChickenState(true);
                break;
            case Item.ItemType.cow_upgrade:
                _storeData.cowAvailable = false;
                SceneLoader.Instance.cowAlive = true;
                break;
            case Item.ItemType.sheep_upgrade:
                _storeData.sheepAvailable = false;
                SceneLoader.Instance.sheepAlive = true;
                break;
        }
        
        CheckDisablePermanentItems();
    }
    
    public void playPurchaseSound()
    {
        purchase_Sound.Play();
    }

    public void DecreaseMoney(int amount)
    {
        _money -= amount;
        money_text.text = _money + "$";
    }
}
