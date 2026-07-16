using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * На момент CharacterBase.Init() CharacterBase.GetSpecialValue() ещё не работает, все вещи связанные со
 * SpecialValue нужно получать как минимум на момент CharacterBase.OnLevelStart(), иначе
 * SpecialValue всегда будет 0!
*/

public class SimpleStone : ObstacleBase
{

    public override CharacterBase Init()
    {
        id = 0;
        hp = 20;
        def = 0;
        speed = 0;
        speed_dif = 0;
        dmg_k = 0f;
        moves = 0;
        energy = 0;
        name = "Обычный камень";
        description = "всё ещё обычный камень";
        ModelId = 0;
        base.Init();
        return this;
    }
};


public class Chapter1RandomObstacle : ObstacleBase
{

    public override CharacterBase Init()
    {
        id = 3;
        hp = 20;
        def = 0;
        speed = 0;
        speed_dif = 0;
        dmg_k = 0f;
        moves = 0;
        energy = 0;
        name = "Обычный камень";
        description = "всё ещё обычный камень";
        ModelId = Random.Range(3, 6);
        base.Init();
        return this;
    }

    public override void OnSpawn(GridField field_data)
    {
        base.OnSpawn(field_data);
        Transform t = field_data.current_object.transform;
        t.RotateAround(t.position, Vector3.up, Random.Range(0, 360));
    }
};


public class CharacterSpawn : ObstacleBase
{
    public override CharacterBase Init()
    {
        id = 1;
        hp = 1;
        def = 0;
        speed = 0;
        speed_dif = 0;
        dmg_k = 0f;
        moves = 0;
        energy = 0;
        name = "Спавнпоинт";
        description = "Здесь появляется персонаж игрока";
        ModelId = 1;
        base.Init();
        return this;
    }

    public override void OnSpawn(GridField field_data)
    {
        base.OnSpawn(field_data);
        GridCell cell = field_data.current_object.cell_;
        field_data.RemoveObject(field_data.current_object);
        field_data.SpawnCharacter(cell.x_, cell.y_);
        field_data.character_spawn_num++;
    }


};

public class LevelChange : ObstacleBase
{
    public override CharacterBase Init()
    {
        id = 2;
        hp = 1;
        def = 0;
        speed = 0;
        speed_dif = 0;
        dmg_k = 0f;
        moves = 0;
        energy = 0;
        name = "Дверь";
        description = "Начинает путешествие до качалки, чтобы получить звание мастера";
        skills_description = "Начинает путешествие до качалки, чтобы получить звание мастера";
        InteractVariants = 1;
        ModelId = 2;
        Interactable = true;
        use_count = 1;
        base.Init();
        return this;
    }

    public override void OnInteract1(GridField field_data)
    {
        base.OnInteract1(field_data);
        if (GetSpecialValue() == 0)
        {
            ResoursesDict.GetClass<BattleMain>().chapter_num = 1;
            ResoursesDict.GetClass<BattleMain>().compaign_num = 1;
            ResoursesDict.GetClass<BattleMain>().level_num = 1;
        }
        ResoursesDict.GetClass<BattleMain>().NextLevel();
        Debug.Log("inter1");
    }

    public override void OnLevelStart(GridField field_data)
    {
        base.OnLevelStart(field_data);
        if (GetSpecialValue() == 1)
        {
            skills_description = "продолжает ваш путь";
        }
    }
};

public class BuffAltar : ObstacleBase
{
    BattleBuff var1;
    BattleBuff var2;
    BattleBuff var3;
    public override CharacterBase Init()
    {
        id = 4;
        hp = 1;
        def = 0;
        speed = 0;
        speed_dif = 0;
        dmg_k = 0f;
        moves = 0;
        energy = 0;
        name = "Алтарь силы";
        description = "Придаёт силы отморозкам";
        CreateRandomBaff(out var1);
        CreateRandomBaff(out var2);
        CreateRandomBaff(out var3);
        skills_description = "Баффы: \n\n" + var1.Name + ":\n" + var1.Description + "\n\n" + var2.Name + ":\n" + var2.Description + "\n\n" + var3.Name + ":\n" + var3.Description;
        InteractVariants = 3;
        ModelId = 7;
        Interactable = true;
        use_count = 1;
        one_time_use = true;
        base.Init();
        return this;
    }

    void CreateRandomBaff(out BattleBuff slot)
    {
        slot = null;
        var baff = DataDicts.BaffTypes[UnityEngine.Random.Range(0, DataDicts.BaffTypes.Count - 1)];
        slot = BattleBuff.GetBaffInstance(baff);
    }

    public override void OnInteract1(GridField field_data)
    {
        base.OnInteract1(field_data);
        ResoursesDict.GetClass<BattleMain>().ApplyBaff(var1);
        ToggleOffParticles(field_data);
    }
    public override void OnInteract2(GridField field_data)
    {
        base.OnInteract1(field_data);
        ResoursesDict.GetClass<BattleMain>().ApplyBaff(var2);
        ToggleOffParticles(field_data);
    }
    public override void OnInteract3(GridField field_data)
    {
        base.OnInteract1(field_data);
        ResoursesDict.GetClass<BattleMain>().ApplyBaff(var3);
        ToggleOffParticles(field_data);
    }

    private void ToggleOffParticles(GridField field_data)
    {
        ParticleSystem particles;
        if (StaticFuncs.RecursiveGetComponentDeep<ParticleSystem>(object_, out particles))
        {
            particles.Stop();
        }
    } 
};