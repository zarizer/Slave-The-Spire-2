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
    Stack<GameObject> MenuStack = new Stack<GameObject>();


    private void Awake()
    {
        CurrentDebugMenu.SetActive(false);        
    }

    void Start()
    {
        if (EnableOnStart) { SetMenu(StartMenu); BackGroundRoom.SetActive(true); }
        LevelData.Init();
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

    public void ReturnMenu()
    {
        if (MenuStack.Count == 0) return;
        ProfileManager.SaveProfile();
        CurrentMenu.SetActive(false);
        CurrentMenu = MenuStack.Pop();
        CurrentMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    public void StartBattle()
    {
        SetMenu(battleMain.gameObject);
        battleMain.CurrentLevelId = prepareBattleScrit.LevelId;
        battleMain.StartGame();
    }
}
