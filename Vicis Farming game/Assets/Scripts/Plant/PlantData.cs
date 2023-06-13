using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public Sprite plantSprite;
    public Sprite seedSprite;
    public int price;

    // Growth
    public Sprite[] growthSprites;
    public float growthTime;
    public int harvestYield;

    // Start Amount of seeds when starting the game
    [Range(0, 999)]
    public int startCount;
}