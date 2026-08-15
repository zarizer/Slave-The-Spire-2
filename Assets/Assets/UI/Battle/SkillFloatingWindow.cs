using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SkillFloatingWindow : MonoBehaviour, IPointerClickHandler
{
    public List<GameObject> roll_windows = new List<GameObject>();
    public Transform RollsUI;
    public GameObject RollMenu;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(gameObject.name + transform.parent.name);
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            StaticFuncs.DestroyChildren(RollsUI);
            roll_windows.Clear();
            var character = Camera.main.transform.parent.gameObject.GetComponent<CameraController>().Target.gameObject.GetComponent<GridCharacter>().character_;
            PlayerSkill skill = new PlayerSkill();

            switch(GetComponent<UIElement>().UIType)
            {
                case("attack1") : skill = character.Skill1; break;
                case ("attack2") : skill = character.Skill2; break;
                case ("attack3") : skill = character.Skill3; break;
                case ("attack4") : skill = character.Skill4; break;
            }

            for (int i = 0; i < skill.rolls.Count; i++)
            {
                var obj = Instantiate(RollMenu, RollsUI);
                var position = new Vector3(1760, 620 - i * 200, 0);
                var menu = obj.GetComponent<FloatingRollMenu>();
                menu.SetParent(gameObject);
                menu.transform.position = position;
                menu.roll = skill.rolls[i];
                menu.UpdateInfo();
            }
        }
    }

    public void CloseAllWindows()
    {
        StaticFuncs.DestroyChildren(RollsUI);
        EffectObject.ClearMenus();
        roll_windows.Clear();
    }
}
