using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBase : CharacterBase
{
    public bool Destroyable = false;
    public bool Interactable = false;
    public int InteractVariants = 1;
    public int ModelId = 0;
    public int use_count = 0;
    public int cur_use_count = 0;
    public bool one_time_use = false;
    public bool is_showing_passives = true;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override CharacterBase Init()
    {
        base.Init();
        
        return this;
    }

    public ObstacleBase()
    {

    }
    public ObstacleBase(ObstacleBase other) : base(other)
    {
        this.Destroyable = other.Destroyable;
        this.Interactable = other.Interactable;
        this.ModelId = other.ModelId;
        this.InteractVariants = other.InteractVariants;
    }

    public override CharacterBase Clone()
    {
        return new ObstacleBase(this);
    }

    public override void OnSpawn(GridField field_data)
    {
        base.OnSpawn(field_data);
    }


    public virtual void OnInteract1(GridField field_data)
    {
        cur_use_count--;
    }
    public virtual void OnInteract2(GridField field_data)
    {
        cur_use_count--;
    }
    public virtual void OnInteract3(GridField field_data)
    {
        cur_use_count--;
    }
    public virtual void OnInteract4(GridField field_data)
    {
        cur_use_count--;
    }

    public virtual bool InteractReq1(GridField field_data) { return true; }
    public virtual bool InteractReq2(GridField field_data) { return true; }
    public virtual bool InteractReq3(GridField field_data) { return true; }
    public virtual bool InteractReq4(GridField field_data) { return true; }

    public override void CreateStatsAccourdingToLevel()
    {
        base.CreateStatsAccourdingToLevel();
        //Debug.Log("Level:" + level.ToString() + "  k:" + (1 + ((float)level) / 12).ToString());
        cur_hp = (int)(cur_hp * (1 + ((float)level) / 12));
        start_hp = (int)(start_hp * (1 + ((float)level) / 12));
        cur_def = (int)(cur_def * (1 + ((float)level) / 12));
        float baff_k = (1 - (float)level / 500);
        cur_dmg_k += (level / 4) / 10f;
        fire_k = (float)Math.Round(fire_k * baff_k, 2);
        water_k = (float)Math.Round(water_k * baff_k, 2);
        dendro_k = (float)Math.Round(dendro_k * baff_k, 2);
        light_k = (float)Math.Round(light_k * baff_k, 2);
        darkness_k = (float)Math.Round(darkness_k * baff_k, 2);
        none_k = (float)Math.Round(none_k * baff_k, 2);


        for (int i = 0; i < level / 40; i++)
        {
            foreach (var passive in passives)
            {
                passive.level++;
            }
        }
    }

}
