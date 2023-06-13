using UnityEngine;

public class QuestGenerator
{
    private int minEnemies;
    private int maxEnemies;
    private float rareEnemiePercantage; //Determines how many of the maxEnemies are rare ones
    private Creature[] enemies;
    private AnimationCurve difficultyCurve;

    public QuestGenerator(int minEnemies, int maxEnemies, Creature[] enemies, AnimationCurve difficultyCurve)
    {
        this.minEnemies = minEnemies;
        this.maxEnemies = maxEnemies;
        this.enemies = enemies;
        this.difficultyCurve = difficultyCurve;
    }
    private float DifficultyScale()
    {
        // The curve is evaluated at the player's level.
        // You need to ensure that the curve is properly defined.
        return difficultyCurve.Evaluate(PlayerStats.playerLevel);
    }

    private int CalculateEnemyCount()
    {
        int commonEnemyCount = Random.Range(minEnemies, maxEnemies);
        Debug.Log(commonEnemyCount);
        return commonEnemyCount;
    }

    private int CalculateRareEnemyCount()
    {
        float rareEnemiesPercentage = DifficultyScale();
        int maxRareEnemies = (int)Mathf.Floor(maxEnemies * rareEnemiesPercentage);
        int rareEnemyCount = Random.Range(0, maxRareEnemies);

        Debug.Log($"Rare Enemy Count: {rareEnemyCount}, Percentage: " + rareEnemiePercantage + $" max rare Enemies: {maxRareEnemies}");
        return rareEnemyCount;
    }   


    public Quest GenerateQuest()
    {
        return new KillQuest
        {
            name = "Kill Enemies",
            description = $"In the forest are invaders. It seems like some {enemies[0].enemyName} found their way here. Kill them to protect the city",
            enemies = this.enemies,
            killCounts = new int[] { CalculateEnemyCount(), CalculateRareEnemyCount() },
            rewards = new QuestReward[]
            {
                new QuestReward { rewardName = $"Reward (Level {PlayerStats.playerLevel})", rewardCount = 1 }
            }
        };
    }
}
