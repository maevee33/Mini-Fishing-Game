using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public List<InventoryItem> stallItems = new List<InventoryItem>();
    public Transform stallRoot;
    public StallItem stallItemPrefab;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeStall();
    }

    public void InitializeStall() {
        // clear existing items
        foreach (Transform child in stallRoot) {
            Destroy(child.gameObject);
        }

        // create new items
        foreach (InventoryItem item in stallItems) {
            StallItem newItem = Instantiate(stallItemPrefab, stallRoot);
            newItem.itemData = item;
            newItem.UpdateItemDisplay();
        }
    }
 
    public InventoryItem GetItem(string itemKey) {
        foreach (InventoryItem item in stallItems) {
            if (item.itemKey == itemKey) {
                return item;
            }
        }

        return null;
    }

    public void PurchaseUpgrade(InventoryItem item) {
        int cost = item.nextUpgradeData.price;

        if(cost == -1) {
            Debug.Log("Item is maxed out, cannot purchase upgrade.");
            return;
        }

        if(CurrencyManager.instance.CanBuy(cost)) {
            CurrencyManager.instance.SpendCoins(cost);
            InventoryManager.instance.UpgradeItem(item.itemKey);
        }
        else {
            Debug.Log("Not enough coins to purchase upgrade.");
            return;
        }
    }
}