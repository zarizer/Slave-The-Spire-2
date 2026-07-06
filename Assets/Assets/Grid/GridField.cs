using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public GameObject DebugEnemy;
    public int DebugEnemyId;
    public int DebugEnemyPosX;
    public int DebugEnemyPosY;

    public int DebugLevelSaveId;
    public int DebugMinLevel;
    public int DebugMaxLevel;

    public int DebugObstacleId;
    public int DebugObstaclePosX;
    public int DebugObstaclePosY;

    public CameraController Camera;

    public List<GriddableObject> GridObjects;

    [SerializeField] private int SizeX_;
    [SerializeField] private int SizeY_;

    public List<GridEnemy> GridEnemies;
    public List<GridCharacter> GridCharacters;
    public List<GridObstacle> GridObstacles;

    public GridCharacter BaseCharacter;
    public GridEnemy BaseEnemy;
    public GridObstacle BaseObstacle;
    void Start()
    {

    }

    void Update()
    {

    }

    public void StartField(BattleMain battle_data)
    {
        List<List<GridCell>> Cells_ = new List<List<GridCell>>();
        CreateField();
        CreateDebugEnemy();
        CreateDebugCharacter();
    }

    void GetLevelData(int levelId)
    {

    }

    [ContextMenu("SaveLevelData")] 
    public void SaveLevelData()
    {
        LevelData data = new LevelData();

        data.x_ = SizeX_;
        data.y_ = SizeY_;
        data.DebugSaveId = DebugLevelSaveId;
        data.minLevel = DebugMinLevel;
        data.maxLevel = DebugMaxLevel;

        foreach (GridEnemy enemy in GridEnemies)
        {
            LevelObject obj = new LevelObject();
            obj.type = "Enemy";
            obj.id = enemy.GetCharacter().id;
            if (enemy.IsCustomLevel)
            {
                obj.isCustomLevel = true;
                obj.level = enemy.CustomLevel;
            }
            data.Objects[(enemy.cell_.x_, enemy.cell_.y_)] = obj;
        }
        foreach (GridCharacter character in GridCharacters)
        {
            LevelObject obj = new LevelObject();
            obj.type = "Character";
            obj.id = character.GetCharacter().id;
            if (character.IsCustomLevel)
            {
                obj.isCustomLevel = true;
                obj.level = character.CustomLevel;
            }
            data.Objects[(character.cell_.x_, character.cell_.y_)] = obj;
        }
        foreach (GridObstacle obstacle in GridObstacles)
        {
            LevelObject obj = new LevelObject();
            obj.type = "Obstacle";
            obj.id = obstacle.GetCharacter().id;
            if (obstacle.IsCustomLevel)
            {
                obj.isCustomLevel = true;
                obj.level = obstacle.CustomLevel;
            }
            data.Objects[(obstacle.cell_.x_, obstacle.cell_.y_)] = obj;
        }
        LevelData.SaveLevel(data);
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
            int rx = UnityEngine.Random.Range(0, SizeX_);
            int ry = UnityEngine.Random.Range(0, SizeY_);

            if (GetGridObject(rx, ry) == null)
            {
                CreateGridObject(GriddableObject.GriddableObjectType.Obstacle, 0, rx, ry);
            }
        }
    }

    [ContextMenu("CreateDebugCharacter")]
    public void CreateDebugCharacter()
    {
        CreateGridObject(GriddableObject.GriddableObjectType.Character, DebugCharacterId, DebugCharacterPosX, DebugCharacterPosY);
    }

    [ContextMenu("CreateDebugEnemy")]
    public void CreateDebugEnemy()
    {
        CreateGridObject(GriddableObject.GriddableObjectType.Enemy, DebugEnemyId, DebugEnemyPosX, DebugEnemyPosY);
    }

    [ContextMenu("CreateDebugObstacle")]
    public void CreateDebugObstacle()
    {
        CreateGridObject(GriddableObject.GriddableObjectType.Obstacle, DebugObstacleId, DebugObstaclePosX, DebugObstaclePosY);
    }

    public GriddableObject CreateGridObject(GriddableObject.GriddableObjectType type, int ID, int X, int Y)
    {
        if (GetGridCell(X, Y) == null)
        {
            Debug.Log("ERROR: CREATING OBJECT IN INVALID POSITION");
            return null;
        }
        GriddableObject cur_object = null;
        if (type == GriddableObject.GriddableObjectType.Character)
        {
            var obj = Instantiate(BaseCharacter, transform);
            cur_object = obj.GetComponent<GriddableObject>();
            GridCharacters.Add(cur_object.GetComponent<GridCharacter>());
        }
        else if (type == GriddableObject.GriddableObjectType.Enemy)
        {
            var obj = Instantiate(BaseEnemy, transform);
            cur_object = obj.GetComponent<GriddableObject>();
            GridEnemies.Add(cur_object.GetComponent<GridEnemy>());
        }
        else if (type == GriddableObject.GriddableObjectType.Obstacle)
        {
            var obj = Instantiate(BaseObstacle, transform);
            cur_object = obj.GetComponent<GriddableObject>();
            GridObstacles.Add(cur_object.GetComponent<GridObstacle>());
        }
        cur_object.ReplaceObject(ID);
        AddGridObject(X, Y, cur_object);
        GetGridCell(X, Y).SnapObject();
        return cur_object;
    }


    GriddableObject GetGridObject(int x, int y)
    {
        if (x < 0 || y < 0 || x >= SizeX_ || y >= SizeY_)
        {
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
        List<(int, int)> cells = new List<(int, int)>();

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
            cells.Add((0, 0));
        }
        else if (roll.rollRadius == RollRadius.PlayerRadius || roll.rollRadius == RollRadius.TargetRadius)
        {
            foreach (var pos in roll.DamagePositions)
            {
                cells.Add(pos);
            }
        }
        else if (roll.rollRadius == RollRadius.StLine)
        {
            int dist = roll.skill != null ? roll.skill.dist : 3;

            if (dir == Direction.up)
            {
                for (int d = 1; d <= dist; d++) cells.Add((0, d));
            }
            else if (dir == Direction.down)
            {
                for (int d = 1; d <= dist; d++) cells.Add((0, -d));
            }
            else if (dir == Direction.right)
            {
                for (int d = 1; d <= dist; d++) cells.Add((d, 0));
            }
            else if (dir == Direction.left)
            {
                for (int d = 1; d <= dist; d++) cells.Add((-d, 0));
            }
        }
        else if (roll.rollRadius == RollRadius.DgLine)
        {
            int dist = roll.skill != null ? roll.skill.dist : 3;

            if (dir == Direction.dg_r_u)
            {
                for (int d = 1; d <= dist; d++) cells.Add((d, d));
            }
            else if (dir == Direction.dg_r_d)
            {
                for (int d = 1; d <= dist; d++) cells.Add((d, -d));
            }
            else if (dir == Direction.dg_l_u)
            {
                for (int d = 1; d <= dist; d++) cells.Add((-d, d));
            }
            else if (dir == Direction.dg_l_d)
            {
                for (int d = 1; d <= dist; d++) cells.Add((-d, -d));
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
            //Debug.Log("Adding out of grid: " + x + " " + y);
            return;
        }
        if (GetGridObject(x, y) != null) 
        {
            //Debug.Log("Adding object on top of anather object: " + x + " " + y);
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

    public List<GriddableObject> GetTargetedObjects()
    {
        var list = new List<GriddableObject>();
        foreach (var row in Cells_)
        {
            foreach (var cell in row)
            {
                if (cell.color_type == GridCell.ColorType.Red)
                {
                    if (cell.object_ != null) list.Add(cell.object_);
                }
            }
        }
        return list;
    }

    public GridCell EnemyFindBestCell(GridEnemy enemy)
    {
        var start_cell = enemy.cell_;
        int maxMoves = enemy.enemy_.cur_moves;

        List<GridCell> stopCells = new List<GridCell>();
        List<GridCell> nonstopCells = new List<GridCell>();

        ClearVisitedFlags();

        Queue<GridCell> queue = new Queue<GridCell>();
        queue.Enqueue(start_cell);
        start_cell.visited = true;
        start_cell.moves = 1;

        while (queue.Count > 0)
        {
            GridCell cell = queue.Dequeue();

            if (cell.moves >= maxMoves)
                continue;

            var up = GetGridCell(cell.x_, cell.y_ + 1);
            var down = GetGridCell(cell.x_, cell.y_ - 1);
            var left = GetGridCell(cell.x_ - 1, cell.y_);
            var right = GetGridCell(cell.x_ + 1, cell.y_);

            CheckAndAddCell(up, cell, maxMoves, stopCells, nonstopCells, queue);
            CheckAndAddCell(down, cell, maxMoves, stopCells, nonstopCells, queue);
            CheckAndAddCell(left, cell, maxMoves, stopCells, nonstopCells, queue);
            CheckAndAddCell(right, cell, maxMoves, stopCells, nonstopCells, queue);
        }

        List<GridCell> allAvailableCells = new List<GridCell>();
        allAvailableCells.AddRange(stopCells);
        allAvailableCells.AddRange(nonstopCells);

        allAvailableCells = allAvailableCells.Where(c => c.moves <= maxMoves).ToList();
        allAvailableCells = allAvailableCells.Where(c => !IsCellOccupiedByPlayer(c)).ToList();

        if (allAvailableCells.Count == 0)
            return start_cell;

        bool hasAttackRolls = enemy.enemy_.CurrentRolls.Any(r => r.rollType == RollType.Atk);

        if (!hasAttackRolls)
        {
            GridCell playerCell = FindPlayerCell();
            if (playerCell != null)
            {
                return allAvailableCells.OrderBy(c => GetDistance(c, playerCell)).FirstOrDefault() ?? start_cell;
            }
            return start_cell;
        }

        List<GridCell> playerCells = FindAllPlayerCells();

        GridCell bestCell = null;
        int maxTargets = -1;

        foreach (var cell in allAvailableCells)
        {
            int targetsCount = CountReachablePlayers(cell, enemy);

            //Debug.Log($"Cell ({cell.x_}, {cell.y_}) moves: {cell.moves}, targets: {targetsCount}");

            if (targetsCount > maxTargets)
            {
                maxTargets = targetsCount;
                bestCell = cell;
            }
            else if (targetsCount == maxTargets && bestCell != null)
            {
                float currentMinDistance = GetMinDistanceToAnyPlayer(cell, playerCells);
                float bestMinDistance = GetMinDistanceToAnyPlayer(bestCell, playerCells);

                if (currentMinDistance < bestMinDistance)
                {
                    bestCell = cell;
                }
                else if (Mathf.Approximately(currentMinDistance, bestMinDistance) && cell.moves < bestCell.moves)
                {
                    bestCell = cell;
                }
            }
        }

        if (bestCell != null && IsCellOccupiedByPlayer(bestCell))
        {
            bestCell = allAvailableCells.FirstOrDefault(c => !IsCellOccupiedByPlayer(c)) ?? start_cell;
        }

        //Debug.Log($"Best cell: ({bestCell?.x_}, {bestCell?.y_}) with moves: {bestCell?.moves}, targets: {maxTargets}");
        return bestCell ?? start_cell;
    }

    private List<GridCell> FindAllPlayerCells()
    {
        List<GridCell> playerCells = new List<GridCell>();

        for (int i = 0; i < SizeX_; i++)
        {
            for (int j = 0; j < SizeY_; j++)
            {
                var cell = GetGridCell(i, j);
                if (cell != null && IsCellOccupiedByPlayer(cell))
                {
                    playerCells.Add(cell);
                }
            }
        }

        return playerCells;
    }
    private float GetMinDistanceToAnyPlayer(GridCell fromCell, List<GridCell> playerCells)
    {
        if (playerCells == null || playerCells.Count == 0)
            return float.MaxValue;

        float minDistance = float.MaxValue;

        foreach (var playerCell in playerCells)
        {
            float distance = GetDistance(fromCell, playerCell);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }

        return minDistance;
    }

    private void CheckAndAddCell(GridCell neighbor, GridCell current, int maxMoves,
                                 List<GridCell> stopCells, List<GridCell> nonstopCells,
                                 Queue<GridCell> queue)
    {
        if (neighbor == null || neighbor.visited)
            return;

        if (!neighbor.IsMovable())
            return;

        if (IsCellOccupiedByPlayer(neighbor))
            return;

        int newMoves = current.moves + 1;

        if (newMoves > maxMoves)
            return;

        neighbor.moves = newMoves;

        if (neighbor.IsStoppable())
        {
            stopCells.Add(neighbor);
        }
        else
        {
            nonstopCells.Add(neighbor);
        }

        neighbor.ParentCell = current;
        neighbor.visited = true;
        queue.Enqueue(neighbor);
    }

    private bool IsCellOccupiedByPlayer(GridCell cell)
    {
        if (cell == null || cell.object_ == null)
            return false;

        var gridObject = cell.object_.GetComponent<GriddableObject>();
        if (gridObject == null)
            return false;

        if (gridObject.GType_ != GriddableObject.GriddableObjectType.Character)
            return false;

        var character = cell.object_.GetComponent<GridCharacter>();
        return character != null;
    }

    private GridCell FindPlayerCell()
    {
        for (int i = 0; i < SizeX_; i++)
        {
            for (int j = 0; j < SizeY_; j++)
            {
                var cell = GetGridCell(i, j);
                if (cell != null && IsCellOccupiedByPlayer(cell))
                {
                    return cell;
                }
            }
        }
        return null;
    }

    private int CountReachablePlayers(GridCell fromCell, GridEnemy enemy)
    {
        HashSet<GridCharacter> reachablePlayers = new HashSet<GridCharacter>();

        if (enemy.enemy_.CurrentRolls == null || enemy.enemy_.CurrentRolls.Count == 0)
            return 0;

        foreach (var roll in enemy.enemy_.CurrentRolls)
        {
            if (roll.rollType == RollType.Def)
                continue;

            if (roll.rollType != RollType.Atk)
                continue;

            List<GridCell> targetCells = GetTargetCellsForRoll(fromCell, roll);

            foreach (var targetCell in targetCells)
            {
                List<Direction> directions = GetDirectionsForRadius(roll);

                foreach (var dir in directions)
                {
                    List<GridCell> damageCells = GetDamageCellsFromTargetCell(targetCell, roll, dir);

                    foreach (var damageCell in damageCells)
                    {
                        if (damageCell != null && IsCellOccupiedByPlayer(damageCell))
                        {
                            GridCharacter player = damageCell.object_.GetComponent<GridCharacter>();
                            if (player != null && player != enemy)
                            {
                                reachablePlayers.Add(player);
                            }
                        }
                    }
                }
            }
        }

        return reachablePlayers.Count;
    }

    public List<GridCell> GetTargetCellsForRoll(GridCell fromCell, Roll roll)
    {
        List<GridCell> targetCells = new List<GridCell>();
        int dist = GetSkillDist(roll);

        if (roll.skill.rollDist == RollDist.Any || roll.skill.rollDist == RollDist.Radius)
        {
            for (int i = -dist; i <= dist; i++)
            {
                for (int j = -dist; j <= dist; j++)
                {
                    if (Mathf.Abs(i) + Mathf.Abs(j) > dist)
                        continue;

                    GridCell cell = GetGridCell(fromCell.x_ + i, fromCell.y_ + j);
                    if (cell != null)
                        targetCells.Add(cell);
                }
            }
        }
        else if (roll.skill.rollDist == RollDist.StLine)
        {
            for (int d = 1; d <= dist; d++)
            {
                GridCell up = GetGridCell(fromCell.x_, fromCell.y_ + d);
                GridCell down = GetGridCell(fromCell.x_, fromCell.y_ - d);
                GridCell right = GetGridCell(fromCell.x_ + d, fromCell.y_);
                GridCell left = GetGridCell(fromCell.x_ - d, fromCell.y_);

                if (up != null) targetCells.Add(up);
                if (down != null) targetCells.Add(down);
                if (right != null) targetCells.Add(right);
                if (left != null) targetCells.Add(left);
            }
        }
        else if (roll.skill.rollDist == RollDist.DgLine)
        {
            for (int d = 1; d <= dist; d++)
            {
                GridCell ru = GetGridCell(fromCell.x_ + d, fromCell.y_ + d);
                GridCell rd = GetGridCell(fromCell.x_ + d, fromCell.y_ - d);
                GridCell lu = GetGridCell(fromCell.x_ - d, fromCell.y_ + d);
                GridCell ld = GetGridCell(fromCell.x_ - d, fromCell.y_ - d);

                if (ru != null) targetCells.Add(ru);
                if (rd != null) targetCells.Add(rd);
                if (lu != null) targetCells.Add(lu);
                if (ld != null) targetCells.Add(ld);
            }
        }

        return targetCells;
    }

    public List<GriddableObject> GetTargetObjectsForRoll(GridCell fromCell, Roll roll)
    {
        List<GriddableObject> targetObjects = new List<GriddableObject>();
        int dist = GetSkillDist(roll);

        if (roll.skill.rollDist == RollDist.Any || roll.skill.rollDist == RollDist.Radius)
        {
            for (int i = -dist; i <= dist; i++)
            {
                for (int j = -dist; j <= dist; j++)
                {
                    if (Mathf.Abs(i) + Mathf.Abs(j) > dist)
                        continue;

                    GridCell cell = GetGridCell(fromCell.x_ + i, fromCell.y_ + j);
                    if (cell != null)
                    {
                        if (cell.object_ != null)
                        {
                            targetObjects.Add(cell.object_);
                        }
                    }
                        
                }
            }
        }
        else if (roll.skill.rollDist == RollDist.StLine)
        {
            for (int d = 1; d <= dist; d++)
            {
                GridCell up = GetGridCell(fromCell.x_, fromCell.y_ + d);
                GridCell down = GetGridCell(fromCell.x_, fromCell.y_ - d);
                GridCell right = GetGridCell(fromCell.x_ + d, fromCell.y_);
                GridCell left = GetGridCell(fromCell.x_ - d, fromCell.y_);

                if (up != null) { if (up.object_ != null) { targetObjects.Add(up.object_); } }
                if (down != null) { if (down.object_ != null) { targetObjects.Add(down.object_); } }
                if (right != null) { if (right.object_ != null) { targetObjects.Add(right.object_); } }
                if (left != null) { if (left.object_ != null) { targetObjects.Add(left.object_); } }
            }
        }
        else if (roll.skill.rollDist == RollDist.DgLine)
        {
            for (int d = 1; d <= dist; d++)
            {
                GridCell ru = GetGridCell(fromCell.x_ + d, fromCell.y_ + d);
                GridCell rd = GetGridCell(fromCell.x_ + d, fromCell.y_ - d);
                GridCell lu = GetGridCell(fromCell.x_ - d, fromCell.y_ + d);
                GridCell ld = GetGridCell(fromCell.x_ - d, fromCell.y_ - d);

                if (ru != null) { if (ru.object_ != null) { targetObjects.Add(ru.object_); } }
                if (rd != null) { if (rd.object_ != null) { targetObjects.Add(rd.object_); } }
                if (lu != null) { if (lu.object_ != null) { targetObjects.Add(lu.object_); } }
                if (ld != null) { if (ld.object_ != null) { targetObjects.Add(ld.object_); } }
            }
        }

        return targetObjects;
    }

    private int GetSkillDist(Roll roll)
    {
        return roll.skill.dist;
    }

    private void ClearVisitedFlags()
    {
        for (int i = 0; i < SizeX_; i++)
        {
            for (int j = 0; j < SizeY_; j++)
            {
                var cell = GetGridCell(i, j);
                if (cell != null)
                {
                    cell.visited = false;
                    cell.moves = 0;
                    cell.ParentCell = null;
                }
            }
        }
    }

    private bool IsAlly(GridCharacter character, GridEnemy enemy)
    {
        return false;
    }

    private int GetDistance(GridCell cell1, GridCell cell2)
    {
        return Math.Abs(cell1.x_ - cell2.x_) + Math.Abs(cell1.y_ - cell2.y_);
    }

    public GridCell FindBestCellForRoll(Roll roll, GridEnemy enemy, out Direction bestDirection)
    {
        bestDirection = Direction.none;

        List<GridCell> targetCells = GetTargetCellsForRollFromEnemy(roll, enemy);

        if (targetCells.Count == 0)
            return enemy.cell_;

        GridCell bestCell = null;
        float bestScore = float.MinValue;
        Direction bestDir = Direction.none;

        foreach (var targetCell in targetCells)
        {
            List<Direction> directions = GetDirectionsForRadius(roll);

            foreach (var dir in directions)
            {
                List<GridCell> damageCells = GetDamageCellsFromTargetCell(targetCell, roll, dir);

                int playersHit = 0;
                int enemiesHit = 0;

                foreach (var damageCell in damageCells)
                {
                    if (damageCell == null) continue;

                    if (IsCellOccupiedByPlayer(damageCell))
                    {
                        GridCharacter player = damageCell.object_.GetComponent<GridCharacter>();
                        if (player != null && player != enemy)
                        {
                            playersHit++;
                        }
                    }

                    if (IsCellOccupiedByEnemy(damageCell))
                    {
                        GridEnemy otherEnemy = damageCell.object_.GetComponent<GridEnemy>();
                        if (otherEnemy != null)
                        {
                            enemiesHit++;
                        }
                    }
                }

                float score = CalculateScore(playersHit, enemiesHit, targetCell, enemy);

                Debug.Log($"Target ({targetCell.x_}, {targetCell.y_}) Dir: {dir} Players: {playersHit}, Enemies: {enemiesHit}, Score: {score}");

                if (score > bestScore)
                {
                    bestScore = score;
                    bestCell = targetCell;
                    bestDir = dir;
                }
                else if (Mathf.Approximately(score, bestScore) && bestCell != null)
                {
                    float currentDist = GetDistance(targetCell, enemy.cell_);
                    float bestDist = GetDistance(bestCell, enemy.cell_);

                    if (currentDist < bestDist)
                    {
                        bestCell = targetCell;
                        bestDir = dir;
                    }
                }
            }
        }

        bestDirection = bestDir;
        Debug.Log($"Best cell: ({bestCell?.x_}, {bestCell?.y_}) with score: {bestScore}, direction: {bestDir}");
        return bestCell ?? enemy.cell_;
    }
    private float CalculateScore(int playersHit, int enemiesHit, GridCell targetCell, GridEnemy enemy)
    {
        float score = 0;

        if (enemiesHit > 0)
        {
            score = -1000 - (enemiesHit * 100);
            score += playersHit * 10;
        }
        else
        {
            score = playersHit * 100;
        }

        List<GridCell> playerCells = FindAllPlayerCells();
        if (playerCells.Count > 0)
        {
            float minDistance = GetMinDistanceToAnyPlayer(targetCell, playerCells);
            score += Mathf.Max(0, 10 - minDistance) * 2;
        }

        return score;
    }
    private float EvaluateCellForRoll(GridCell fromCell, Roll roll, GridEnemy enemy, out Direction bestDirection)
    {
        bestDirection = Direction.none;
        int enemiesHit = 0;
        int playersHit = 0;

        List<GridCell> targetCells = GetTargetCellsForRoll(fromCell, roll);

        Dictionary<Direction, (int players, int enemies)> directionStats = new Dictionary<Direction, (int, int)>();
        List<Direction> directions = GetDirectionsForRadius(roll);

        foreach (var dir in directions)
        {
            int dirPlayersHit = 0;
            int dirEnemiesHit = 0;

            foreach (var targetCell in targetCells)
            {
                List<GridCell> damageCells = GetDamageCellsFromTargetCell(targetCell, roll, dir);
                Debug.Log("count:" + damageCells.Count);
                foreach (var damageCell in damageCells)
                {

                    if (damageCell == null)
                        continue;

                    if (IsCellOccupiedByPlayer(damageCell))
                    {
                        GridCharacter character = damageCell.object_.GetComponent<GridCharacter>();
                        if (character != null && character != enemy)
                        {
                            dirPlayersHit++;
                            playersHit++;
                        }
                    }

                    if (IsCellOccupiedByEnemy(damageCell))
                    {
                        GridEnemy otherEnemy = damageCell.object_.GetComponent<GridEnemy>();
                        if (otherEnemy != null && otherEnemy != enemy)
                        {
                            dirEnemiesHit++;
                            enemiesHit++;
                        }
                    }
                }
            }

            directionStats[dir] = (dirPlayersHit, dirEnemiesHit);
        }

        float score = 0;

        if (enemiesHit > 0)
        {
            score = -1000 - (enemiesHit * 100);
            score += playersHit * 10;
        }
        else
        {
            score = playersHit * 100;
        }

        List<GridCell> playerCells = FindAllPlayerCells();
        if (playerCells.Count > 0)
        {
            float minDistance = GetMinDistanceToAnyPlayer(fromCell, playerCells);
            score += Mathf.Max(0, 10 - minDistance) * 2;
        }

        Direction bestDir = Direction.none;
        int bestDirScore = int.MinValue;

        foreach (var kvp in directionStats)
        {
            int dirScore;
            if (kvp.Value.enemies > 0)
            {
                dirScore = -10000 - (kvp.Value.enemies * 1000) + (kvp.Value.players * 10);
            }
            else
            {
                dirScore = kvp.Value.players * 1000;
            }

            if (dirScore > bestDirScore)
            {
                bestDirScore = dirScore;
                bestDir = kvp.Key;
            }
            else if (dirScore == bestDirScore && bestDir != Direction.none)
            {
                if (kvp.Value.players > directionStats[bestDir].players)
                {
                    bestDir = kvp.Key;
                }
            }
        }

        bestDirection = bestDir;
        return score;
    }

    private float EvaluateCellForRoll(GridCell fromCell, Roll roll, GridEnemy enemy)
    {
        return EvaluateCellForRoll(fromCell, roll, enemy, out _);
    }

    private List<Direction> GetDirectionsForRadius(Roll roll)
    {
        List<Direction> directions = new List<Direction>();

        if (roll.rollRadius == RollRadius.Single ||
            roll.rollRadius == RollRadius.PlayerRadius ||
            roll.rollRadius == RollRadius.TargetRadius ||
            roll.rollRadius == RollRadius.Field)
        {
            directions.Add(Direction.none);
        }
        else if (roll.rollRadius == RollRadius.StLine)
        {
            directions.AddRange(new[] {
            Direction.up, Direction.down,
            Direction.left, Direction.right
        });
        }
        else if (roll.rollRadius == RollRadius.DgLine)
        {
            directions.AddRange(new[] {
            Direction.dg_r_u, Direction.dg_r_d,
            Direction.dg_l_u, Direction.dg_l_d
        });
        }

        return directions;
    }

    private bool IsCellOccupiedByEnemy(GridCell cell)
    {
        if (cell == null || cell.object_ == null)
            return false;

        var gridObject = cell.object_.GetComponent<GriddableObject>();
        if (gridObject == null)
            return false;

        if (gridObject.GType_ == GriddableObject.GriddableObjectType.Enemy)
            return true;

        return false;
    }

    public List<GridCell> GetTargetCellsForRollFromEnemy(Roll roll, GridEnemy enemy)
    {
        var fromCell = enemy.cell_;
        List<GridCell> targetCells = new List<GridCell>();

        int dist = 0;
        if (roll.skill != null)
        {
            dist = roll.skill.dist;
        }
        RollDist rollDist = roll.skill != null ? roll.skill.rollDist : RollDist.Any;
        if (rollDist == RollDist.Any)
        {
            foreach(var row in Cells_)
            {
                foreach(GridCell cell in row)
                {
                    targetCells.Add(cell);
                }
            }
        }
        else if (rollDist == RollDist.Radius)
        {
            for (int i = -dist; i <= dist; i++)
            {
                for (int j = -dist; j <= dist; j++)
                {
                    if (Mathf.Abs(i) + Mathf.Abs(j) > dist)
                        continue;

                    GridCell cell = GetGridCell(fromCell.x_ + i, fromCell.y_ + j);
                    if (cell != null)
                        targetCells.Add(cell);
                }
            }
        }
        else if (rollDist == RollDist.StLine)
        {
            for (int d = 1; d <= dist; d++)
            {
                GridCell up = GetGridCell(fromCell.x_, fromCell.y_ + d);
                GridCell down = GetGridCell(fromCell.x_, fromCell.y_ - d);
                GridCell right = GetGridCell(fromCell.x_ + d, fromCell.y_);
                GridCell left = GetGridCell(fromCell.x_ - d, fromCell.y_);

                if (up != null) targetCells.Add(up);
                if (down != null) targetCells.Add(down);
                if (right != null) targetCells.Add(right);
                if (left != null) targetCells.Add(left);
            }
        }
        else if (rollDist == RollDist.DgLine)
        {
            for (int d = 1; d <= dist; d++)
            {
                GridCell ru = GetGridCell(fromCell.x_ + d, fromCell.y_ + d);
                GridCell rd = GetGridCell(fromCell.x_ + d, fromCell.y_ - d);
                GridCell lu = GetGridCell(fromCell.x_ - d, fromCell.y_ + d);
                GridCell ld = GetGridCell(fromCell.x_ - d, fromCell.y_ - d);

                if (ru != null) targetCells.Add(ru);
                if (rd != null) targetCells.Add(rd);
                if (lu != null) targetCells.Add(lu);
                if (ld != null) targetCells.Add(ld);
            }
        }
        else if (rollDist == RollDist.Other)
        {
        }


        return targetCells;
    }

    public List<GridCell> GetDamageCellsFromTargetCell(GridCell targetCell, Roll roll, Direction dir = Direction.none)
    {
        List<GridCell> resultCells = new List<GridCell>();

        if (targetCell == null)
            return resultCells;

        if (roll.rollRadius == RollRadius.Single)
        {
            resultCells.Add(targetCell);
            return resultCells;
        }

        if (roll.rollRadius == RollRadius.Field)
        {
            for (int i = 0; i < SizeX_; i++)
            {
                for (int j = 0; j < SizeY_; j++)
                {
                    GridCell cell = GetGridCell(i, j);
                    if (cell != null && !resultCells.Contains(cell))
                    {
                        resultCells.Add(cell);
                    }
                }
            }
            return resultCells;
        }

        if (roll.rollRadius == RollRadius.PlayerRadius || roll.rollRadius == RollRadius.TargetRadius)
        {
            foreach (var pos in roll.DamagePositions)
            {
                int targetX = targetCell.x_ + pos.Item1;
                int targetY = targetCell.y_ + pos.Item2;

                GridCell cell = GetGridCell(targetX, targetY);
                if (cell != null && !resultCells.Contains(cell))
                {
                    resultCells.Add(cell);
                }
            }
            return resultCells;
        }

        if (roll.rollRadius == RollRadius.StLine || roll.rollRadius == RollRadius.DgLine)
        {
            resultCells.Add(targetCell);

            var damageCells = GetDamageCellsByRoll(targetCell.x_, targetCell.y_, roll, dir);

            foreach (var (dx, dy) in damageCells)
            {
                int targetX = targetCell.x_ + dx;
                int targetY = targetCell.y_ + dy;

                GridCell cell = GetGridCell(targetX, targetY);
                if (cell != null && !resultCells.Contains(cell))
                {
                    resultCells.Add(cell);
                }
            }
        }

        return resultCells;
    }
}

public enum Direction
{
    up,
    down,
    left,
    right,
    dg_r_u,
    dg_r_d,
    dg_l_u,
    dg_l_d,
    none
}