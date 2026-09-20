using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class BattleMain : MonoBehaviour
{
    [SerializeField] private Camera battle_camera;
    [SerializeField] private Camera ui_camera;
    [SerializeField] private GameObject battle_ui;
    [SerializeField] private GameObject ui_ui;
    [SerializeField] private EndGameMenu EndGameMenu;
    public bool IsInBattle = false;
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
    public static int Swagapoints;
    public bool is_custom_level;

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
        
        //Debug.Log(Application.companyName + " " + Application.productName + " " + Application.persistentDataPath);
    }


    void Update()
    {
        CheckCycleLock();
    }

    void DebugAddPlayCharacters()
    {
        play_characters.Add(GridCharacter.GetCharacterByID(0));
        play_characters.Add(GridCharacter.GetCharacterByID(1));
        //play_characters.Add(GridCharacter.GetCharacterByID(0));
        //play_characters.Add(GridCharacter.GetCharacterByID(0));
        //play_characters.Add(GridCharacter.GetCharacterByID(0));
        //play_characters.Add(GridCharacter.GetCharacterByID(0));
    }

    void GetPlayCharacters()
    {
        foreach (CharacterCard card in CharacterMenuController.selected_characters)
        {
            var c = GridCharacter.GetCharacterByID(card.character.id, card.character);
            
            play_characters.Add(c);
        }
        
    }
    void GetSavePlayCharacters()
    {
        for (int i = 0; i < ProfileManager.profile.save_character_ids.Count && i < 4; i++)
        {
            //CharacterCard c_card;
            //var c = GridCharacter.GetCharacterByID(ProfileManager.profile.save_character_ids[i], c_card.character);

            //play_characters.Add(c);
        }

    }

    public void StartGame()
    {
        IsInBattle = true;
        play_characters.Clear();
        GetPlayCharacters();
        StartBattle();
    }
    public void StartFromSave()
    {
        IsInBattle = true;
        play_characters.Clear();
        GetPlayCharacters();
        compaign_num = ProfileManager.profile.compaign_num;
        chapter_num = ProfileManager.profile.chapter_num;
        level_num = ProfileManager.profile.level_num;
        CurrentLevelId = ProfileManager.profile.level_id;
        difficulty = ProfileManager.profile.difficulty;
        StartBattle();
    }

    [ContextMenu("StartBattle")]
    public void StartBattle(bool start_battle = true)
    {
        
        ResoursesDict.GetClass<CameraController>().lock_navigation = false;
        LoadProfileData();
        UpdateCharacters();
        ProfileManager.profile.compaign_num = compaign_num;
        ProfileManager.profile.chapter_num = chapter_num;
        ProfileManager.profile.level_num = level_num;
        ProfileManager.profile.level_id = CurrentLevelId;
        ProfileManager.profile.difficulty = difficulty;
        ProfileManager.profile.is_in_game = true;
        ProfileManager.SaveProfile();

        Swagapoints = 120;
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
            StartCoroutine(NextCycle(0));
        }
        
    }

    void LoadProfileData()
    {
        for (int i = 0; i < ProfileManager.profile.baffs_id.Count; i++)
        {
            foreach(CharacterBase c in play_characters)
            {
                if (c.buffs.Count == 0)
                {
                    BattleBuff buff = BattleBuff.GetBaffInstance(DataDicts.BaffTypes[ProfileManager.profile.baffs_id[i]]);
                    buff.Value = ProfileManager.profile.baffs_counters[i];
                    c.buffs.Add(buff);
                }

            }
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

    public void EndGame(bool Isvictory)
    {
        ProfileManager.profile.baffs_id.Clear();
        ProfileManager.profile.baffs_counters.Clear();
        ProfileManager.profile.is_in_game = false;
        ProfileManager.SaveProfile();
        ResoursesDict.GetClass<UIController>().CloseAllObjectTabs();
        ResoursesDict.GetClass<UIController>().CloseTabs();
        SetLevel("None");
        battle_camera.gameObject.SetActive(false);
        ui_camera.gameObject.SetActive(true);
        ResoursesDict.GetClass<MainMenuManager>().SetMenu(EndGameMenu.gameObject);
        int budgets = (int)(4 * ProfileManager.profile.levels_counter);
        if (Isvictory) { budgets += 50; }
        budgets = (int)(budgets * GetAverageSwagaK());
        EndGameMenu.CreateStats(Isvictory, budgets);
        ProfileManager.profile.budget += budgets;
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
            StartCoroutine(NextCycle());
        }
    }

    public IEnumerator NextCycle(int num = 1, bool activate = true)
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
                StartCoroutine(NextCycle());
            }
            else if (current_cycle == BattleCycle.PlayerTurn)
            {
                if (current_field.GridCharacters.Count <= 0)
                {
                    EndGame(false);
                }
                Swagapoints -= 25;
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
                yield return StartCoroutine(EnemiesUseRolls());
                
                turn++;
                OnTurnEnd();


            }
        }
    }

    IEnumerator EnemyCreateRolls()
    {
        yield return new WaitForEndOfFrame();
        foreach (var enemy in current_field.GridEnemies)
        {
            enemy.enemy_.CreateSkills(turn);
            while (enemy.enemy_.CreateNextRolls()) { }
            yield return new WaitForSeconds(0.2f * ProfileManager.profile.roll_speed);
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

    int GetLevelBonus(CharacterBase attacker, CharacterBase target, Roll roll)
    {
        if (roll.IsScaling) { return 0; }
        return (attacker.level - target.level) / 4;
    }

    private IEnumerator MakeFightInternal(GriddableObject attacker, Roll roll, List<GriddableObject> Targets, bool isEnemyAttacker)
    {
        ProcessOnUseEffects(roll.skill, attacker.GetCharacter(), null);

        int attackerRollValue = roll.GetRoll();

        int maxTargetLevel = 0;
        foreach (var target in Targets)
        {
            if (target == attacker) continue;
            int targetLevel = target.GetCharacter().level;
            if (targetLevel > maxTargetLevel)
                maxTargetLevel = targetLevel;
        }

        bool hasAnyTargetRoll = false;
        foreach (var target in Targets)
        {
            if (target == attacker) continue;
            var target_roll = target.GetFirstRoll();
            if (target_roll != null)
            {
                hasAnyTargetRoll = true;
                break;
            }
        }

        List<FightResult> results = new List<FightResult>();

        for (int j = 0; j < Targets.Count; j++)
        {
            var target = Targets[j];

            if (target.GetCharacter() == attacker.GetCharacter() && !roll.selfDamage)
            {
                continue;
            }

            FightResult result = new FightResult();
            result.target = target;
            result.context = new RollContext();
            result.context.Attacker = attacker.GetCharacter();
            result.context.Defender = target.GetCharacter();
            result.context.RollValue = attackerRollValue;
            result.isSelfAttack = (target == attacker);
            result.attackerRollValue = attackerRollValue;
            result.hasTargetRoll = false;

            var target_roll = target.GetFirstRoll();
            result.targetRoll = target_roll;
            result.attackerRoll = roll;

            // === —ќ’–јЌя≈ћ »Ќƒ≈ —џ ===
            result.attackerRollIndex = GetRollUIIndex(attacker, roll);
            result.targetRollIndex = GetRollUIIndex(target, target_roll);

            if (target_roll != null)
                result.hasTargetRoll = true;

            if (target == attacker)
            {
                if (target_roll != null && target_roll.rollType == RollType.Def)
                {
                    int char_power = roll.GetDamage();
                    int target_defense = target_roll.maxRoll;

                    result.charPower = char_power;
                    result.targetPower = target_defense;
                    result.targetRollValue = target_defense;

                    Damage dmg_obj = new Damage(char_power, roll.element, attacker.GetCharacter());
                    char_power = target.GetCharacter().GetRealDamage(dmg_obj);
                    dmg_obj.damage = char_power;
                    result.damage = dmg_obj;

                    target_roll.maxRoll -= char_power;

                    if (target_roll.maxRoll <= 0)
                    {
                        result.isHit = true;
                        result.isMiss = false;
                        result.shouldRemoveTargetRoll = true;

                        int remaining_damage = char_power - target_defense;
                        roll.maxRoll = remaining_damage;
                        roll.minRoll = remaining_damage;
                    }
                    else
                    {
                        result.isHit = false;
                        result.isMiss = true;
                        result.shouldRemoveTargetRoll = false;
                    }
                }
                else
                {
                    int res = roll.GetDamage();
                    result.damage = new Damage(res, roll.element, attacker.GetCharacter());
                    result.charPower = res;
                    result.targetPower = 0;
                    result.isHit = true;
                    result.isMiss = false;
                    result.shouldRemoveTargetRoll = false;
                }

                result.context.IsHit = result.isHit;
                result.context.IsMiss = result.isMiss;

                results.Add(result);
                continue;
            }

            if (target_roll == null)
            {
                int res = roll.GetDamage();
                result.damage = new Damage(res, roll.element, attacker.GetCharacter());
                result.charPower = res;
                result.targetPower = 0;
                result.isHit = true;
                result.isMiss = false;
                result.shouldRemoveTargetRoll = false;
            }
            else if (target_roll.rollType == RollType.Def)
            {
                int char_power = roll.GetDamage();
                int target_defense = target_roll.maxRoll;

                result.charPower = char_power;
                result.targetPower = target_defense;
                result.targetRollValue = target_defense;

                Damage dmg_obj = new Damage(char_power, roll.element, attacker.GetCharacter());
                char_power = target.GetCharacter().GetRealDamage(dmg_obj);
                dmg_obj.damage = char_power;
                result.damage = dmg_obj;

                target_roll.maxRoll -= char_power;

                if (target_roll.maxRoll <= 0)
                {
                    result.isHit = true;
                    result.isMiss = false;
                    result.shouldRemoveTargetRoll = true;

                    int remaining_damage = char_power - target_defense;
                    roll.maxRoll = remaining_damage;
                    roll.minRoll = remaining_damage;
                }
                else
                {
                    result.isHit = false;
                    result.isMiss = true;
                    result.shouldRemoveTargetRoll = false;
                }
            }
            else
            {
                int target_power = target_roll.GetRoll();

                int levelBonus = GetLevelBonus(attacker.GetCharacter(), target.GetCharacter(), roll);
                int finalAttackerPower = attackerRollValue + levelBonus;

                result.charPower = finalAttackerPower;
                result.targetPower = target_power;

                if (finalAttackerPower >= target_power)
                {
                    result.isHit = true;
                    result.isMiss = false;
                    result.shouldRemoveTargetRoll = true;
                    if (isEnemyAttacker == false)
                    {
                        Swagapoints += 2;
                    }
                }
                else
                {
                    result.isHit = false;
                    result.isMiss = true;
                    result.shouldRemoveTargetRoll = false;
                    if (isEnemyAttacker == true)
                    {
                        Swagapoints += 4;
                    }
                    if (isEnemyAttacker == false)
                    {
                        Swagapoints -= 2;
                    }
                }
            }

            result.context.IsHit = result.isHit;
            result.context.IsMiss = result.isMiss;

            results.Add(result);
        }

        if (hasAnyTargetRoll)
        {
            for (int i = 0; i < results.Count; i++)
            {
                var result = results[i];
                var target = result.target;

                if (!result.hasTargetRoll) continue;

                if (result.isSelfAttack)
                {
                    if (result.targetRoll != null && result.targetRoll.rollType == RollType.Def)
                    {
                        // јтакующий ролл Ч по сохранЄнному индексу
                        GetRollScript(attacker, result.attackerRollIndex)?.ShowResult(result.charPower);
                        if (!result.shouldRemoveTargetRoll)
                        {
                            var enemy = attacker.GetComponent<GridEnemy>();
                            if (enemy != null)
                            {
                                // ÷ель Ч self, но UI у enemy: показываем по targetRollIndex
                                int idx = result.targetRollIndex >= 0 ? result.targetRollIndex : 0;
                                for (int k = 0; k < enemy.RollsUI.childCount; k++)
                                {
                                    enemy.RollsUI.GetChild(k).GetComponent<RollScript>().ShowResult(result.targetRollValue);
                                }
                            }
                        }
                    }
                    else
                    {
                        GetRollScript(attacker, result.attackerRollIndex)?.ShowResult(result.targetRollValue);
                    }
                }
                else if (isEnemyAttacker)
                {
                    if (result.targetRoll == null)
                    {
                        GetRollScript(attacker, result.attackerRollIndex)?.ShowResult(result.targetRollValue);
                    }
                    else if (result.targetRoll.rollType == RollType.Def)
                    {
                        GetRollScript(attacker, result.attackerRollIndex)?.ShowResult(result.targetRollValue);
                    }
                    else
                    {
                        var enemy = attacker.GetComponent<GridEnemy>();
                        int idx = result.attackerRollIndex >= 0 ? result.attackerRollIndex : 0;
                        if (enemy != null && enemy.RollsUI.childCount > idx)
                            enemy.RollsUI.GetChild(idx).GetComponent<RollScript>().ShowResult(result.targetRollValue);
                    }
                }
                else
                {
                    if (result.targetRoll == null)
                    {
                        GetRollScript(attacker, result.attackerRollIndex)?.ShowResult(result.charPower);
                    }
                    else if (result.targetRoll.rollType == RollType.Def)
                    {
                        GetRollScript(attacker, result.attackerRollIndex)?.ShowResult(result.charPower);

                        if (!result.shouldRemoveTargetRoll)
                        {
                            if (target.GType_ == GriddableObject.GriddableObjectType.Enemy)
                            {
                                var enemy = target.GetComponent<GridEnemy>();
                                int idx = result.targetRollIndex >= 0 ? result.targetRollIndex : 0;
                                if (enemy != null && enemy.RollsUI.childCount > idx)
                                {
                                    var rs = enemy.RollsUI.GetChild(idx).GetComponent<RollScript>();
                                    rs.MinMaxText.text = rs.roll.maxRoll.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        var enemy = target.GetComponent<GridEnemy>();
                        int idx = result.targetRollIndex >= 0 ? result.targetRollIndex : 0;
                        if (enemy != null && enemy.RollsUI.childCount > idx)
                            enemy.RollsUI.GetChild(idx).GetComponent<RollScript>().ShowResult(result.targetPower);

                        GetRollScript(attacker, result.attackerRollIndex)?.ShowResult(result.charPower);
                    }
                }
            }

            yield return new WaitForSeconds(0.33f * ProfileManager.profile.roll_speed);

            for (int i = 0; i < results.Count; i++)
            {
                var result = results[i];
                var target = result.target;

                if (!result.hasTargetRoll) continue;

                if (result.isHit && result.shouldRemoveTargetRoll && result.targetRoll != null)
                {
                    if (result.targetRoll.rollType != RollType.Def)
                    {
                        target.RemoveFirstRoll(0.3f);

                        if (target.GType_ == GriddableObject.GriddableObjectType.Enemy)
                        {
                            var enemy = target.GetComponent<GridEnemy>();
                            int idx = result.targetRollIndex >= 0 ? result.targetRollIndex : 0;
                            if (enemy != null && enemy.RollsUI.childCount > idx)
                            {
                                enemy.RollsUI.GetChild(idx).GetComponent<RollScript>().Fade(0f, 0.3f, 0, true);
                            }
                            LeanTween.delayedCall(0.4f, () => { EnemyUpdateRolls(enemy); });
                        }
                    }
                }

                if (result.isHit && result.shouldRemoveTargetRoll && result.targetRoll != null && result.targetRoll.rollType == RollType.Def)
                {
                    target.RemoveFirstRoll(0.3f);

                    if (target.GType_ == GriddableObject.GriddableObjectType.Enemy)
                    {
                        if (target == null) { continue; }
                        var enemy = target.GetComponent<GridEnemy>();
                        int idx = result.targetRollIndex >= 0 ? result.targetRollIndex : 0;
                        if (enemy != null && enemy.RollsUI.childCount > idx)
                        {
                            enemy.RollsUI.GetChild(idx).GetComponent<RollScript>().Fade(0f, 0.3f, 0, true);
                        }
                        LeanTween.delayedCall(0.4f, () => { EnemyUpdateRolls(enemy); });
                    }
                }
            }

            yield return new WaitForSeconds(0.2f * ProfileManager.profile.roll_speed);

            for (int i = 0; i < results.Count; i++)
            {
                var result = results[i];
                var target = result.target;

                if (!result.hasTargetRoll) continue;

                if (attacker.RollsUI != null && attacker.RollsUI.childCount > 0)
                {
                    var attackerRollScript = GetRollScript(attacker, result.attackerRollIndex);
                    if (attackerRollScript != null)
                    {
                        attackerRollScript.is_showing_result = false;
                        attackerRollScript.ShowStats(null);
                    }
                }

                if (result.targetRoll != null && result.isHit == false)
                {
                    if (target.GType_ == GriddableObject.GriddableObjectType.Enemy)
                    {
                        var enemy = target.GetComponent<GridEnemy>();
                        if (enemy != null && enemy.RollsUI != null && enemy.RollsUI.childCount > 0)
                        {
                            var targetRollScript = GetRollScript(target, result.targetRollIndex);
                            if (targetRollScript != null)
                            {
                                targetRollScript.is_showing_result = false;
                                targetRollScript.ShowStats(null);
                            }
                        }
                    }
                    else if (target.GType_ == GriddableObject.GriddableObjectType.Character)
                    {
                        if (target.RollsUI != null && target.RollsUI.childCount > 0)
                        {
                            var targetRollScript = GetRollScript(target, result.targetRollIndex);
                            if (targetRollScript != null)
                            {
                                targetRollScript.is_showing_result = false;
                                targetRollScript.ShowStats(null);
                            }
                        }
                    }
                }
            }

            yield return new WaitForSeconds(0.2f * ProfileManager.profile.roll_speed);
        }

        bool hasAnyHit = false;
        for (int i = 0; i < results.Count; i++)
        {
            var result = results[i];
            var target = result.target;

            if (result.isHit)
            {
                hasAnyHit = true;
                int damageRollValue = roll.GetDamage();
                int levelBonus = GetLevelBonus(attacker.GetCharacter(), target.GetCharacter(), roll);
                result.damageRollValue = damageRollValue + levelBonus;

                var roll_ui = GetRollScript(attacker, result.attackerRollIndex);
                if (roll_ui != null)
                {
                    roll_ui.roll_result = result.damageRollValue;
                    roll_ui.is_showing_result = true;
                }

                result.damage = new Damage(damageRollValue, roll.element, attacker.GetCharacter());
            }
        }

        if (hasAnyHit)
        {
            yield return new WaitForSeconds(0.33f * ProfileManager.profile.roll_speed);
        }

        for (int i = 0; i < results.Count; i++)
        {
            var result = results[i];
            var target = result.target;

            if (result.isHit)
            {
                int finalDamage = result.damage.damage;

                target.GetCharacter().GetDamage(result.damage);
            }
            else if (result.isMiss && result.targetRoll != null && result.targetRoll.rollType != RollType.Def)
            {
                if (!isEnemyAttacker)
                {
                    if (target.GType_ == GriddableObject.GriddableObjectType.Enemy)
                    {
                        var enemy = target.GetComponent<GridEnemy>();
                        int idx = result.targetRollIndex >= 0 ? result.targetRollIndex : 0;
                        if (enemy != null && enemy.RollsUI.childCount > idx)
                            enemy.RollsUI.GetChild(idx).GetComponent<RollScript>().ShowStats(null);
                    }
                }
            }

            roll.ProcessEffects(attacker.GetCharacter(), target.GetCharacter(), result.context);
        }
    }
    private int GetRollUIIndex(GriddableObject obj, Roll roll)
    {
        if (obj == null || roll == null || obj.RollsUI == null) return -1;

        for (int i = 0; i < obj.RollsUI.childCount; i++)
        {
            var rs = obj.RollsUI.GetChild(i).GetComponent<RollScript>();
            if (rs != null && rs.roll == roll)
                return i;
        }
        return -1;
    }

    private RollScript GetRollScript(GriddableObject obj, int index)
    {
        if (obj == null || obj.RollsUI == null) return null;
        if (index < 0 || index >= obj.RollsUI.childCount) return null;
        return obj.RollsUI.GetChild(index).GetComponent<RollScript>();
    }
    public static void GainSwaga(int value)
    {
        Swagapoints += value;
    }
    private class FightResult
    {
        public GriddableObject target;
        public RollContext context;
        public Roll targetRoll;
        public Roll attackerRoll;
        public Damage damage;
        public int charPower;
        public int targetPower;
        public int targetRollValue;
        public int attackerRollValue;
        public int damageRollValue;
        public bool isHit;
        public bool isMiss;
        public bool shouldRemoveTargetRoll;
        public bool isSelfAttack;
        public bool hasTargetRoll;
        public int targetRollIndex; 
        public int attackerRollIndex;
    }

    public IEnumerator MakeFight(GridCharacter character, List<GriddableObject> Targets)
    {
        yield return new WaitForSeconds(1f * ProfileManager.profile.roll_speed);

        for (int i = 0; i < character.CurrentSkillRolls.Count; i += 0)
        {
            var char_roll = character.CurrentSkillRolls[0];
            char_roll.skill.character = character.GetCharacter();

            yield return MakeFightInternal(character, char_roll, Targets, false);

            if (character.CurrentSkillRolls.Count == 1)
            {

            }
            character.CurrentSkillRolls.RemoveAt(0);
            character.GetCharacter().CurrentRolls.RemoveAt(0);
            character.UpdateRollsUI();
        }
    }
    public IEnumerator MakeFightEnemy(GridEnemy enemy, Roll roll, List<GriddableObject> Targets)
    {
        yield return MakeFightInternal(enemy, roll, Targets, true);
        yield return new WaitForSeconds(0.33f * ProfileManager.profile.roll_speed);
    }
    private void ProcessOnUseEffects(PlayerSkill skill, CharacterBase caster, CharacterBase target)
    {
        if (skill == null) return;

        foreach (Roll roll in skill.rolls)
        {
            RollContext context = new RollContext();
            context.Attacker = caster;
            context.Defender = target;
            context.IsHit = false;
            context.IsMiss = false;
            context.IsCrit = false;

            foreach (var effect in roll.effects)
            {
                if (effect.triggerType == TriggerType.OnUse)
                {
                    effect.Execute(caster, target, context);
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
            yield return new WaitForSeconds(1.5f * ProfileManager.profile.roll_speed);
        }
        StartCoroutine(NextCycle());
    }

    public IEnumerator DealDamageByRoll(Roll roll, GriddableObject obj, RollScript roll_ui = null)
    {
        
        Damage damage = new Damage(roll.GetDamage(), roll.element, roll.skill.character);
        if (roll_ui != null) roll_ui.ShowResult(damage.damage);
        yield return new WaitForSeconds(0.5f * ProfileManager.profile.roll_speed);
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
                yield return new WaitForSeconds(2.1f * ProfileManager.profile.roll_speed);

                var target_cells = current_field.GetDamageCellsFromTargetCell(cell, roll, dir);
                foreach (var target_cell in target_cells)
                {
                    target_cell.FlashColor(Color.red, 1f);
                }
                yield return new WaitForSeconds(2.1f * ProfileManager.profile.roll_speed);

                var objects = new List<GriddableObject>();
                foreach (var obj in target_cells)
                {
                    if (obj.object_ != null) objects.Add(obj.object_);
                }
                yield return StartCoroutine(MakeFightEnemy(enemy, roll, objects));
                enemy.RemoveRoll(roll);
                EnemyUpdateRolls(enemy);
            }
            
            
        }
        StartCoroutine(NextCycle());
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
        
        if (CurrentLevel != null) { StaticFuncs.DestroySingle(CurrentLevel.transform); }
        if (name == "None") { StaticFuncs.DestroySingle(current_field.transform);
            IsInBattle = false; ;
        }
        else
        {
            CurrentLevel = Instantiate(ResoursesDict.ObjectSet[name]);
        }
    }

    public void SetBackGroundAccourdingToLevelId(int levelId)
    {
        LevelId level = new LevelId(levelId);
        if (levelId == 0)
        {
            SetLevel("Level0");
            return;
        }
        if (levelId == 8)
        {
            SetLevel("LevelCamp");
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
        if (CurrentLevelId > 10)
        {
            ProfileManager.profile.levels_counter++;
            ProfileManager.profile.swaga_k = (ProfileManager.profile.swaga_k * ProfileManager.profile.levels_counter + Swagapoints) / ProfileManager.profile.levels_counter;
            
        }
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

    public void GiveUp()
    {

    }

    public void SetMenuCamera()
    {
        ui_camera.gameObject.SetActive(true);
        battle_camera.gameObject.SetActive(false);
    }

    public void SetBattleCamera()
    {
        ui_camera.gameObject.SetActive(false);
        battle_camera.gameObject.SetActive(true);
    }

    public void DisassambleScene()
    {
        StaticFuncs.DestroySingle(current_field.transform);
        StaticFuncs.DestroySingle(CurrentLevel.transform);
        turn = 0;
        CurrentLevelId = 0;
    }

    public static float GetSwagaK()
    {
        if (BattleMain.Swagapoints < 50) { return 0.6f; }
        if (BattleMain.Swagapoints < 100) { return 0.8f; }
        if (BattleMain.Swagapoints < 150) { return 1f; }
        if (BattleMain.Swagapoints < 200) { return 0.2f; }
        if (BattleMain.Swagapoints < 250) { return 1.4f; }
        if (BattleMain.Swagapoints < 300) { return 1.6f; }
        return 1.8f;
    }
    public static float GetAverageSwagaK()
    {
        if (ProfileManager.profile.swaga_k < 50) { return 0.6f; }
        if (ProfileManager.profile.swaga_k < 100) { return 0.8f; }
        if (ProfileManager.profile.swaga_k < 150) { return 1f; }
        if (ProfileManager.profile.swaga_k < 200) { return 0.2f; }
        if (ProfileManager.profile.swaga_k < 250) { return 1.4f; }
        if (ProfileManager.profile.swaga_k < 300) { return 1.6f; }
        return 1.8f;
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