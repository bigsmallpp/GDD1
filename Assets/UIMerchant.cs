using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIMerchant : MonoBehaviour
{
    private Inventory inventory;

    [SerializeField] private PlayerController player;

    private Transform itemSlotContainer_Sell;
    private Transform itemSlotTemplate_Sell;

    private Transform itemSlotContainer_Buy;
    private Transform itemSlotTemplate_Buy;

    private Transform sellPage;
    private Transform buyPage;

    private Button itemButton;

    private List<Item> merchantListedItems;

    private bool UIisActive;
    public Item itemToSell;

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();

    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();

    }

    private void Awake()
    {
        sellPage = transform.Find("SellPage");
        itemSlotContainer_Sell = sellPage.Find("ItemSlotContainer");
        itemSlotTemplate_Sell = itemSlotContainer_Sell.Find("ItemSlotTemplate");

        buyPage = transform.Find("BuyPage");
        itemSlotContainer_Buy = buyPage.Find("ItemSlotContainer");
        itemSlotTemplate_Buy = itemSlotContainer_Buy.Find("ItemSlotTemplate");

        sellPage.gameObject.SetActive(false);
        buyPage.gameObject.SetActive(true);

    }

    private void Start()
    {
        merchantListedItems = inventory.GetMerchantItemList();

        merchantListedItems.Add(new Item { itemType = Item.ItemType.potato,         amount = 1, price = 10 });
        merchantListedItems.Add(new Item { itemType = Item.ItemType.tomato,         amount = 1, price = 15 });
        merchantListedItems.Add(new Item { itemType = Item.ItemType.tomato_seed,    amount = 1, price = 20 });

        
        BuyPageInventory();
    }

    // picturing the same items as in the inventory with the difference that a button is spawned and with the click on the item button
    // the corresponding item is sold

    private void RefreshInventoryItems()
    {
        if (itemSlotContainer_Sell != null)
        {
            foreach (Transform child in itemSlotContainer_Sell)
            {
                if (child == itemSlotTemplate_Sell) continue;
                Destroy(child.gameObject);
            }

            int x = 0;
            int y = 0;
            float itemSlotCellSize = 80f;

            foreach (Item item in inventory.GetItemList())
            {
                RectTransform itemsSlotRectTransform_Sell = Instantiate(itemSlotTemplate_Sell, itemSlotContainer_Sell).GetComponent<RectTransform>();
                itemsSlotRectTransform_Sell.gameObject.SetActive(true);

                itemsSlotRectTransform_Sell.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);
                Image image = itemsSlotRectTransform_Sell.Find("Image").GetComponent<Image>();
                image.sprite = item.GetSprite();

                Button button_Sell = itemsSlotRectTransform_Sell.Find("Button").GetComponent<Button>();
                button_Sell.onClick.AddListener(ButtonBuyPressed);

                TextMeshProUGUI uiText = itemsSlotRectTransform_Sell.Find("Text").GetComponent<TextMeshProUGUI>();
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


    private void BuyPageInventory()
    {
        if(itemSlotContainer_Buy != null)
        {
            foreach (Transform child in itemSlotContainer_Buy)
            {
                if (child == itemSlotTemplate_Buy) continue;
                Destroy(child.gameObject);
            }

            int x = 0;
            int y = 0;
            float itemSlotCellSize = 80f;

            foreach (Item item in merchantListedItems)
            {
                RectTransform itemsSlotRectTransform_Buy = Instantiate(itemSlotTemplate_Buy, itemSlotContainer_Buy).GetComponent<RectTransform>();
                itemsSlotRectTransform_Buy.gameObject.SetActive(true);

                itemsSlotRectTransform_Buy.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);
                Image image = itemsSlotRectTransform_Buy.Find("Image").GetComponent<Image>();
                image.sprite = item.GetSprite();

                Button button_Buy = itemsSlotRectTransform_Buy.GetComponent<Button>();
                button_Buy.onClick.AddListener(ButtonSellPressed);

                x++;
                if (x > 3)
                {
                    x = 0;
                    y++;
                }
            }
        }
    }

    // the item to the corresponding button should be added/removed from the inventory list
    private void ButtonBuyPressed()
    {
        inventory.AddItem(new Item { itemType = Item.ItemType.potato, amount = 1 });  



        //player.currentMoney -= 
    }

    private void ButtonSellPressed()
    {
        inventory.RemoveItem(new Item { itemType = Item.ItemType.potato, amount = 1 });

        //player.currentMoney += 
    }
}
