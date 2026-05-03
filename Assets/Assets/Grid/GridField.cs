using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridField : MonoBehaviour
{
    public List<List<GridCell>> Cells_ = new List<List<GridCell>>();
    public GameObject CellObject;
    public GameObject RockObject;
    public GameObject DebugCharacter;
    public int DebugCharacterId;
    public int DebugCharacterPosX;
    public int DebugCharacterPosY;

    public GameObject DebugEnemy;
    public int DebugEnemyId;
    public int DebugEnemyPosX;
    public int DebugEnemyPosY;

    public CameraController Camera;

    public List<GriddableObject> GridObjects;

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
        Camera.prev_cell = Cells_[SizeX_ - 1][SizeY_-1].GetComponent<GridCell>();
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
        if (GetGridObject(DebugCharacterPosX, DebugCharacterPosY) == null)
        {
            var character = Instantiate(DebugCharacter, transform);
            character.GetComponent<GridCharacter>().ReplaceCharacter(DebugCharacterId);
            AddGridObject(DebugCharacterPosX, DebugCharacterPosY, character);
            GetGridCell(DebugCharacterPosX, DebugCharacterPosY).SnapObject();
        }
    }

    [ContextMenu("CreateDebugEnemy")]
    public void CreateDebugEnemy()
    {
        if (GetGridObject(DebugEnemyPosX, DebugEnemyPosY) == null)
        {
            var character = Instantiate(DebugEnemy, transform);
            character.GetComponent<GridEnemy>().ReplaceEnemy(DebugEnemyId);
            AddGridObject(DebugEnemyPosX, DebugEnemyPosY, character);
            GetGridCell(DebugEnemyPosX, DebugEnemyPosY).SnapObject();
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
                up.ParentCell = cell;
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
                down.ParentCell = cell;
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
                left.ParentCell = cell;
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
                right.ParentCell = cell;
                right.visited = true;
                queue.Enqueue(right);
            }
        }
        
    }

    public void FindAttacksPlayer(int x, int y, PlayerSkill skill)
    {
        if (skill.energy > Camera.Target.gameObject.GetComponent<GridCharacter>().character_.energy) return;
        if (skill.rollDist == RollDist.Any)
        {
            for (int i = 0; i<SizeX_; i++)
            {
                for (int j = 0; j<SizeY_; j++)
                {
                    GetGridCell(i, j).ColorCell(GridCell.ColorType.Yellow);
                }
            }
        }
        foreach (var pos in skill.AttackPositions)
        {
            if (GetGridCell(x + pos.Item1, y + pos.Item2) != null)
            {
                GetGridCell(x + pos.Item1, y + pos.Item2).ColorCell(GridCell.ColorType.Yellow);
            }
        }
    }
    public void ShowDamagePlayer(int x, int y, PlayerSkill skill, int px = 0, int py = 0)
    {
        if (skill.energy > Camera.Target.gameObject.GetComponent<GridCharacter>().character_.energy) return;
        (int, int) p_pos = (Camera.Target.gameObject.GetComponent<GridCharacter>().cell_.x_,
                            Camera.Target.gameObject.GetComponent<GridCharacter>().cell_.y_);
        List<(int, int)> damage_cells = new List<(int, int)>();

        Direction dir = Direction.up;
        if (p_pos.Item1 == x && p_pos.Item2 > y) dir = Direction.down;
        else if (p_pos.Item1 == x && p_pos.Item2 < y) dir = Direction.up;
        else if (p_pos.Item1 > x && p_pos.Item2 == y) dir = Direction.left;
        else if (p_pos.Item1 < x && p_pos.Item2 == y) dir = Direction.right;
        else if (p_pos.Item1 > x && p_pos.Item2 > y) dir = Direction.dg_l_d;
        else if (p_pos.Item1 > x && p_pos.Item2 < y) dir = Direction.dg_l_u;
        else if (p_pos.Item1 < x && p_pos.Item2 > y) dir = Direction.dg_r_d;
        else if (p_pos.Item1 <= x && p_pos.Item2 <= y) dir = Direction.dg_r_u;


        foreach (var roll in skill.rolls)
        {
            foreach (var pos in roll.DamagePositions)
            {
                Debug.Log(pos);
            }
            Debug.Log(dir);
            foreach(var cell in GetDamageCellsByRoll(x, y, roll, dir))
            {
                if (!damage_cells.Contains(cell)) damage_cells.Add(cell);
            }
        }
        CellsRestoreColor();
        foreach (var cell_pos in damage_cells)
        {
            if (skill.rolls[0].rollRadius != RollRadius.PlayerRadius &&
                skill.rolls[0].rollRadius != RollRadius.DgLine &&
                skill.rolls[0].rollRadius != RollRadius.StLine)
            {
                if (GetGridCell(cell_pos.Item1 + x, cell_pos.Item2 + y) != null)
                {
                    var cell = GetGridCell(cell_pos.Item1 + x, cell_pos.Item2 + y);
                    cell.IsTargeted = true;
                }
            }
            else
            {
                Debug.Log(GetGridCell(cell_pos.Item1 + px, cell_pos.Item2 + py) + "!!!");
                if (GetGridCell(cell_pos.Item1 + px, cell_pos.Item2 + py) != null)
                {
                    var cell = GetGridCell(cell_pos.Item1 + px, cell_pos.Item2 + py);
                    cell.IsTargeted = true;
                }
            }
        }

        
        UpdateDamageVisibilityCells();
        
    }

    List<(int, int)> GetDamageCellsByRoll(int x, int y, Roll roll, Direction dir)
    {
        List<(int, int)> cells = new List<(int, int)> ();

        if (roll.rollRadius == RollRadius.Field)
        {
            for (int i = 0; i < SizeX_; i++)
            {
                for (int j = 0; j < SizeY_; j++)
                {
                    cells.Add((i, j));
                }
            }
        }
        else if (roll.rollRadius == RollRadius.Single)
        {
            cells.Add((0,0));
        }
        else if (roll.rollRadius == RollRadius.PlayerRadius || roll.rollRadius == RollRadius.TargetRadius)
        {
            foreach (var pos in roll.DamagePositions)
            {
                cells.Add(pos);
            }
        }
        else if (roll.rollRadius == RollRadius.DgLine)
        {
            if (dir == Direction.dg_r_u)
            {
                foreach (var pos in roll.DamagePositions)
                {
                    if (pos.Item1 > 0 && pos.Item2 > 0) cells.Add(pos);
                }
            }
            if (dir == Direction.dg_r_d)
            {
                foreach (var pos in roll.DamagePositions)
                {
                    if (pos.Item1 > 0 && pos.Item2 < 0) cells.Add(pos);
                }
            }
            if (dir == Direction.dg_l_u)
            {
                foreach (var pos in roll.DamagePositions)
                {
                    if (pos.Item1 < 0 && pos.Item2 > 0) cells.Add(pos);
                }
            }
            if (dir == Direction.dg_l_d)
            {
                foreach (var pos in roll.DamagePositions)
                {
                    if (pos.Item1 < 0 && pos.Item2 < 0) cells.Add(pos);
                }
            }

        }
        else if (roll.rollRadius == RollRadius.StLine)
        {
            if (dir == Direction.up)
            {
                foreach (var pos in roll.DamagePositions)
                {
                    if (pos.Item2 > 0) cells.Add(pos);
                }
            }
            if (dir == Direction.down)
            {
                foreach (var pos in roll.DamagePositions)
                {
                    if (pos.Item2 < 0) cells.Add(pos);
                }
            }
            if (dir == Direction.right)
            {
                foreach (var pos in roll.DamagePositions)
                {
                    if (pos.Item1 > 0) cells.Add(pos);
                }
            }
            if (dir == Direction.left)
            {
                foreach (var pos in roll.DamagePositions)
                {
                    if (pos.Item1 < 0) cells.Add(pos);
                }
            }
        }
            
        return cells;
    }


    [ContextMenu("DeColor")]
    public void DeColor()
    {
        foreach (var row in Cells_)
        {
            foreach (var cell in row)
            {
                cell.PrevColor = GridCell.ColorType.None;
                cell.GetComponent<GridCell>().ColorCell(GridCell.ColorType.None);
                cell.IsTargeted = false;
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
        GridObjects.Add(obj);
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
                cell.GetComponent<GridCell>().ParentCell = null;
            }
        }
    }

    public void GridObjectsActionNullify()
    {
        foreach(var obj in GridObjects)
        {
            obj.TryingToMove = false;
        }
    }
    public void CellsNullify()
    {
        DeColor();
        DeVisitCells();
    }

    public void UpdateDamageVisibilityCells()
    {
        foreach (var row in Cells_)
        {
            foreach (var cell in row)
            {
                cell.GetComponent<GridCell>().UpdateTargeting();
            }
        }
    }

    public void CellsRestoreColor()
    {
        foreach (var row in Cells_)
        {
            foreach (var cell in row)
            {
                if (cell.GetComponent<GridCell>().IsTargeted)
                {
                    cell.GetComponent<GridCell>().ColorCell(cell.GetComponent<GridCell>().PrevColor);
                    cell.GetComponent<GridCell>().IsTargeted = false;
                }
            }
        }
    }
}

enum Direction
{
    up,
    down,
    left,
    right,
    dg_r_u,
    dg_r_d,
    dg_l_u,
    dg_l_d,
}