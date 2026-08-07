using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RedactorAddTab : TabController
{
    bool IsVisible = false;
    public int TypeListNum = 0;
    public GameObject TypeCatalogue;
    public GameObject CatalogueBatton;
    public override void RequestedUpdate(bool is_visible)
    {
        IsVisible = is_visible;

        if (IsVisible) gameObject.SetActive(true);
        else { 
            TypeCatalogue.SetActive(false);
            gameObject.SetActive(false); 
        }




    }
    public void CreateCatalogue(int num)
    {
        List<TypePair> list = new List<TypePair>();
        TypeListNum = num;
        TypeCatalogue.SetActive(true);
        if (TypeListNum == 0)
        {
            for (int i = 0; i < DataDicts.EnemyTypes.Count; i++)
            {
                list.Add(new TypePair(DataDicts.EnemyTypes[i].Name, i));
            }
        }
        if (TypeListNum == 1)
        {
            for (int i = 0; i < DataDicts.ObstacleTypes.Count; i++)
            {
                list.Add(new TypePair(DataDicts.ObstacleTypes[i].Name, i));
            }
        }
        if (TypeListNum == 2)
        {
            for (int i = 0; i < DataDicts.CharacterTypes.Count; i++)
            {
                list.Add(new TypePair(DataDicts.CharacterTypes[i].Name, i));
            }
        }

        while (TypeCatalogue.transform.GetChild(0).GetChild(0).childCount > 1)
        {
            StaticFuncs.DestroySingle(TypeCatalogue.transform.GetChild(0).GetChild(0).GetChild(1));
        }

        foreach (TypePair pair in list)
        {
            var obj = Instantiate(CatalogueBatton, TypeCatalogue.transform.GetChild(0).GetChild(0));
            obj.transform.localPosition = new Vector3(190, -40 - (60*pair.index) ,0);
            obj.name = pair.index.ToString();
            obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = pair.name;
        }
    }

    public void CreateObject(GameObject button)
    {
        var camera = ResoursesDict.GetClass<CameraController>();
        var field = camera.field_;
        LevelObject obj = new LevelObject();
        LevelData data = new LevelData();
        if (camera.Target.GetComponent<GridCell>().object_ != null) 
        {
            ResoursesDict.GetClass<CameraController>().field_.RemoveObject(camera.Target.GetComponent<GridCell>().object_, true);
        }

        if (TypeListNum == 0)
        {
            field.CreateGridObject(GriddableObject.GriddableObjectType.Enemy, int.Parse(button.name),
                camera.Target.GetComponent<GridCell>().x_, camera.Target.GetComponent<GridCell>().y_, obj, data, false);
        }
        if (TypeListNum == 1)
        {
            field.CreateGridObject(GriddableObject.GriddableObjectType.Obstacle, int.Parse(button.name),
                camera.Target.GetComponent<GridCell>().x_, camera.Target.GetComponent<GridCell>().y_, obj, data, false);
        }
        if (TypeListNum == 2)
        {
            field.CreateGridObject(GriddableObject.GriddableObjectType.Character, int.Parse(button.name),
                camera.Target.GetComponent<GridCell>().x_, camera.Target.GetComponent<GridCell>().y_, obj, data, false);
        }

    }

    struct TypePair
    {
        public string name;
        public int index;

        public TypePair(string name_, int index_)
        {
            name = name_;
            index = index_;
        }
    };
}
