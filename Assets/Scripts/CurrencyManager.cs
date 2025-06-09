using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    public TextMeshProUGUI currencyText;
    int _coins;
    [SerializeField]
    int coins
    {
        get { return _coins; }
        set
        {
            _coins = value;
            // Save the coins to PlayerPrefs
            PlayerPrefs.SetInt("Coins", _coins);
            currencyText.text = _coins.ToString();
        }
    }

    void Awake()
    {
        instance = this;
        // Determine if the player has a saved coins data
        coins = PlayerPrefs.GetInt("Coins", 0);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
    }

    public bool CanBuy(int amount)
    {
        return coins >= amount;
    }

    public bool SpendCoins(int amount)
    {
        if (CanBuy(amount))
        {
            coins -= amount;
            return true;
        }

        Debug.Log("Not enough coins!");

        return false;
    }
}
