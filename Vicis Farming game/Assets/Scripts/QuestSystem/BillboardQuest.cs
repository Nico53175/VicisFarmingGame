using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardQuest : MonoBehaviour
{
    [SerializeField]
    private List<Quest> quests;
    private QuestGenerator questGenerator;
    public Creature[] enemiesInQuestArea;
    public AnimationCurve difficultyCurve;
    // Start is called before the first frame update
    void Start()
    {
        questGenerator = new QuestGenerator(10, 20, enemiesInQuestArea, difficultyCurve);
        quests.Add(CreateQuest());    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Quest CreateQuest()
    {
        return questGenerator.GenerateQuest();
    }
}
