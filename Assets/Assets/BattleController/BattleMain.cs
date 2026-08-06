using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleMain : MonoBehaviour
{
    [SerializeField] private Camera battle_camera;
    [SerializeField] private Camera ui_camera;
    [SerializeField] private GameObject battle_ui;
    [SerializeField] private GameObject ui_ui;
    public bool IsInBattle = true;
    public bool PlayerCanAttack = false;
    public int turn = 0;
    public bool lock_cycle = false;
    public bool make_next_cycle_on_unlock = false;
    public int ext_data_counter = 0;
    public List<CharacterBase> play_characters = new List<CharacterBase>();
    public int CurrentLevelId = 0;
    public int level_num;
    public int compaign_num;
    public int chapter_num;
    public int difficulty;

    List<LevelId> variants = new List<LevelId>();
    List<LevelId> variants_normal = new List<LevelId>();
    List<LevelId> variants_hard = new List<LevelId>();

    GameObject CurrentLevel = null;

    List<GameObject> DestroyList = new List<GameObject>();

    List<BattleCycle> cycles = new List<BattleCycle>
    {
        BattleCycle.EnemyRollsCreate,
        BattleCycle.PlayerTurn,
        BattleCycle.EnemyTurn1,
        BattleCycle.EnemyTurn2
    };

    private BattleCycle current_cycle;

    public GridField current_field;

    void Start()
    {
        

        LeanTween.init(10000);
        Random.InitState(System.DateTime.Now.Second + System.DateTime.Now.Minute + System.DateTime.Now.Millisecond);
        LevelData.Init();
    }

    private void Awake()
    {
        Skills.Init();
        //Debug.Log(Application.companyName + " " + Application.productName + " " + Application.persistentDataPath);
    }


    void Update()
    {
        CheckCycleLock();
    }

    void DebugAddPlayCharacters()
    {
        play_characters.Add(GridCharacter.GetCharacterByID(0));
        //play_characters.Add(GridCharacter.GetCharacterByID(0));
        //play_characters.Add(GridCharacter.GetCharacterByID(0));
        //play_characters.Add(GridCharacter.GetCharacterByID(0));
        //play_characters.Add(GridCharacter.GetCharacterByID(0));
    }

    public void StartGame()
    {
        play_characters.Clear();
        DebugAddPlayCharacters();
        StartBattle();
    }

    [ContextMenu("StartBattle")]
    public void StartBattle(bool start_battle = true)
    {
        UpdateCharacters();
        battle_ui.SetActive(true);
        ui_camera.gameObject.SetActive(false);
        battle_camera.gameObject.SetActive(true);
        SetBackGroundAccourdingToLevelId(CurrentLevelId);
        if (current_field != null) Destroy(current_field.gameObject);
        current_field = Instantiate(ResoursesDict.ObjectSet["Field"]).GetComponent<GridField>();
        ResoursesDict.GetClass<CameraController>().Target = current_field.transform;
        if (start_battle)
        {
            current_field.StartField(this);
            current_cycle = cycles[0];
            turn = 0;
            foreach (var character in current_field.GridCharacters)
            {
                character.GetCharacter().OnBattleStart(current_field);
            }
            NextCycle(0);
        }
    }

    public void StartLevel(int level_id)
    {
        CurrentLevelId = level_id;
        StartBattle();
    }

    [ContextMenu("StopCode")]
    public void StopCode()
    {
        
    }

    public void NextTurn()
    {
        if (current_field.GridEnemies.Count <= 0 && CurrentLevelId > 20)
        {
            NextLevel();
            return;
        }
        else
        {
            NextCycle();
        }
    }

    public void NextCycle(int num = 1, bool activate = true)
    {
        var next_cycle = cycles[(cycles.IndexOf(current_cycle) + num) % cycles.Count];
        Debug.Log("NextCycle: " + next_cycle);

        current_cycle = next_cycle;
        if (activate)
        {
            if (current_cycle == BattleCycle.EnemyRollsCreate)
            {
                UpdatePlayerSkills();
                UpdateEnemySkills();
                UpdateObstacleSkills();
                CharacterTabSwitch(true, false);
                StartCoroutine(EnemyCreateRolls());
                OnTurnStart();
                NextCycle();
            }
            else if (current_cycle == BattleCycle.PlayerTurn)
            {
                
                CharacterTabSwitch(false, false, true);
                PlayerCanAttack = true;
            }
            else if (current_cycle == BattleCycle.EnemyTurn1)
            {
                CharacterTabSwitch(true, false);
                PlayerCanAttack = false;
                StartCoroutine(EnemiesMove());
            }
            else if (current_cycle == BattleCycle.EnemyTurn2)
            {
                StartCoroutine(EnemiesUseRolls());
                turn++;
                OnTurnEnd();
                
                
            }
        }
    }

    IEnumerator EnemyCreateRolls()
    {
        foreach (var enemy in current_field.GridEnemies)
        {
            enemy.enemy_.CreateSkills(turn);
            while (enemy.enemy_.CreateNextRolls()) { }
            yield return new WaitForSeconds(0.2f);
            enemy.UpdateRollsUI(1f);
            for (int i = 0; i < enemy.enemy_.CurrentRolls.Count; i++)
            {
                enemy.RollsUI.GetChild(i).GetComponent<RollScript>().Fade(1f, 0.75f, i * 0.5f, true);
            } 
        }
    }

    void EnemyUpdateRolls(GridEnemy enemy)
    {
        StaticFuncs.DestroyChildren(enemy.RollsUI);
        var list = enemy.UpdateRollsUI();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].GetComponent<RollScript>().Fade(1f, 0.75f, i * 0.5f, true);
        }
    }

    [ContextMenu("NextCycle")]
    public void DebugNextCycle()
    {
        NextCycle();
    }

    void CharacterTabSwitch(bool locked, bool enable, bool need_lock = true, bool need_enable = true)
    {
        if (need_enable) { ResoursesDict.ObjectSet["CharacterTab"].GetComponent<CharacterTabController>().RequestedUpdate(enable); }
        if (need_lock) { ResoursesDict.ObjectSet["CharacterTab"].GetComponent<CharacterTabController>().IsLocked = locked; }
    }

    void CheckCycleLock()
    {
        if (!lock_cycle && make_next_cycle_on_unlock)
        {
            make_next_cycle_on_unlock = false;
            NextCycle();
        } 
    }

    public int GetEnemyRollsCount()
    {
        int counter = 0;
        foreach (var enemy in current_field.GridEnemies)
        {
            counter += enemy.RollsUI.childCount;
        }
        return counter;
    }

    public void MakeFight(GridCharacter character, List<GriddableObject> Targets)
    {
        //Debug.Log("ATTACK!");
        for (int i = 0; i<character.CurrentSkillRolls.Count; i += 0)
        {
            var char_roll = character.CurrentSkillRolls[0];
            for (int j = 0; j < Targets.Count; j++) {
                var target_roll = Targets[i].GetFirstRoll();
                if (target_roll == null) { DealDamageByRoll(char_roll, Targets[i]); }
                else
                {
                    if (target_roll.rollType == RollType.Def)
                    {
                        int char_power;
                        int target_power;
                        GetRolls(out char_power, out target_power, char_roll, target_roll);
                        int init_roll = target_roll.maxRoll;
                        target_roll.maxRoll -= char_power;
                        if (target_roll.maxRoll < 0)
                        {
                            int dmg = char_power - init_roll;
                            char_roll.maxRoll = dmg;
                            char_roll.minRoll = dmg;
                            DealDamageByRoll(char_roll, Targets[i]);
                            Targets[i].RemoveFirstRoll(0.3f);
                            if (Targets[i].GType_ == GriddableObject.GriddableObjectType.Enemy)
                            {
                                var enemy = Targets[i].GetComponent<GridEnemy>();

                                for (int k = 0; k < enemy.RollsUI.childCount; k++)
                                {
                                    enemy.RollsUI.GetChild(k).GetComponent<RollScript>().Fade(0f, 0.3f, 0, true);
                                }
                                LeanTween.delayedCall(0.4f, () => { EnemyUpdateRolls(enemy); });
                            }
                        }
                        else
                        {
                            if (Targets[i].GType_ == GriddableObject.GriddableObjectType.Enemy)
                            {
                                var enemy = Targets[i].GetComponent<GridEnemy>();

                                for (int k = 0; k < enemy.RollsUI.childCount; k++)
                                {
                                    enemy.RollsUI.GetChild(k).GetComponent<RollScript>().MinMaxText.text = enemy.RollsUI.GetChild(k).GetComponent<RollScript>().roll.maxRoll.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        int char_power;
                        int target_power;
                        GetRolls(out char_power, out target_power, char_roll, target_roll);
                        Debug.Log("Fighting: " + char_power + " / " + target_power);
                        while (char_power == target_power)
                        {
                            Debug.Log("Tie: " + char_power + " | " + target_power);
                            GetRolls(out char_power, out target_power, char_roll, target_roll);
                            Debug.Log("Fighting: " + char_power + " / " + target_power);
                        }
                        if (char_power > target_power)
                        {
                            DealDamageByRoll(char_roll, Targets[i]);
                            Targets[i].RemoveFirstRoll(0.3f);
                            if (Targets[i].GType_ == GriddableObject.GriddableObjectType.Enemy)
                            {
                                var enemy = Targets[i].GetComponent<GridEnemy>();

                                for (int k = 0; k < enemy.RollsUI.childCount; k++)
                                {
                                    enemy.RollsUI.GetChild(k).GetComponent<RollScript>().Fade(0f, 0.3f, 0, true);
                                }
                                LeanTween.delayedCall(0.4f, () => { EnemyUpdateRolls(enemy); });
                            }
                        }
                    }    
                } 
            }
            character.CurrentSkillRolls.RemoveAt(0);
        }
    }


    public void MakeFightEnemy(GridEnemy enemy, Roll roll, List<GriddableObject> Targets)
    {
        //Debug.Log("ATTACK!");


        for (int j = 0; j < Targets.Count; j++)
        {
            var target = Targets[j];
            var target_roll = target.GetFirstRoll();
            if (target == enemy) continue;
            if (target_roll == null) { DealDamageByRoll(roll, target); }
            else
            {
                int char_power;
                int target_power;
                GetRolls(out char_power, out target_power, roll, target_roll);
                Debug.Log("Fighting: " + char_power + " / " + target_power);
                while (char_power == target_power)
                {
                    Debug.Log("Tie: " + char_power + " | " + target_power);
                    GetRolls(out char_power, out target_power, roll, target_roll);
                    Debug.Log("Fighting: " + char_power + " / " + target_power);
                }
                if (char_power > target_power)
                {
                    DealDamageByRoll(roll, target);
                    target.RemoveFirstRoll(0.3f);
                    if (target.GType_ == GriddableObject.GriddableObjectType.Enemy)
                    {
                        var cur_enemy = target.GetComponent<GridEnemy>();

                        for (int k = 0; k < cur_enemy.RollsUI.childCount; k++)
                        {
                            cur_enemy.RollsUI.GetChild(k).GetComponent<RollScript>().Fade(0f, 0.3f, 0, true);
                        }
                        LeanTween.delayedCall(0.4f, () => { EnemyUpdateRolls(cur_enemy); });
                    }
                }
            }
        }

        
    }

    void GetRolls(out int p1, out int p2, Roll roll1, Roll roll2)
    {
        int pow1 = roll1.GetRoll();
        int pow2 = roll2.GetRoll();

        p1 = pow1 + (roll1.skill.character.level - roll2.skill.character.level) / 4;
        p2 = pow2;
    }

    IEnumerator EnemiesMove()
    {
        current_field.CellsNullify();
        foreach (var enemy in current_field.GridEnemies)
        {
            var cell = current_field.EnemyFindBestCell(enemy);
            enemy.MoveToCell(cell);
            yield return new WaitForSeconds(1.5f);
        }
        NextCycle();
    }

    public void DealDamageByRoll(Roll roll, GriddableObject obj)
    {
        
        Damage damage = new Damage(roll.GetDamage(), roll.element, roll.skill.character);
        foreach (var effect in roll.effects)
        {
            if (effect.Item4 == 0)
            {
                GridCharacter.ApplyBattleEffect(DataDicts.EffectTypes[effect.Item1], effect.Item2,
                    effect.Item3, obj.GetCharacter(), roll.skill.character);
            }
        }
        obj.GetDamage(damage);
        Debug.Log("Dealed damage!" + damage.damage);
    }

    IEnumerator EnemiesUseRolls()
    {
        for (int i = 0; i < current_field.GridEnemies.Count; i++)
        {
            var enemy = current_field.GridEnemies[i];
            while (enemy.GetAtkRollCount() != 0)
            {
                var roll = enemy.GetFirstAtkRoll();
                Direction dir;
                var cell = current_field.FindBestCellForRoll(roll, enemy, out dir);
                var dist_cells = current_field.GetTargetCellsForRollFromEnemy(roll, enemy);
                foreach (var dist_cell in dist_cells)
                {
                    dist_cell.FlashColor(Color.yellow, 1f);
                }
                yield return new WaitForSeconds(2.1f);

                var target_cells = current_field.GetDamageCellsFromTargetCell(cell, roll, dir);
                foreach (var target_cell in target_cells)
                {
                    target_cell.FlashColor(Color.red, 1f);
                }
                yield return new WaitForSeconds(2.1f);

                var objects = new List<GriddableObject>();
                foreach (var obj in target_cells)
                {
                    if (obj.object_ != null) objects.Add(obj.object_);
                }
                MakeFightEnemy(enemy, roll, objects);
                enemy.RemoveRoll(roll);
                EnemyUpdateRolls(enemy);
            }
            
            
        }
        NextCycle();
    }

    void UpdatePlayerSkills()
    {
        foreach (var character in current_field.GridCharacters)
        {
            character.character_.UpdateStatsOnNewTurn();
        }
    }
    void UpdateObstacleSkills()
    {
        foreach (var character in current_field.GridObstacles)
        {
            ((ObstacleBase)(character.GetCharacter())).cur_use_count = ((ObstacleBase)(character.GetCharacter())).use_count;
        }
    }

    void SetLevel(string name)
    {

        if (CurrentLevel != null) Destroy(CurrentLevel);
        CurrentLevel = Instantiate(ResoursesDict.ObjectSet[name]);
    }

    public void SetBackGroundAccourdingToLevelId(int levelId)
    {
        LevelId level = new LevelId(levelId);
        if (levelId == 0)
        {
            SetLevel("Level0");
            return;
        }
        if (level.compaign == 1)
        {
            if (level.chapter == 1)
            {
                SetLevel("Level1.1");
            }
        }
               
    }

    void GetLevelCurrentChapterVariants()
    {
        variants.Clear();
        var levels = LevelData.LevelAmounts[compaign_num][chapter_num];
        foreach (var level in levels) {
            var cur_level = new LevelId(level);
            variants.Add(cur_level);
            if (cur_level.is_hard) variants_hard.Add(cur_level);
            else variants_normal.Add(cur_level);
        }
    }

    int GetNextLevelId(int curLevelId)
    {
        int next_id = -1;
        if (curLevelId < 10)
        {
            GetLevelCurrentChapterVariants();
            

            bool is_hard;
            if (Random.Range(0, level_num) > 3) is_hard = true;
            else is_hard = false;

            LevelId level;
            if (is_hard) level = variants_hard[Random.Range(0, variants_hard.Count-1)];
            else level = variants_normal[Random.Range(0, variants_normal.Count-1)];
            level.difficulty = difficulty;

            next_id = level.GetLevelId();
        }
        else
        {
            if (level_num < 9)
            {
                level_num++;
                return 1;
            }
        }
        return next_id;
    }

    public void NextLevel()
    {
        CurrentLevelId = GetNextLevelId(CurrentLevelId);
        UpdateCharacters();
        StartBattle();
    }

    public void ApplyBaff(BattleBuff buff)
    {
        foreach (var character in play_characters)
        {
            if (buff.OneTime)
            {
                buff.MakeBuff(character);
            }
            else
            {
                character.buffs.Add(buff);
            }
        }
    }

    public static void UseBaffs(CharacterBase character)
    {
        foreach (var buff in character.buffs)
        {
            buff.MakeBuff(character);
        }
    }

    void UpdateCharacters()
    {
        foreach (var character in play_characters)
        {
            character.Init();
            character.CreatePassives();
            UseBaffs(character);
            character.CreateStatsAccourdingToLevel();
        }
        
    }

    void UpdateEnemySkills()
    {
        foreach (var enemy in current_field.GridEnemies)
        {
            enemy.GetCharacter().UpdateStatsOnNewTurn();
        }
    }
    

    void OnTurnStart()
    {
        foreach(var obj in current_field.GridObjects)
        {
            obj.GetCharacter().OnTurnStart(current_field);
        }
    }

    void OnTurnEnd()
    {
        foreach (var obj in current_field.GridObjects)
        {
            obj.GetCharacter().OnTurnEnd(current_field);
        }
    }

}




enum BattleCycle
{
    EnemyRollsCreate,
    PlayerTurn,
    EnemyTurn1,
    EnemyTurn2
};

class LevelId
{
    public bool correct;
    public int compaign;
    public int chapter;
    public int level;
    public int difficulty;
    public bool is_hard; 

    public LevelId(int level_id)
    {
        correct = false;
        if (level_id >= 0 && level_id <= 5) correct = true;
        if (level_id / 10000000 == 52) correct = true;

        compaign = (level_id / 100000) % 100;

        chapter = (level_id / 1000) % 100;

        level = (level_id / 10) % 100;

        difficulty = level_id % 10;

        if (level >= 50) is_hard = true;
        else is_hard = false;

    }

    public LevelId()
    {
        correct = true;
        compaign= 0;
        chapter = 0;
        level = 0;
        difficulty = 0;
        is_hard = false;
    }

    public int GetLevelId()
    {
        return 52 * 10000000 + compaign * 100000 + chapter * 1000 + level * 10 + difficulty;
    }
}