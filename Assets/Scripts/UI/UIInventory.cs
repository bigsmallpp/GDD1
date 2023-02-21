using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    private bool UIisActive;
    private PlayerController _player;

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    public void SetPlayer(PlayerController player)
    {
        _player = player;
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void Awake ()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");

        UIisActive = false;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void RefreshInventoryItems()
    {
        if (itemSlotContainer != null)
        {
            foreach (Transform child in itemSlotContainer)
            {
                if (child == itemSlotTemplate) continue;
                Destroy(child.gameObject);
            }

            int x = 0;
            int y = 0;
            float itemSlotCellSize = 60f;

            foreach (Item item in inventory.GetItemList())
            {
                GameObject new_item = Instantiate(itemSlotTemplate, itemSlotContainer).gameObject;
                new_item.GetComponent<InventoryInteraction>().SetItem(item);
                
                RectTransform itemsSlotRectTransform = new_item.GetComponent<RectTransform>();
                itemsSlotRectTransform.gameObject.SetActive(true);
                itemsSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);
                Image image = itemsSlotRectTransform.Find("Image").GetComponent<Image>();
                image.sprite = item.GetSprite();
                
                // TODO Pls change this
                if (item.itemType == Item.ItemType.egg)
                {
                    image.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
                }

                TextMeshProUGUI uiText = itemsSlotRectTransform.Find("Text").GetComponent<TextMeshProUGUI>();
                if (item.amount > 1)
                {
                    uiText.SetText(item.amount.ToString());
                }
                else
                {
                    uiText.SetText("");
                }
                
                x++;
                if (x > 3)
                {
                    x = 0;
                    y++;
                }
            }
        }
    }

    public void SetActiveAlternativly()
    {
        if (UIisActive)
        {
            gameObject.SetActive(false);
            UIisActive = false;
        }
        else
        {
            gameObject.SetActive(true);
            UIisActive = true;
        }
    }

    public void SetActive(bool state)
    {
        if (UIisActive == state)
        {
            return;
        }
        
        gameObject.SetActive(state);
        UIisActive = state;
    }
}
