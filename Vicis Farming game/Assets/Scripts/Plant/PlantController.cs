using UnityEngine;

public class PlantController : MonoBehaviour
{
    public PlantData plantData;
    private SpriteRenderer spriteRenderer;
    private float currentGrowthTime;
    private int currentGrowthStage;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateGrowth(Time.deltaTime);
    }

    public void InitializePlant(PlantData data)
    {
        plantData = data;
        spriteRenderer.sprite = plantData.growthSprites[0];
        currentGrowthTime = 0;
        currentGrowthStage = 0;
    }

    public void UpdateGrowth(float deltaTime)
    {
        currentGrowthTime += deltaTime;



        float timePerStage = plantData.growthTime / plantData.growthSprites.Length;
        int targetGrowthStage = Mathf.FloorToInt(currentGrowthTime / timePerStage);



        if (targetGrowthStage != currentGrowthStage && targetGrowthStage < plantData.growthSprites.Length)
        {
            currentGrowthStage = targetGrowthStage;
            spriteRenderer.sprite = plantData.growthSprites[currentGrowthStage];
        }
    }

    public int GetCurrentGrowStage()
    {
        return currentGrowthStage;
    }
}