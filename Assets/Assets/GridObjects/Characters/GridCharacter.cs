using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCharacter : GriddableObject
{


    public GameObject TexturePlane;
    public CharacterBase character_;
    public int CharacterId_ = -1;
    
    public List<Roll> CurrentSkillRolls = new List<Roll>();
    public List<Roll> DefenceRolls = new List<Roll>();

    void Start()
    {
        character_ = GetCharacterByID(CharacterId_);
        player_ = true;
    }

    void Update()
    {
        LookAtCamera();
        MoveToDestination();
    }



    private void OnEnable()
    {
        GType_ = GriddableObjectType.Character;
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
        character_ = GetCharacterByID(id);
    }
    CharacterBase GetCharacterByID(int id)
    {
        CharacterBase ret_character = null;
        

        ret_character = DataDicts.CharacterSet[id].Clone();
        
        Debug.Log("Got Character: " + ret_character.name + " id: "+ id);
        
        if (ret_character == null) ret_character = new TestCharacter();
        
        ret_character.Init();
        return ret_character;
    }

    public bool CanMove()
    {
        if (character_.cur_moves>0) return true;
        return false;
    }

    public void MakeCurrentRolls(int skill_num)
    {
        if (skill_num == 1) CurrentSkillRolls = new List<Roll>(character_.Skill1.rolls);
        if (skill_num == 2) CurrentSkillRolls = new List<Roll>(character_.Skill2.rolls);
        if (skill_num == 3) CurrentSkillRolls = new List<Roll>(character_.Skill3.rolls);
        if (skill_num == 4) CurrentSkillRolls = new List<Roll>(character_.Skill4.rolls);
    }

    public override Roll GetFirstRoll() 
    { 
        if (DefenceRolls.Count == 0) return null;
        return DefenceRolls[0];
    }

    public override void RemoveFirstRoll(float offset = 0f) { DefenceRolls.RemoveAt(0); }
}






