using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class GridCharacter : GriddableObject
{


    public GameObject TexturePlane;
    public RawImage Texture;
    public UnityEngine.UI.Image hp_circle;
    public CharacterBase character_;
    public int CharacterId_ = 0;
    public Transform EffectObject;
    public GameObject EffectPrefab;
   

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
        hp_circle.fillAmount = Mathf.Lerp(hp_circle.fillAmount,((float)character_.cur_hp) / character_.start_hp, 0.05f);
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
    public static CharacterBase GetCharacterByID(int id, CharacterBase parent = null)
    {
        Type type = DataDicts.CharacterTypes[id];
        CharacterBase result = (CharacterBase)Activator.CreateInstance(type);
        
        result.Init();
        if (parent != null) { result.is_custom_secondary_stats = true; parent.WriteSecondaryData(result); }
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
            CreateCurrentRollsUI(CurrentSkillRolls);
        }
    }

    public List<Roll> GetCurrentRolls(int skill_num)
    {
        List<Roll> res = new List<Roll>();
        PlayerSkill cur_skill = null;
        if (skill_num == 1) cur_skill = character_.Skill1;
        if (skill_num == 2) cur_skill = character_.Skill2;
        if (skill_num == 3) cur_skill = character_.Skill3;
        if (skill_num == 4) cur_skill = character_.Skill4;

        if (CheckSkillResourses(cur_skill))
        {
            res.AddRange((new PlayerSkill(cur_skill).rolls));
        }
        return res;
    }



    public void CreateCurrentRollsUI(List<Roll> rolls)
    {
        character_.CurrentRolls.AddRange(rolls);
        UpdateRollsUI(1f);
    }

    bool CheckSkillResourses(PlayerSkill skill)
    {
        if (skill.CheckSkillResourses() && skill.CheckSkillRestrictions()) return true;
        return false;
    }

    public override Roll GetFirstRoll(bool targetable = true) 
    { 
        if (DefenceRolls.Count == 0) return null;
        return DefenceRolls[0];
    }

    public override void RemoveFirstRoll(float offset = 0f, bool targetable = true) { DefenceRolls.RemoveAt(0); }

    public override void GetDamage(Damage damage)
    {
        character_.GetDamage(damage);
        if (damage.damage > 0)
        {
            BattleMain.GainSwaga(8);
        }
    }

    public override int GetLevel()
    {
        return character_.level;
    }

    public override CharacterBase GetCharacter() { return character_; }

    [ContextMenu("test_effect")]
    public void TestEffect()
    {
        ApplyBattleEffect(DataDicts.EffectTypes[0], 10, 10, character_, character_);
    }
    public static void ApplyBattleEffect(Type effect_type, int power, int duration, CharacterBase target, CharacterBase source, bool is_turn_start = false)
    {
        BattleEffect effect = BattleEffect.GetEffectInstance(effect_type);
        effect.power = power;
        effect.duration = duration;
        effect.character = target;
        effect.source = source;
        effect.Init();
        bool flag = false;
        for (int i = 0; i < target.effects.Count; i++)
        {
            if (target.effects[i].name == effect.name)
            {
                target.effects[i].duration += duration;
                target.effects[i].power += power;
                flag = true;
            }
        }
        if (!flag)
        {
            if (duration <= 0) effect.duration = 1;
            if (is_turn_start) effect.duration++; //ÍÅÎÁÕÎÄÈÌÎ ÈÍÀ×Å ÝÔÔÅÊÒ ÌÃÍÎÂÅÍÍÎ ÓÁÅÐ¨ÒÑß Â ÍÀ×ÀËÅ ÕÎÄÀ 
            target.effects.Add(effect);
        }
        if (target.object_ == null) return;
        TryUpdateEffectIcons(target.object_.GetComponent<GriddableObject>());
        effect.OnApply(ResoursesDict.GetClass<CameraController>().field_, effect, power, duration, flag);
    }

    public static void TryUpdateEffectIcons(GriddableObject obj)
    {
        if (obj.GType_ == GriddableObjectType.Character)
        {
            obj.GetComponent<GridCharacter>().UpdateEffectIcons();
        }
        if (obj.GType_ == GriddableObjectType.Enemy)
        {
            obj.GetComponent<GridEnemy>().UpdateEffectIcons();
        }
    }
    public void UpdateEffectIcons()
    {
        StaticFuncs.DestroyChildren(EffectObject);
        for (int i = 0; i < character_.effects.Count; i++)
        {
            var effect = Instantiate(EffectPrefab, EffectObject).GetComponent<EffectObject>();
            effect.effect = character_.effects[i];
            effect.transform.localPosition = new Vector3(0, 0.4f + 0.3f*i, 0);
            effect.UpdateData();

        }
    }

    public override List<GameObject> UpdateRollsUI(float start_alpha = 1f)
    {
        StaticFuncs.DestroyChildren(RollsUI);
        List<GameObject> rolls_list = new List<GameObject>();
        foreach (Roll roll in character_.CurrentRolls)
        {
            GameObject menu_roll = Instantiate(RollUIPrefab, RollsUI);
            menu_roll.GetComponent<CanvasGroup>().alpha = 0f;
            menu_roll.GetComponent<RollScript>().Fade(1f, 0.75f, 0.1f, true);
            menu_roll.GetComponent<RollScript>().ShowStats(roll);
            menu_roll.transform.localScale = (Vector3.one) / 250;
            menu_roll.transform.Rotate(Vector3.up, 180);
            rolls_list.Add(menu_roll);
            //Debug.Log(roll.minRoll + " " + roll.maxRoll);
        }
        if (rolls_list.Count > 6)
        {
            for (int i = 0; i < rolls_list.Count; i++)
            {
                rolls_list[i].transform.localPosition = new Vector3(0.225f * ((float)-Math.Pow(-1f, i + 1)), 0.2f + 0.45f * (i / 2), 0);
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

    public override void ChangeCharacterBase(int id)
    {
        base.ChangeCharacterBase(id);
        int level = character_.level;
        int hp = character_.hp;
        int energy = character_.energy;
        character_ = GridCharacter.GetCharacterByID(id);
        character_.level = level;
        character_.CreateStatsAccourdingToLevel();
        character_.hp = hp;
        character_.energy = energy;
    }
}



