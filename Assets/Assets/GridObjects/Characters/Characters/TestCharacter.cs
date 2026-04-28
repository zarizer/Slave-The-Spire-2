using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : CharacterBase
{
    public override CharacterBase Init() 
    {
        id = -1;
        hp = 100;
        def = 10;
        speed = 10;
        speed_dif = 2;
        base_dmg = 1;
        moves = 8;
        energy = 20;
        name = "Райан Гослинг Тестовый";
        description = "ЕМУ ПОЕБАТЬ\nshjbsbcsbchchjsbhcbhjjsdbhfsbhdbvhdbdhcbghxdvghcghdbchbdhcbhdbchjjbhdcbshebchbsdhcbshdbcbsdcbhjsdbchjsbdcbsdbschsghdc-hsbghfcbsghbghbghbzhgbhsbhjsdbhsd";
        skill_id1 = -1;
        skill_id2 = -2;
        skill_id3 = -1;
        skill_id4 = -2;
        base.Init();
        return this;
    }

    public int GetMoves() { return moves; }
}
