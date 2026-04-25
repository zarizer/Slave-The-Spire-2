using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class GridField : MonoBehaviour
{
    public List<List<GridCell>> Cells_ = new List<List<GridCell>>();
    public GameObject CellObject;
    public GameObject RockObject;

    [SerializeField] private int SizeX_;
    [SerializeField] private int SizeY_;
    void Start()
    {
        List<List<GridCell>> Cells_ = new List<List<GridCell>>();
        CreateField();
    }

    void Update()
    {
        
    }

    [ContextMenu("CreateField")]
    public void CreateField()
    {
        for(int i = Cells_.Count - 1; i>=0;i--)
        {
            for (int j = Cells_[i].Count - 1; j >= 0; j--)
            {
                DestroyImmediate(Cells_[i][j].gameObject);
            }
        }
        for (int j = transform.childCount - 1; j >= 0; j--)
        {
            DestroyImmediate(transform.GetChild(j).gameObject);
        }
        Cells_ = new List<List<GridCell>>();
        for (int x = 0; x < SizeX_; x++)
        {
            List<GridCell> list = new List<GridCell>();
            for (int y = 0; y < SizeY_; y++)
            {
                var obj = Instantiate(CellObject, transform);
                obj.transform.localPosition = new Vector3(x, 0.5f, y);
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<GridCell>().x_ = x;
                obj.GetComponent<GridCell>().y_ = y;
                list.Add(obj.GetComponent<GridCell>());
            }
            Cells_.Add(list);
        }
    }

    [ContextMenu("CreateRocks")]
    public void CreateRocks()
    {
        for (int i = 0; i<4; i++)
        {
            int rx = Random.Range(0, SizeX_);
            int ry = Random.Range(0, SizeY_);

            if (GetGridObject(rx, ry) == null)
            {
                AddGridObject(rx, ry, Instantiate(RockObject));
            }
        }
    }



    GriddableObject GetGridObject(int x, int y)
    {
        if (x>SizeX_-1 || y>SizeY_-1 || x<0 || y<0) return null;
        GriddableObject obj = Cells_[x][y].object_;
        if (obj == null) return null;
        return obj;
    }

    void AddGridObject(int x, int y, GriddableObject obj)
    {
        if (x > SizeX_ - 1 || y > SizeY_ - 1)
        {
            Debug.Log("Adding out of grid: " + x + " " + y);
            return;
        }
        if (GetGridObject(x, y) != null) 
        {
            Debug.Log("Adding object on top of anather object: " + x + " " + y);
            return;
        }
        Cells_[x][y].object_ = obj;
        Cells_[x][y].SnapObject();
    }

    void AddGridObject(int x, int y, GameObject obj)
    {
        if (obj.GetComponent<GriddableObject>() == null) return;
        if (x > SizeX_ - 1 || y > SizeY_ - 1)
        {
            Debug.Log("Adding out of grid: " + x + " " + y);
            return;
        }
        if (GetGridObject(x, y) != null)
        {
            Debug.Log("Adding object on top of anather object: " + x + " " + y);
            return;
        }
        Cells_[x][y].object_ = obj.GetComponent<GriddableObject>();
        Cells_[x][y].SnapObject();
    }
}

