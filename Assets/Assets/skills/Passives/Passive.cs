using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive
{
    public string Name;
    public string Description;
    public float Value;
    public CharacterBase character;
    public int level = 1;
    public static Passive GetPassiveInstance(Type type)
    {
        var passive = (Passive)Activator.CreateInstance(type);
        passive.Init();
        return passive;
    }

    virtual public void UpdateAccourdingToLevel() { }

    virtual public void Init() { UpdateAccourdingToLevel(); }

    virtual public void OnTurnStart(GridField field) { }

    virtual public void OnTurnEnd(GridField field) { }

    virtual public void OnAttack(GridField field, CharacterBase target, Damage dmg) { }

    virtual public void OnDealDamage(GridField field, Damage dmg, CharacterBase target) { }

    virtual public void OnGetDamage(GridField field, Damage dmg) { }

    virtual public void OnGetHeal(GridField field) { }

    virtual public void OnGetDefense(GridField field) { }

    virtual public void OnMove(GridField field) { }

    virtual public void OnBattleStart(GridField field) { }

    virtual public void OnDeath(GridField field, Damage dmg) { }

    virtual public void OnGetEnergy(GridField field, int value) { }
}
