using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // Inventory Plant SetUp
    public InventoryItemController inventoryItemPrefab;
    public GridLayoutGroup gridLayoutGroup;
    public PlantData[] availablePlants;

    // Safe ItemCounts
    public Dictionary<PlantData, int[]> itemCount;
    public const int seedCountIndex = 0;
    public const int plantCountIndex = 1;

    // New Patch
    public PatchManager patchManager;
    public Button patchButton;

    // Planting
    private PlantData selectedPlant;

    // InventoryUI Reference
    public GameObject inventoryUI;

    // Currency Reference
    public CurrencyManager currencyManager;


    private void Start()
    {
        itemCount = new Dictionary<PlantData, int[]>();
        PopulateInventory();
        patchButton.onClick.AddListener(patchManager.ActivatePatchMode);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    private void PopulateInventory()
    {
        if (availablePlants != null)
        {
            foreach (PlantData plant in availablePlants)
            {
                itemCount[plant] = new int[2] { plant.startCount, 0 }; // Set Amount

                InventoryItemController seedItem = Instantiate(inventoryItemPrefab, gridLayoutGroup.transform); // Create Item
                seedItem.SetSeed(plant, itemCount[plant][seedCountIndex]);
                seedItem.isPlant = false;
                seedItem.GetComponent<Button>().onClick.AddListener(() => SelectSeedToPlant(plant));

                InventoryItemController plantItem = Instantiate(inventoryItemPrefab, gridLayoutGroup.transform);
                plantItem.SetPlant(plant, itemCount[plant][plantCountIndex]);
                plantItem.isPlant = true;

                if (itemCount[plant][seedCountIndex] < 1) // If Amout < 1 -> Deactivate Item in Inventory
                {
                    seedItem.gameObject.SetActive(false);
                }

                if (itemCount[plant][plantCountIndex] < 1)
                {
                    plantItem.gameObject.SetActive(false);
                }
            }
        }
    }

    private void SelectSeedToPlant(PlantData plant)
    {
        if (itemCount[plant][seedCountIndex] > 0)
        {
            selectedPlant = plant;
            Debug.Log("Seed of " + plant.plantName + " selected.");
        }
        else
        {
            selectedPlant = null;
            Debug.Log("No seeds available for " + plant.plantName + ".");
        }
    }

    public PlantData GetSelectedPlant()
    {
        return selectedPlant;
    }

    public void RemoveItem(PlantData plant, bool isPlant)
    {
        int index;
        if (isPlant)
        {
            index = 1;
        }
        else
        {
            index = 0;
        }

        itemCount[plant][index] -= 1;
        InventoryItemController item = GetItem(plant, isPlant);
        item.UpdatePlantCountDisplay(itemCount[plant][index]);
    }

    public void AddItem(PlantData plant, bool isPlant)
    {
        int index;
        if (isPlant)
        {
            index = 1;
        }
        else
        {
            index = 0;
        }

        itemCount[plant][index] += 1;
        InventoryItemController item = GetItem(plant, isPlant);
        item.UpdatePlantCountDisplay(itemCount[plant][index]);
    }

    public InventoryItemController GetItem(PlantData plant, bool isPlant)
    {
        InventoryItemController[] items = gridLayoutGroup.GetComponentsInChildren<InventoryItemController>(true);

        if (items.Length == 0)
        {
            Debug.LogError("No InventoryItemController components found.");
            return null;
        }

        InventoryItemController item = Array.Find(items, i => i.plantData == plant && i.isPlant == isPlant);

        if (item == null)
        {
            Debug.LogError("Could not find the InventoryItemController for the given plant: " + plant.plantName);
        }

        return item;
    }


}