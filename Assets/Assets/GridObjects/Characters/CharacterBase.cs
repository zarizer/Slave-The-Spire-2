using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterBase
{
    public int hp;
    public int def;
    public int speed;
    public int speed_dif;
    public float base_dmg;
    public int moves;

    public virtual void Init() { }
}
