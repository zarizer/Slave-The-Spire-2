using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class GridField : MonoBehaviour
{
    public List<List<GridCell>> Cells_ = new List<List<GridCell>>();
    public GameObject CellObject;
    public GameObject RockObject;
    public GameObject DebugCharacter;
    public int DebugCharacterId;
    public int DebugCharacterPosX;
    public int DebugCharacterPosY;

    public CameraController Camera;

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
                obj.GetComponent<GridCell>().field_ = this;
                list.Add(obj.GetComponent<GridCell>());
            }
            Cells_.Add(list);
        }
    }

    [ContextMenu("CreateRocks")]
    public void CreateRocks()
    {
        for (int i = 0; i<10; i++)
        {
            int rx = Random.Range(0, SizeX_);
            int ry = Random.Range(0, SizeY_);

            if (GetGridObject(rx, ry) == null)
            {
                AddGridObject(rx, ry, Instantiate(RockObject, transform));
            }
        }
    }
    [ContextMenu("CreateDebugCharacter")]
    public void CreateDebugCharacter()
    {

        if (GetGridObject(5, 5) == null)
        {
            var character = Instantiate(DebugCharacter, transform);
            character.GetComponent<GridCharacter>().ReplaceCharacter(DebugCharacterId);
            AddGridObject(DebugCharacterPosX, DebugCharacterPosY, character);
        }

    }



    GriddableObject GetGridObject(int x, int y)
    {
        if (x < 0 || y < 0 || x >= SizeX_ || y >= SizeY_)
        {
            Debug.Log("Getting cell out of field at: " + x + " " + y);
            return null;
        }
        GriddableObject obj = Cells_[x][y].object_;
        if (obj == null) return null;
        return obj;
    }

    GridCell GetGridCell(int x, int y)
    {
        if (x < 0 || y < 0 || x >= SizeX_ || y >= SizeY_)
        {
            Debug.Log("Getting cell out of field at: " + x + " " + y);
            return null;
        }
        return Cells_[x][y];
    }

    public void FindWaysPlayer(int x, int y, int moves) 
    {

        List<GridCell> cells = new List<GridCell>();
        List<GridCell> nonstopcells = new List<GridCell>();
        CheckWaysPlayer(x, y, moves, cells, nonstopcells);
        foreach (GridCell cell in cells)
        {
            cell.ColorCell(GridCell.ColorType.Green);
        }
        foreach (GridCell cell in nonstopcells)
        {
            cell.ColorCell(GridCell.ColorType.Yellow);
        }
    }

    void CheckWaysPlayer(int x, int y, int moves, List<GridCell> cells, List<GridCell> nonstopcells)
    {
        Queue<GridCell> queue = new Queue<GridCell>();
        queue.Enqueue(GetGridCell(x, y));
        while (queue.Count>0)
        {
            GridCell cell = queue.Dequeue();
            var up = GetGridCell(cell.x_, cell.y_ + 1);
            if (up != null) up.moves = cell.moves + 1;
            var down = GetGridCell(cell.x_, cell.y_ - 1);
            if (down != null) down.moves = cell.moves + 1;
            var left = GetGridCell(cell.x_ - 1, cell.y_);
            if (left != null) left.moves = cell.moves + 1;
            var right = GetGridCell(cell.x_ + 1, cell.y_);
            if (right != null) right.moves = cell.moves + 1;
            if (up != null && !up.visited && up.IsMovable() && up.moves < moves)
            {
                if (up.IsStoppable())
                {
                    cells.Add(up);
                }
                else {
                    nonstopcells.Add(up);
                }
                up.visited = true;
                queue.Enqueue(up);
            }
            if (down != null && !down.visited && down.IsMovable() && down.moves < moves)
            {
                if (down.IsStoppable())
                {
                    cells.Add(down);
                }
                else
                {
                    nonstopcells.Add(down);
                }
                down.visited = true;
                queue.Enqueue(down);
            }
            if (left != null && !left.visited && left.IsMovable() && left.moves < moves)
            {
                if (left.IsStoppable())
                {
                    cells.Add(left);
                }
                else
                {
                    nonstopcells.Add(left);
                }
                left.visited = true;
                queue.Enqueue(left);
            }
            if (right != null && !right.visited && right.IsMovable() && right.moves < moves)
            {
                if (right.IsStoppable())
                {
                    cells.Add(right);
                }
                else
                {
                    nonstopcells.Add(right);
                }
                right.visited = true;
                queue.Enqueue(right);
            }
        }
        
    }

    [ContextMenu("DeColor")]
    public void DeColor()
    {
        foreach (var row in Cells_)
        {
            foreach (var cell in row)
            {
                cell.GetComponent<GridCell>().ColorCell(GridCell.ColorType.None);
            }
        }
    }

    [ContextMenu("DebugFindWays")]
    public void DebugFindWays()
    {
        FindWaysPlayer(5, 5, 5);
        DeVisitCells();
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
        obj.cell_ = GetGridCell(x, y);
        obj.field_ = this;
        Cells_[x][y].object_ = obj;
        Cells_[x][y].SnapObject();
    }

    void AddGridObject(int x, int y, GameObject obj)
    {
        AddGridObject(x, y, obj.GetComponent<GriddableObject>());
    }

    public void DeVisitCells()
    {
        foreach (var row in Cells_)
        {
            foreach (var cell in row)
            {
                cell.GetComponent<GridCell>().visited = false;
                cell.GetComponent<GridCell>().moves = 0;
            }
        }
    }

    public void CellsNullify()
    {
        DeColor();
        DeVisitCells();
    }
}

