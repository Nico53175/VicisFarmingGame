using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GridManager;

public class PlayerFarming : MonoBehaviour
{
    // Koordinates
    private int gridX;
    private int gridY;

    // References  
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private Camera mainCamera;
    private GridManager gridManager;
    private PlantData selectedPlant;
    private GameObject[,] grid;
    private bool isGridInRange;

    // Distance Check
    private float distance;
    [SerializeField] private int plantRange;

    private void Update()
    {
        if (isGridInRange)
        {
            GetSelectedPatchCell();
        }
    }

    private void GetSelectedPatchCell()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // First, raycast only on the main collider layer
            int mainColliderLayerMask = 1 << LayerMask.NameToLayer("Patch");
            RaycastHit2D hitMainCollider = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, mainColliderLayerMask);

            if (hitMainCollider.collider != null)
            {
                Debug.Log("Raycast hit: " + hitMainCollider.collider.name);
                HandleHit(hitMainCollider, mousePosition);
                return;
            }
            
            Debug.Log("Raycast did not hit the grid collider.");
        }
    }

    private void HandleHit(RaycastHit2D hit, Vector2 mousePosition)
    {
        gridManager = hit.collider.GetComponent<GridManager>();
        gridX = Mathf.RoundToInt(mousePosition.x - gridManager.transform.position.x);
        gridY = Mathf.RoundToInt(mousePosition.y - gridManager.transform.position.y);

        if (IsPlayerInRange())
        {
            PerformAction();
        }
        else
        {
            Debug.Log($"Cannot plant at position ({gridX}, {gridY}) because the player is out of range. Distance: {distance}");
        }
    }

    private bool IsPlayerInRange()
    {
        distance = Vector2.Distance(transform.position, new Vector2(gridX + gridManager.transform.position.x, gridY + gridManager.transform.position.y));

        if (distance <= plantRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PerformAction()
    {
        if (IsPositionOccupied())
        {
            if (IsReadyToHarvest())
            {
                Harvest();
            }
        }
        else
        {
            if (IsPlantingAvailable())
            {
                Plant();
            }
        }
    }

    private bool IsPlantingAvailable()
    {
        int seedCountIndex = InventoryManager.seedCountIndex;
        selectedPlant = inventory.GetSelectedPlant();

        if (selectedPlant != null
            && inventory.itemCount[selectedPlant][seedCountIndex] > 0
                && gridX >= 0 && gridX < gridManager.width && gridY >= 0 && gridY < gridManager.height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsPositionOccupied()
    {
        grid = gridManager.grid;
        if (grid[gridX, gridY] == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool IsReadyToHarvest()
    {
        PlantController plantController = grid[gridX, gridY].GetComponent<PlantController>();

        if (plantController.GetCurrentGrowStage() == plantController.plantData.growthSprites.Length - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Harvest()
    {
        PlantData harvestedPlantData = GetPlantDataFromPlant(grid[gridX, gridY]);
        Dictionary<PlantData, int[]> itemCount = inventory.itemCount;
        int plantCountIndex = InventoryManager.plantCountIndex;
        itemCount[harvestedPlantData][plantCountIndex] += harvestedPlantData.harvestYield;
        Destroy(grid[gridX, gridY]);
        grid[gridX, gridY] = null;

        InventoryItemController item = inventory.GetItem(harvestedPlantData, true);
        item.UpdatePlantCountDisplay(itemCount[harvestedPlantData][plantCountIndex]);
    }

    private void Plant()
    {
        Dictionary<PlantData, int[]> itemCount = inventory.itemCount;
        int seedCountIndex = InventoryManager.seedCountIndex;

        gridManager.Plant(selectedPlant, gridX, gridY);
        itemCount[selectedPlant][seedCountIndex]--;

        InventoryItemController item = inventory.GetItem(selectedPlant, false);
        item.UpdatePlantCountDisplay(itemCount[selectedPlant][seedCountIndex]);
    }

    private PlantData GetPlantDataFromPlant(GameObject plant)
    {
        PlantController plantController = plant.GetComponent<PlantController>();
        return plantController.plantData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Outer Patch Collider")
        {
            Debug.Log("Player entered Grid!");
            gridManager = collision.GetComponentInParent<GridManager>();
            isGridInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Outer Patch Collider")
        {
            Debug.Log("Player left Grid!");
            gridManager = null;
            isGridInRange = false;
        }
    }
}