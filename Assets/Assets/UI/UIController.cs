using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public CharacterTabController character_tab_controller;
    public EnemyTabController enemy_tab_controller;

    public Transform EnemyRollsUI;

    public List<TabController> ObjectTabs;
    void Start()
    {
        ObjectTabs.Add(character_tab_controller);
        ObjectTabs.Add(enemy_tab_controller);
    }

    void Update()
    {
        
    }

    public void CloseTabs()
    {
        character_tab_controller.RequestedUpdate(false);
        enemy_tab_controller.RequestedUpdate(false);
    }

    public void UpdateTabEnemy(bool is_visible, EnemyBase enemy = null)
    {
        CloseAllTabs(ObjectTabs);
        enemy_tab_controller.SetCurrentEnemy(enemy);
        enemy_tab_controller.RequestedUpdate(is_visible);
        
    }
    public void UpdateTabCharacter(bool is_visible)
    {
        CloseAllTabs(ObjectTabs);
        character_tab_controller.RequestedUpdate(is_visible);
    }

    void CloseAllTabs(List<TabController> tabs)
    {
        foreach (TabController tab in tabs)
        {
            tab.RequestedUpdate(false);
        }
    }

    public void CloseAllObjectTabs()
    {
        foreach (TabController tab in ObjectTabs)
        {
            tab.RequestedUpdate(false);
        }
    }

    public void EnemyUIRollsPrint(List<Roll> rolls)
    {

    }

}
