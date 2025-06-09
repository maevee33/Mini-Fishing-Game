using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Create class to hold item information for upgrades
[System.Serializable] //Mark this class as serializable so it can be viewed in the inspector
public class UpgradeData
{
    public Sprite icon;
    public int price;
}

public class InventoryItem : MonoBehaviour
{
    public string itemKey;
    public TextMeshProUGUI itemLevelText;
    public Image itemIcon;
    public string itemDescription;
    public int level = 1;
    public List<UpgradeData> upgradeData = new List<UpgradeData>();

    public UpgradeData nextUpgradeData
    {
        get
        {
            if (level < upgradeData.Count)
            {
                return upgradeData[level];
            }
            else
            {
                return null;
            }
        }
    }

    public UpgradeData currentItemData
    {
        get
        {
            if (level - 1 < upgradeData.Count)
                return upgradeData[level - 1];

            return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadItemLevel();
        UpdateItemDisplay();
    }

    public void LoadItemLevel()
    {
        level = PlayerPrefs.GetInt(itemKey + "_level", 1);

    }

    public void UpdateItemDisplay()
    {
        itemLevelText.text = "Level " + level;
        itemIcon.sprite = currentItemData.icon;
    }

    // This will be called by the shop manager later
    public void Upgrade()
    {
        level++;
        PlayerPrefs.SetInt(itemKey + "_level", level);
        UpdateItemDisplay();
    }
}