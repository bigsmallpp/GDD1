using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _increase;
    [SerializeField] private TextMeshProUGUI _decrease;
    [SerializeField] private TextMeshProUGUI _amount;
    
    [SerializeField] private Image _image;
    [SerializeField] private Button _purchase;

    [SerializeField] private Item _itemReference;
    [SerializeField] private Store _store;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDescription(string text)
    {
        _description.text = text;
    }

    public void SetSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public void SetItemReference(Item reference)
    {
        _itemReference = reference;
        ResetAmount();
    }

    public void IncreaseItemAmount()
    {
        if (!CheckChangeAmountPossible(_itemReference))
        {
            return;
        }
        
        if (!CheckIncreaseParameters())
        {
            return;
        }

        int new_amount = int.Parse(_amount.text) + 1;
        _itemReference.amount = new_amount;
        _amount.text = new_amount.ToString();
    }

    public void DecreaseItemAmount()
    {
        if (!CheckChangeAmountPossible(_itemReference))
        {
            return;
        }

        if (!CheckDecreaseParameters())
        {
            return;
        }
        
        int new_amount = int.Parse(_amount.text) - 1;
        _itemReference.amount = new_amount;
        _amount.text = new_amount.ToString();
    }

    private bool CheckChangeAmountPossible(Item item)
    {
        Item.ItemType[] static_items =
        {
            Item.ItemType.chicken_upgrade, Item.ItemType.lamp, Item.ItemType.scissor, Item.ItemType.bucket, 
            Item.ItemType.cow_upgrade, Item.ItemType.sheep_upgrade
        };
        
        return !static_items.Contains(item.itemType);
    }

    private bool CheckIncreaseParameters()
    {
        int current_amount = int.Parse(_amount.text);
        if (current_amount == int.MaxValue || current_amount == Utils.Constants.MAX_STACKS)
        {
            return false;
        }

        int new_amount = current_amount + 1;
        if (new_amount * _itemReference.prize > _store.GetMoney())
        {
            return false;
        }

        return true;
    }
    
    private bool CheckDecreaseParameters()
    {
        int current_amount = int.Parse(_amount.text);
        if (current_amount == 1)
        {
            return false;
        }

        return true;
    }

    public void ResetAmount()
    {
        _amount.text = "1";
        _itemReference.amount = 1;
    }

    public void PurchaseItem()
    {
        if (_itemReference.itemType == Item.ItemType.chicken_upgrade || _itemReference.itemType == Item.ItemType.sheep_upgrade ||
            _itemReference.itemType == Item.ItemType.cow_upgrade)
        {
            _store.HidePermanentItem(_itemReference.itemType);
            SceneLoader.Instance.EnableAnimal(_itemReference.itemType);
            _store.InitFirstItemEntry();
        }
        else if (_itemReference.itemType == Item.ItemType.bucket || _itemReference.itemType == Item.ItemType.lamp ||
                 _itemReference.itemType == Item.ItemType.scissor)
        {
            // TODO Add Item To Playerbar
            AddItemToToolbar(_itemReference.itemType);
            _store.HidePermanentItem(_itemReference.itemType);
            _store.InitFirstItemEntry();
        }
        else
        {
            _store.AddItemToPlayer(_itemReference);
            ResetAmount();
        }
    }

    private void AddItemToToolbar(Item.ItemType type)
    {
        if (type == Item.ItemType.bucket)
        {
            UIHandler.Instance.UnlockTool(SelectedToolHighlighted.ToolbarIndices.Bucket);
        }
        else if (type == Item.ItemType.scissor)
        {
            UIHandler.Instance.UnlockTool(SelectedToolHighlighted.ToolbarIndices.Scissors);
        }
        else if (type == Item.ItemType.lamp)
        {
            TimeManager.Instance.UnlockLantern();
        }
        else
        {
            Debug.LogError("Unknown Item " + type);
        }
    }
}
