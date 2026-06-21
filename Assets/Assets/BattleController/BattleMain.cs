using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BattleMain : MonoBehaviour
{
    public bool IsInBattle = true;
    public bool PlayerCanAttack = false;
    public int turn = 0;

    List<BattleCycle> cycles = new List<BattleCycle>
    {
        BattleCycle.EnemyRollsCreate,
        BattleCycle.PlayerTurn,
        BattleCycle.EnemyTurn1,
        BattleCycle.EnemyTurn2
    };

    private BattleCycle current_cycle;

    public GridField current_field;

    void Start()
    {

        current_field.StartField();
        StartBattle();
    }


    void Update()
    {

    }

    public void StartBattle()
    {
        current_cycle = cycles[0];
        NextCycle(0);
    }

    public void NextCycle(int num = 1, bool activate = true)
    {
        var next_cycle = cycles[(cycles.IndexOf(current_cycle) + num) % cycles.Count];

        current_cycle = next_cycle;
        if (activate)
        {
            if (current_cycle == BattleCycle.EnemyRollsCreate)
            {
                CharacterTabSwitch(true, false);
                EnemyCreateRolls();
            }
            else if (current_cycle == BattleCycle.PlayerTurn)
            {
                CharacterTabSwitch(false, false, false);
                PlayerCanAttack = true;
            }
            else if (current_cycle == BattleCycle.EnemyTurn1)
            {
                CharacterTabSwitch(true, false);

                PlayerCanAttack = false;
            }
            else if (current_cycle == BattleCycle.EnemyTurn2)
            {

            }
        }
    }

    void EnemyCreateRolls()
    {
        foreach (var enemy in current_field.GridEnemies)
        {
            enemy.enemy_.CreateSkills(turn);
            while (enemy.enemy_.CreateNextRolls()) { }
            enemy.UpdateRollsUI();
        }
    }

    [ContextMenu("NextCycle")]
    public void DebugNextCycle()
    {
        NextCycle();
    }

    void CharacterTabSwitch(bool locked, bool enable, bool need_lock = true, bool need_enable = true)
    {
        if (need_enable) { ResoursesDict.ObjectSet["CharacterTab"].GetComponent<CharacterTabController>().RequestedUpdate(enable); }
        if (need_lock) { ResoursesDict.ObjectSet["CharacterTab"].GetComponent<CharacterTabController>().IsLocked = locked; }
    }
}


enum BattleCycle
{
    EnemyRollsCreate,
    PlayerTurn,
    EnemyTurn1,
    EnemyTurn2
};
