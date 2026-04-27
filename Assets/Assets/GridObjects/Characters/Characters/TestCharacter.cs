using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : CharacterBase
{
    public override void Init() 
    {

        hp = 100;
        def = 10;
        speed = 10;
        speed_dif = 2;
        base_dmg = 1;
        moves = 8;
        energy = 20;
        name = "TestCharacter";
        skill_id1 = -1;
        skill_id2 = -2;
        skill_id3 = -1;
        skill_id4 = -2;
        base.Init();
    }

    public int GetMoves() { return moves; }
}
