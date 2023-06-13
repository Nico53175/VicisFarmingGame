using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Creature", menuName = "Creature", order = 1)]
public class Creature : ScriptableObject
{    
    public string enemyName;
    public string description;
    public CreatureType SOEnemyType;
    public CreatureAbility[] SOEnemyAttacks;
    public int health;
    public int damage;
}


[CreateAssetMenu(fileName = "CreatureType", menuName = "CreatureType", order = 2)]
public class CreatureType : ScriptableObject
{
    public string typeName;
    public bool water;
    public bool fire;
    public bool earth;
    public bool air;
    public float dmgBoostPercentage;
}

[CreateAssetMenu(fileName = "CreatureAttack", menuName = "CreatureAttack", order = 3)]
public class CreatureAbility : ScriptableObject
{
    public string attackName;
    public Sprite abilitySprite;
    public int spellCost;
    public bool dmgBoostWater;
    public bool dmgBoostFire;
    public bool dmgBoostEarth;
    public bool dmgBoostAir;
    public float dmgBoostPercentage;
    //Animation...
}