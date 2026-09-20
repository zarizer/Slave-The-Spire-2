using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static System.Math;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI IsVictory;
    [SerializeField] TextMeshProUGUI LevelsCounter;
    [SerializeField] TextMeshProUGUI AverageSwaga;
    [SerializeField] TextMeshProUGUI Budgets;
    [SerializeField] GameObject EndButton;
    [SerializeField] GameObject MainMenu;

    bool waiting_for_budgets = false;
    int budgets_int = 0;
    int final_budgets = 0;

    Coroutine StatsCoroutine; 
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (waiting_for_budgets)
        {
            budgets_int = (int)(Mathf.Lerp(budgets_int, final_budgets, 0.1f) + 1);
            Budgets.text = budgets_int.ToString();
            if (budgets_int >= final_budgets)
            {
                budgets_int = final_budgets;
                Budgets.text = final_budgets.ToString();
            }
        }

    }

    public void CreateStats(bool is_victory, int budgets)
    {
        StatsCoroutine = StartCoroutine(CreateStatsInternal(is_victory, budgets));
        
    }
    IEnumerator CreateStatsInternal(bool is_victory, int budgets)
    {
        final_budgets = budgets;
        yield return new WaitForSeconds(0.5f);
        IsVictory.gameObject.SetActive(true);
        if (is_victory)
        {
            IsVictory.text = "да";
        }
        else
        {
            IsVictory.text = "увы";
        }
        yield return new WaitForSeconds(0.75f);
        LevelsCounter.gameObject.SetActive(true);
        LevelsCounter.text = ProfileManager.profile.levels_counter.ToString();
        yield return new WaitForSeconds(0.75f);
        AverageSwaga.gameObject.SetActive(true);
        AverageSwaga.text = BattleMain.GetSwagaK().ToString();
        yield return new WaitForSeconds(0.75f);
        Budgets.gameObject.SetActive(true);
        Budgets.text = 0.ToString();
        waiting_for_budgets = true;
        while (budgets_int != final_budgets) { yield return null; }
        waiting_for_budgets = false;
        budgets_int = 0;
        

        yield return new WaitForEndOfFrame();
        EndButton.gameObject.SetActive(true);
    }

    public void OnEndButton()
    {
        EndButton.gameObject.SetActive(false);
        LevelsCounter.gameObject.SetActive(false);
        Budgets.gameObject.SetActive(false);
        IsVictory.gameObject.SetActive(false);
        AverageSwaga.gameObject.SetActive(false);
        ResoursesDict.GetClass<MainMenuManager>().SetMenuAndClearStack(MainMenu);
    }

}
