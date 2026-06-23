using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BattleMain : MonoBehaviour
{
    public bool IsInBattle = true;
    public bool PlayerCanAttack = false;
    public int turn = 0;
    public bool lock_cycle = false;
    public bool make_next_cycle_on_unlock = false;
    public int ext_data_counter = 0;

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
        CheckCycleLock();
    }

    public void StartBattle()
    {
        current_cycle = cycles[0];
        NextCycle(0);
    }

    public void NextCycle(int num = 1, bool activate = true)
    {
        var next_cycle = cycles[(cycles.IndexOf(current_cycle) + num) % cycles.Count];
        Debug.Log("NextCycle: " + next_cycle);

        current_cycle = next_cycle;
        if (activate)
        {
            if (current_cycle == BattleCycle.EnemyRollsCreate)
            {
                CharacterTabSwitch(true, false);
                EnemyCreateRolls();
                NextCycle();
            }
            else if (current_cycle == BattleCycle.PlayerTurn)
            {
                
                CharacterTabSwitch(false, false, true);
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
            enemy.UpdateRollsUI(0f);
            for (int i = 0; i < enemy.enemy_.CurrentRolls.Count; i++)
            {
                enemy.RollsUI.GetChild(i).GetComponent<RollScript>().Fade(1f, 0.75f, i * 0.5f, true);
            } 
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

    void CheckCycleLock()
    {
        if (!lock_cycle && make_next_cycle_on_unlock)
        {
            make_next_cycle_on_unlock = false;
            NextCycle();
        } 
    }

    public int GetEnemyRollsCount()
    {
        int counter = 0;
        foreach (var enemy in current_field.GridEnemies)
        {
            counter += enemy.RollsUI.childCount;
        }
        return counter;
    }

    public void MakeFight(GridCharacter character, GriddableObject enemy)
    {
        Debug.Log("ATTACK!");
    }
}


enum BattleCycle
{
    EnemyRollsCreate,
    PlayerTurn,
    EnemyTurn1,
    EnemyTurn2
};
