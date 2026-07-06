using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : CharacterBase
{
    public override CharacterBase Init() 
    {
        id = 0;
        hp = 100;
        def = 10;
        speed = 10;
        level = 1;
        speed_dif = 2;
        dmg_k = 1.5f;
        moves = 8;
        energy = 20;
        name = "Райан Гослинг Тестовый";
        description = "ЕМУ ПОЕБАТЬ\nshjbsbcsbchchjsbhcbhjjsdbhfsbhdbvhdbdhcbghxdvghcghdbchbdhcbhdbchjjbhdcbshebchbsdhcbshdbcbsdcbhjsdbchjsbdcbsdbschsghdc-hsbghfcbsghbghbghbzhgbhsbhjsdbhsd";
        skill_id1 = 1;
        skill_id2 = 1;
        skill_id3 = 1;
        skill_id4 = 0;
        return this;
    }

    public int GetMoves() { return moves; }
}
