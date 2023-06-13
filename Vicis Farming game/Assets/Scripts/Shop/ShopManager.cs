using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public InventoryManager inventory;
    public CurrencyManager currencyManager;
    public GameObject shopItemPrefab;
    public Transform shopContent;
    public PlantData[] availablePlants;
    public TMP_Text currencyText;

    void Start()
    {
        PopulateShop();
    }

    public void BuyItem(PlantData plantData, bool isPlant)
    {
        int playerCurrency = currencyManager.GetCurrentCurrency();

        if (playerCurrency >= plantData.price)
        {
            inventory.AddItem(plantData, isPlant);
            currencyManager.SpendCurrency(plantData.price);
            UpdateCurrencyText();
        }
        else
        {
            Debug.Log("Not enough currency to buy this item.");
        }
    }

    public void SellItem(PlantData plantData, bool isPlant)
    {
        inventory.RemoveItem(plantData, isPlant);
        currencyManager.EarnCurrency(plantData.price);
        UpdateCurrencyText();
    }

    public void PopulateShop()
    {
        foreach (PlantData plantData in availablePlants)
        {
            // Instantiate the prefab
            GameObject shopItem = Instantiate(shopItemPrefab, shopContent);
            ShopItemTemplate shopItemData = shopItem.GetComponent<ShopItemTemplate>();

            // Set the image            
            shopItemData.previewItem.sprite = plantData.plantSprite;

            // Set the title text
            shopItemData.titleTxt.text = plantData.plantName;

            // Set the price text
            shopItemData.baseCostTxt.text = $"{plantData.price} Coins";

            // Set the buy button listener
            shopItemData.buyButton.onClick.AddListener(() => BuyItem(plantData, false));
        }
        UpdateCurrencyText();
    }
    public void UpdateCurrencyText()
    {
        int currentCurrency = currencyManager.GetCurrentCurrency();
        currencyText.text = $"{currentCurrency} Coins";
    }

}