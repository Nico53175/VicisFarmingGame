using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { Start, PlayerAction, EnemyAction, Busy, Won, Lost }

public class BattleSystem : MonoBehaviour
{
    BattleState state;

    public GameObject abilityButtonPrefab;
    public Canvas abilityButtonParent;

    public Creature playerCreature;
    private CreatureAbility[] playerAtbilities;
    private CreatureType playerType;
    private int playerHealth;
    private int playerDamage;
    private string playerName;

    private CreatureAbility[] enemyAtbilities;
    private CreatureType enemyType;
    private int enemyHealth;
    private int enemyDamage;
    private string enemyName;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Creature enemy;
            enemy = collision.gameObject.GetComponent<Creature>();
            Creature player;
            player = playerCreature;

            state = BattleState.Start;
            StartCoroutine(SetupBattle(enemy, player));
        }
    }
    IEnumerator SetupBattle(Creature enemy, Creature player)
    {
        Debug.Log("Battle Starts!");
        enemyName = enemy.name;
        enemyAtbilities = enemy.SOEnemyAttacks;
        enemyType = enemy.SOEnemyType;        
        enemyHealth = enemy.health;

        playerAtbilities = player.SOEnemyAttacks;
        playerType = player.SOEnemyType;        
        playerHealth = player.health;
        playerName = player.name;

        enemyDamage = getAttackerDmgByCheckingVurlnarabilities(enemyType, enemy.damage, playerType);
        playerDamage = getAttackerDmgByCheckingVurlnarabilities(playerType , enemy.damage, playerType);

        PopulateAbilityUI(playerAtbilities);

        yield return new WaitForSeconds(2f);

        state = BattleState.PlayerAction;
        PlayerAction();
    }

    int getAttackerDmgByCheckingVurlnarabilities(CreatureType attacker, int attackerDmg, CreatureType defender)
    {
        int damage = attackerDmg;

        if (attacker.air && defender.water)
        {
            damage += Mathf.RoundToInt(damage * attacker.dmgBoostPercentage);
        }
        else if (attacker.water && defender.fire)
        {
            damage += Mathf.RoundToInt(damage * attacker.dmgBoostPercentage);
        }
        else if (attacker.fire && defender.earth)
        {
            damage += Mathf.RoundToInt(damage * attacker.dmgBoostPercentage);
        }
        else if (attacker.earth && defender.air)
        {
            damage += Mathf.RoundToInt(damage * attacker.dmgBoostPercentage);
        }
        return damage;
    }

    void PopulateAbilityUI(CreatureAbility[] abilities)
    {
        foreach (var ability in abilities)
        {
            GameObject buttonObj = Instantiate(abilityButtonPrefab, abilityButtonParent.transform);
            Button abilityButton = buttonObj.GetComponent<Button>();
            abilityButton.GetComponentInChildren<Text>().text = ability.name;
            abilityButton.GetComponent<Image>().sprite = ability.abilitySprite;
            abilityButton.onClick.AddListener(() => StartCoroutine(PlayerAttack(ability)));
        }
    }
    void DisableCanvas()
    {
        abilityButtonParent.enabled = false;
    }

    void EnableCanvas()
    {
        abilityButtonParent.enabled = true;
    }

    void PlayerAction()
    {
        if (state != BattleState.PlayerAction)
        {
            Debug.Log($"Wrong State: {state}, it should be {BattleState.PlayerAction}");
            return;
        }

        EnableCanvas();
        Debug.Log("Player Action!"); 
    }

    IEnumerator PlayerAttack(CreatureAbility playerAbility)
    {
        state = BattleState.Busy;
        DisableCanvas();
        Debug.Log("Player Attacks!");

        int abilityDamage = 0;

        if (playerAbility != null)
        {
            abilityDamage = getAbilityDmgByCheckingVurlnarabilities(playerAbility, playerDamage, enemyType);
            enemyHealth -= abilityDamage;
        }

        yield return new WaitForSeconds(2f);

        if (enemyHealth <= 0)
        {
            state = BattleState.Won;
            EndBattle();
        }
        else
        {
            state = BattleState.EnemyAction;
            EnemyAction();
        }
    }

    void EnemyAction()
    {
        if (state != BattleState.EnemyAction)
        {
            Debug.Log($"Wrong State: {state}, it should be {BattleState.EnemyAction}");
            return;
        }

        Debug.Log("Enemy Action!");

        StartCoroutine(EnemyAttack());
    }

    IEnumerator EnemyAttack()
    {
        state = BattleState.Busy;

        Debug.Log("Enemy Attacks!");

        int abilityDamage = 0;
        CreatureAbility enemyAbility = ChooseRandomEnemyAttack();

        if(enemyAbility != null)
        {
            abilityDamage = getAbilityDmgByCheckingVurlnarabilities(enemyAbility, enemyDamage, playerType);
            playerHealth -= abilityDamage;
        }

        yield return new WaitForSeconds(2f);

        if (playerHealth <= 0)
        {
            state = BattleState.PlayerAction;
            PlayerAction();
        }
        else
        {
            state = BattleState.Lost;
            EndBattle();
        }
    }

    private CreatureAbility ChooseRandomEnemyAttack()
    {
        int attackRandomizer = Random.Range(0, enemyAtbilities.Length);
        return enemyAtbilities[attackRandomizer];
    }

    int getAbilityDmgByCheckingVurlnarabilities(CreatureAbility attacker, int attackerDmg, CreatureType defender)
    {
        int damage = attackerDmg;

        if (attacker.dmgBoostAir && defender.water)
        {
            damage += Mathf.RoundToInt(damage * attacker.dmgBoostPercentage);
        }
        else if (attacker.dmgBoostWater && defender.fire)
        {
            damage += Mathf.RoundToInt(damage * attacker.dmgBoostPercentage);
        }
        else if (attacker.dmgBoostFire && defender.earth)
        {
            damage += Mathf.RoundToInt(damage * attacker.dmgBoostPercentage);
        }
        else if (attacker.dmgBoostEarth && defender.air)
        {
            damage += Mathf.RoundToInt(damage * attacker.dmgBoostPercentage);
        }
        return damage;
    }

    void EndBattle()
    {
        if (state == BattleState.Won)
        {
            Debug.Log("You won the battle!");
        }
        else if (state == BattleState.Lost)
        {
            Debug.Log("You were defeated.");
        }
    }
}

