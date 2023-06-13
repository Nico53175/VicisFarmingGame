using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    public PlantData plantData;
    public Image plantImage;
    public TextMeshProUGUI plantCountText;
    public bool isPlant;

    public void SetSeed(PlantData data, int count)
    {
        plantData = data;
        plantImage.sprite = plantData.seedSprite;
        UpdatePlantCountDisplay(count);
    }

    public void SetPlant(PlantData data, int count)
    {
        plantData = data;
        plantImage.sprite = plantData.plantSprite;
        UpdatePlantCountDisplay(count);
    }

    public void UpdatePlantCountDisplay(int count)
    {
        plantCountText.text = count.ToString();

        if (count > 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}