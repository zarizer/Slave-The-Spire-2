using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class UIButtonPull : UIButton
{
    public int BudgetThreshold;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (BudgetThreshold < ProfileManager.profile.budget)
        {
            SoundMain.instance.Accept();
        }
        else
        {
            SoundMain.instance.Restrict();
        }
    }
}
