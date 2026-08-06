using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingInfoMenu : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    protected GameObject menu;
    private GameObject CurrentMenu;
    [SerializeField]
    protected GameObject ParantObj;
    protected FloatingInfoMenu ParantMenu;
    public bool visible = false;
    [SerializeField]
    protected Vector3 position;
    public static List<GameObject> visible_others = new List<GameObject>();
    void Start()
    {
        if (ParantObj == null) ParantObj = gameObject;
    }


    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!visible) {
                if (menu == null) return;
                var obj = Instantiate(menu, ResoursesDict.ObjectSet["UICanvas"].transform);
                obj.transform.localPosition = position;
                obj.GetComponent<FloatingInfoMenu>().ParantObj = ParantObj;
                obj.GetComponent<FloatingInfoMenu>().ParantMenu = this;
                obj.GetComponent<FloatingInfoMenu>().UpdateInfo();
                CurrentMenu = obj;
                visible = true;
                foreach (GameObject Menu in visible_others)
                {
                    Menu.GetComponent<FloatingInfoMenu>().ParantMenu.visible = false;
                    Destroy(Menu);
                }
                visible_others.Clear();
                visible_others.Add(obj);
            }
            else
            {
                Destroy(CurrentMenu);
                visible = false;
                visible_others.Remove(CurrentMenu);
            }
        }
    }

    public static void CloseAllWindows()
    {
        foreach(var obj in visible_others)
        {
            obj.GetComponent<FloatingInfoMenu>().visible = false;
            Destroy(obj);
        }
        EffectObject.ClearMenus();
        visible_others.Clear();
    }

    public void SetParent(GameObject obj) { ParantObj = obj; }
    virtual public void UpdateInfo() { }
}
