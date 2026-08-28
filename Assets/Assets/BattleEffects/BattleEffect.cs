using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class BattleEffect
{
    public int duration;
    public int power;
    public string name;
    public string description;
    public Texture image;
    public static string image_name;
    public CharacterBase character;
    public CharacterBase source;

    public static BattleEffect GetEffectInstance(Type type)
    {
        var effect = (BattleEffect)Activator.CreateInstance(type);
        effect.Init();
        return effect;
    }
    virtual public void Init() 
    {
        string iconsPath = Path.Combine(Application.streamingAssetsPath, "effect_icons/");
        Sprite sprite = LoadSprite(iconsPath + image_name);
        image = sprite.texture;

    }

    virtual public void OnTurnStart(GridField field) { 
        duration--;
        if  (duration <= 0)
        {
            OnEffectEnd(field, this);
            character.effects.Remove(this);
        }
        if (character.object_ == null) return;
        GridCharacter.TryUpdateEffectIcons(character.object_.GetComponent<GriddableObject>());
    }

    virtual public void OnTurnEnd(GridField field) { }

    virtual public void OnAttack(GridField field, CharacterBase target, Damage dmg) { }

    virtual public void OnDealDamage(GridField field, Damage dmg, CharacterBase target) { }

    virtual public void OnGetDamage(GridField field, Damage dmg) { }

    virtual public void OnGetHeal(GridField field) { }

    virtual public void OnGetDefense(GridField field) { }

    virtual public void OnMove(GridField field) { }

    virtual public void OnDeath(GridField field, Damage dmg) { }

    virtual public void OnApply(GridField field, BattleEffect effect, int ex_power, int ex_duration, bool is_continue) { }

    virtual public void OnEffectEnd(GridField field, BattleEffect effect) { }

    virtual public void OnKill(GridField field, CharacterBase target) { }

    private static Sprite LoadSprite(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "character_icons", fileName);

        #if UNITY_ANDROID && !UNITY_EDITOR
                            byte[] imageData = ReadFileBytesFromStreamingAssets(filePath);
        #else
                byte[] imageData = File.ReadAllBytes(filePath);
        #endif

        if (imageData != null && imageData.Length > 0)
        {
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageData))
            {
                // Создаем Sprite из Texture2D
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f),
                    100f
                );
                return sprite;
            }
        }
        return null;
    }
}

