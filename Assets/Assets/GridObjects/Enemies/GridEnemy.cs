using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEnemy : GriddableObject
{

    public GameObject TexturePlane;
    public EnemyBase enemy_;
    public Transform RollsUI;
    public GameObject RollUIPrefab;
    public int EnemyId_ = -1;
    void Awake()
    {
        enemy_ = GetEnemyByID(EnemyId_);
        player_ = false;
    }

    void Update()
    {
        LookAtCamera();
        MoveToDestination();
    }



    private void OnEnable()
    {
        GType_ = GriddableObjectType.Enemy;
    }

    void LookAtCamera()
    {
        TexturePlane.transform.LookAt(field_.Camera.Camera);
        TexturePlane.transform.localEulerAngles = new Vector3(0,
                                                              TexturePlane.transform.localEulerAngles.y,
                                                              TexturePlane.transform.localEulerAngles.z);
    }

    void MoveToDestination()
    {
        transform.position = Vector3.Lerp(transform.position, DestinationPosition, 0.1f);
    }

    public override void ReplaceObject(int id)
    {
        enemy_ = GetEnemyByID(id);
    }
    EnemyBase GetEnemyByID(int id)
    {
        EnemyBase ret_character;

        ret_character = DataDicts.EnemySet[id].Clone();
        if (ret_character == null) ret_character = new TestEnemy();

        ret_character.Init();
        return ret_character;
    }

    public bool CanMove()
    {
        if (enemy_.cur_moves > 0) return true;
        return false;
    }

    [ContextMenu("CreateSkills")]
    public void CreateSkills()
    {
        enemy_.CreateSkills(0);
    }

    [ContextMenu("UseFirstInQueueSkill")]
    public void UseNextSkill()
    {
        enemy_.CreateNextRolls();
        UpdateRollsUI();
    }

    public void UpdateRollsUI(float start_alpha = 1f)
    {
        StaticFuncs.DestroyChildren(RollsUI);
        List<GameObject> rolls_list = new List<GameObject>();
        foreach (Roll roll in enemy_.CurrentRolls)
        {
            GameObject menu_roll = Instantiate(RollUIPrefab, RollsUI);
            menu_roll.GetComponent<RollScript>().Fade(0f, 0f, 0f);
            menu_roll.GetComponent<RollScript>().UpdateRollStats(roll);
            menu_roll.transform.localScale = (Vector3.one) / 250;
            menu_roll.transform.Rotate(Vector3.up, 180);
            rolls_list.Add(menu_roll);
        }
        if (rolls_list.Count > 6)
        {
            for (int i = 0; i < rolls_list.Count; i++)
            {
                rolls_list[i].transform.localPosition = new Vector3(0.225f * ((float)-Math.Pow(-1f, i+1)), 0.2f + 0.45f * (i / 2), 0);
            }
        }
        else
        {
            for (int i = 0; i < rolls_list.Count; i++)
            {
                rolls_list[i].transform.localPosition = new Vector3(0, 0.2f + 0.45f * i, 0);
            }
        }
    }

    public EnemyBase GetEnemyBase() { return enemy_; }
}
