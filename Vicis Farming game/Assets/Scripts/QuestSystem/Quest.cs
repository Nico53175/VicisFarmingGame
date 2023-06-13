[System.Serializable]
public class Quest
{
    public string name;
    public string description;
    public QuestReward[] rewards;
}

[System.Serializable]
public class KillQuest : Quest
{
    public Creature[] enemies;
    public int[] killCounts;
}

[System.Serializable]
public class QuestReward
{
    public string rewardName;
    public int rewardCount;
}
