using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingEffectMenu : MonoBehaviour
{
    public BattleEffect effect;
    public RawImage EffectImage;

    public TextMeshProUGUI EffectPower;
    public TextMeshProUGUI EffectDuartion;
    public TextMeshProUGUI Description;
    public int TextSize;

    public void Start()
    {
    }
    public void Update()
    {

    }
    public void UpdateInfo(BattleEffect effect, bool visible)
    {
        if (visible) gameObject.SetActive(true);
        else { gameObject.SetActive(false); return; }
        EffectPower.text = effect.power.ToString();
        EffectDuartion.text = effect.duration.ToString();
        EffectImage.texture = effect.image;
        Description.text = effect.description;
        
    }
}
