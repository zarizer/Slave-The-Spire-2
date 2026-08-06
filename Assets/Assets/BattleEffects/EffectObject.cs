using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EffectObject : MonoBehaviour, IPointerClickHandler
{
    public RawImage image;
    public TextMeshProUGUI power;
    public TextMeshProUGUI duration;
    public BattleEffect effect;
    public GameObject MenuPrefab;
    public GameObject CurMenu;
    public static List<GameObject> menus = new List<GameObject>();
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateData()
    {
        image.texture = effect.image;
        power.text = effect.power.ToString();
        duration.text = effect.duration.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CurMenu == null || menus.Count == 0)
        {
            if (menus.Count == 0) CurMenu = null;
            var menu = Instantiate(MenuPrefab, ResoursesDict.ObjectSet["UICanvas"].transform);
            menu.GetComponent<FloatingEffectMenu>().UpdateInfo(effect, true);
            CurMenu = menu;
            menus.Add(menu);
        }
        else
        {
            ClearMenus();
            CurMenu = null;
        }
    }

    public static void ClearMenus()
    {
        foreach (var menu in menus)
        {
            StaticFuncs.DestroySingle(menu.gameObject.transform);
        }
        menus.Clear();
    } 
}
