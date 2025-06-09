using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Dictionary<string, InventoryItem> inventoryItems = new Dictionary<string, InventoryItem>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }


        // Find all InventoryItem components in the scene
        List<InventoryItem> items = new List<InventoryItem>(FindObjectsOfType<InventoryItem>());

        // Load all items into the inventoryItems dictionary
        foreach (InventoryItem item in items)
        {
            Debug.Log("Adding item with key " + item.itemKey + " to inventory");
            inventoryItems.Add(item.itemKey, item);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshInventory();
    }

    // Go through all items in the inventory and update their display
    public void RefreshInventory()
    {
        foreach (InventoryItem item in inventoryItems.Values)
        {
            item.UpdateItemDisplay();
        }
    }

    // This will be used by the game to check the levels of the specific items
    public int GetLevel(string itemKey)
    {
        // Check if the item exists in the inventory
        if (!inventoryItems.ContainsKey(itemKey))
        {
            Debug.LogError("Item with key " + itemKey + " not found in inventory");
            return 0;
        }

        return inventoryItems[itemKey]?.level ?? 0;
    }


    // This is the function that will actually be called by the shop, then we call the corresponding function in the InventoryItem
    public void UpgradeItem(string itemKey)
    {
        if (!inventoryItems.ContainsKey(itemKey))
        {
            Debug.LogError("Item with key " + itemKey + " not found in inventory");
            return;
        }

        inventoryItems[itemKey].Upgrade();
    }
}