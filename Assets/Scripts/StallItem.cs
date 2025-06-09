using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class StallItem : MonoBehaviour
{
    public InventoryItem itemData;
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemLevelText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemPriceText;
    public Button buyButton;

    // Start is called before the first frame update
    void Start()
    {
        buyButton.onClick.AddListener(BuyItem);
    }
    
    void OnDestroy()
    {
        buyButton.onClick.RemoveListener(BuyItem);
    }

    public void UpdateItemDisplay()
    {
        if (itemData == null)
        {
            return;
        }

        int level = InventoryManager.instance.GetLevel(itemData.itemKey);
        itemLevelText.text = (level + 1).ToString();
        UpgradeData upgradeData = itemData.nextUpgradeData;

        if (upgradeData == null)
        {
            buyButton.interactable = false;
            itemPriceText.text = "Maxed Out";
            itemIcon.sprite = itemData.itemIcon.sprite;
            itemNameText.text = itemData.name;
            itemLevelText.text = level.ToString();
            itemDescriptionText.text = itemData.itemDescription;
            return;
        }

        int cost = upgradeData.price;
        buyButton.interactable = CurrencyManager.instance.CanBuy(cost);
        itemPriceText.text = "Buy for " + cost.ToString() + "G";
        itemIcon.sprite = upgradeData.icon;
        itemNameText.text = itemData.name;
        itemDescriptionText.text = itemData.itemDescription;
    }

    public void BuyItem() {
        ShopManager.instance.PurchaseUpgrade(itemData);
        UpdateItemDisplay();
    }
}