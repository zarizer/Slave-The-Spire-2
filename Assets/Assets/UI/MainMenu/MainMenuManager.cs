using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public PrepareBattleScript prepareBattleScrit;
    public BattleMain battleMain;
    public GameObject CurrentDebugMenu;
    public GameObject StartMenu;
    public GameObject BackGroundRoom;
    public bool EnableOnStart = false;

    public GameObject CurrentMenu;
    [SerializeField] Stack<GameObject> MenuStack = new Stack<GameObject>();


    private void Awake()
    {
        Skills.Init();
        
        CurrentDebugMenu.SetActive(false);        
    }

    void Start()
    {
        if (EnableOnStart) { SetMenu(StartMenu); BackGroundRoom.SetActive(true); }
        LevelData.Init();
        ResoursesDict.GetClass<InventoryManager>().Init();
        InventoryManager.LoadInventory();
    }

    void Update()
    {
        
    }


    public void SetMenu(GameObject menu)
    {
        MenuStack.Push(CurrentMenu);
        if (CurrentMenu != null) CurrentMenu.SetActive(false);
        CurrentMenu = menu;
        CurrentMenu.SetActive(true);
    }

    public void SetMenuAndClearStack(GameObject menu)
    {
        MenuStack.Clear();
        MenuStack.Push(CurrentMenu);
        if (CurrentMenu != null) CurrentMenu.SetActive(false);
        CurrentMenu = menu;
        CurrentMenu.SetActive(true);
    }

    public void ClearStack()
    {
        MenuStack.Clear();
    }


    public void ReturnMenu()
    {
        if (MenuStack.Count == 0) return;
        ProfileManager.SaveProfile();
        CurrentMenu.SetActive(false);
        CurrentMenu = MenuStack.Pop();
        CurrentMenu.SetActive(true);
    }

    public void CloseCurrentMenu()
    {
        CurrentMenu.SetActive(false);
        MenuStack.Pop();
    }

    public void ToMainMenu()
    {
        CurrentMenu.SetActive(false);
        SetMenu(StartMenu);
        MenuStack.Clear();
        ResoursesDict.GetClass<BattleMain>().SetMenuCamera();
        ResoursesDict.GetClass<BattleMain>().DisassambleScene();
        battleMain.IsInBattle = false;

    }

    public void ExitGame()
    {
        Application.Quit();
    }


    public void StartBattle()
    {
        if (CharacterMenuController.selected_characters.Count <= 0) { ResoursesDict.GetClass<SoundMain>().Restrict(); return; }
        ResoursesDict.GetClass<SoundMain>().Accept();
        SetMenu(battleMain.gameObject);
        battleMain.CurrentLevelId = prepareBattleScrit.LevelId;
        battleMain.StartGame();
    }

    public void GiveUp()
    {
        ToMainMenu();
        ResoursesDict.GetClass<BattleMain>().GiveUp();
    }

    public void ToSettingsMenuFromBattle()
    {
        SetMenu(ResoursesDict.GetClass<UIController>().SettingsMenu);
    }
}
