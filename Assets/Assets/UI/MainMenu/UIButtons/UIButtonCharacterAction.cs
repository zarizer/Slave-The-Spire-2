using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class UIButtonCharacterAction : UIButton
{
    CharacterBase character;
    [SerializeField] string Count;
    [SerializeField] string energy;
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (SoundsType == SoundType.Move)
        {
            if (character.GetGridCharacer().CanMove()) { SoundMain.instance.Accept(); }
            else { SoundMain.instance.Restrict(); }
        }
        else if (SoundsType == SoundType.Attack)
        {
            //if (character.GetGridCharacer().CanMove()) { SoundMain.instance.Accept(); }
            //else { SoundMain.instance.Restrict(); }
        }
    }
}
