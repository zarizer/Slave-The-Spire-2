using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GridCharacter : GriddableObject
{


    public GameObject TexturePlane;
    public UnityEngine.UI.Image hp_circle;
    public CharacterBase character_;
    public int CharacterId_ = 0;
    
    public List<Roll> CurrentSkillRolls = new List<Roll>();
    public List<Roll> DefenceRolls = new List<Roll>();

    void Start()
    {
        //character_ = GetCharacterByID(CharacterId_);
        player_ = true;
    }

    void Update()
    {
        LookAtCamera();
        MoveToDestination();
        hp_circle.fillAmount = ((float)character_.cur_hp) / character_.hp;
    }



    private void OnEnable()
    {
        GType_ = GriddableObjectType.Character;
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
        character_ = GetCharacterByID(id);
    }
    public static CharacterBase GetCharacterByID(int id)
    {
        Type type = DataDicts.CharacterTypes[id];
        CharacterBase result = (CharacterBase)Activator.CreateInstance(type);
        result.Init();
        result.InitCurStats();
        result.InitSkills();
        return result;
    }

    public bool CanMove()
    {
        if (character_.cur_moves>0) return true;
        return false;
    }

    public void MakeCurrentRolls(int skill_num)
    {
        PlayerSkill cur_skill = null;
        if (skill_num == 1) cur_skill = character_.Skill1;
        if (skill_num == 2) cur_skill = character_.Skill2;
        if (skill_num == 3) cur_skill = character_.Skill3;
        if (skill_num == 4) cur_skill = character_.Skill4;

        if (CheckSkillResourses(cur_skill))
        {
            CurrentSkillRolls = cur_skill.GetRolls();
        }
    }

    bool CheckSkillResourses(PlayerSkill skill)
    {
        if (skill.CheckSkillResourses() && skill.CheckSkillRestrictions()) return true;
        return false;
    }

    public override Roll GetFirstRoll() 
    { 
        if (DefenceRolls.Count == 0) return null;
        return DefenceRolls[0];
    }

    public override void RemoveFirstRoll(float offset = 0f) { DefenceRolls.RemoveAt(0); }

    public override void GetDamage(Damage damage)
    {
        character_.GetDamage(damage);
    }

    public override int GetLevel()
    {
        return character_.level;
    }

    public override CharacterBase GetCharacter() { return character_; }
}






