using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GridEnemy : GriddableObject
{

    public GameObject TexturePlane;
    public RawImage Texture;
    public UnityEngine.UI.Image hp_circle;
    public EnemyBase enemy_;
    public Transform EffectObject;
    public GameObject EffectPrefab;
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
        hp_circle.fillAmount = ((float)enemy_.cur_hp) / enemy_.hp;
    }



    private void OnEnable()
    {
        GType_ = GriddableObjectType.Enemy;
    }

    void LookAtCamera()
    {
        TexturePlane.transform.LookAt(field_.cameraController.Camera);
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
        Type type = DataDicts.EnemyTypes[id];
        EnemyBase result = (EnemyBase)Activator.CreateInstance(type);
        result.Init();
        return result;
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

    public List<GameObject> UpdateRollsUI(float start_alpha = 1f)
    {
        StaticFuncs.DestroyChildren(RollsUI);
        List<GameObject> rolls_list = new List<GameObject>();
        foreach (Roll roll in enemy_.CurrentRolls)
        {
            GameObject menu_roll = Instantiate(RollUIPrefab, RollsUI);
            menu_roll.GetComponent<CanvasGroup>().alpha = 0f;
            menu_roll.GetComponent<RollScript>().UpdateRollStats(roll);
            menu_roll.transform.localScale = (Vector3.one) / 250;
            menu_roll.transform.Rotate(Vector3.up, 180);
            rolls_list.Add(menu_roll);
            Debug.Log(roll.minRoll + " " + roll.maxRoll);
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
        return rolls_list;
    }

    public override Roll GetFirstRoll()
    {
        if (enemy_.CurrentRolls.Count == 0) { return null; }
        return enemy_.CurrentRolls[0];
    }

    public override void RemoveFirstRoll(float offset = 0f) {   
        enemy_.CurrentRolls.RemoveAt(0);
    }

    public EnemyBase GetEnemyBase() { return enemy_; }

    public override void GetDamage(Damage damage) 
    {
        enemy_.GetDamage(damage);
    }

    public void RemoveRoll(Roll roll) 
    { 
        enemy_.CurrentRolls.Remove(roll);
    }

    public Roll GetFirstAtkRoll()
    {
        foreach (var roll in enemy_.CurrentRolls) 
        {
            if (roll.rollType == RollType.Atk) { return roll; }
        }
        return null;
    }

    public int GetAtkRollCount()
    {
        int count = 0;
        foreach (var roll in enemy_.CurrentRolls)
        {
            if (roll.rollType == RollType.Atk) { count++; }
        }
        return count;
    }

    public override int GetLevel()
    {
        return enemy_.level;
    }

    public override CharacterBase GetCharacter() { return enemy_; }

    public void UpdateEffectIcons()
    {
        StaticFuncs.DestroyChildren(EffectObject);
        for (int i = 0; i < enemy_.effects.Count; i++)
        {
            var effect = Instantiate(EffectPrefab, EffectObject).GetComponent<EffectObject>();
            effect.effect = enemy_.effects[i];
            effect.transform.localPosition = new Vector3(0, 0.4f + 0.3f * i, 0);
            effect.UpdateData();

        }
    }
}
