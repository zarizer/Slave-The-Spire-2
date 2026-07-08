using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GriddableObject : MonoBehaviour
{
    public bool player_;
    [SerializeField]
    public Vector3 DestinationPosition;
    public GriddableObjectType GType_;

    public GridField field_;
    public GridCell cell_;
    public bool TryingToMove = false;
    public bool TRyingToAttack = false;
    public bool IsCustomLevel = false;
    public int CustomLevel = 1;
    public int specialValue = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void ReplaceObject(int ID) { }
    public void MoveToCell(GridCell cell)
    {
        GridCharacter obj_character = null;
        GridEnemy obj_enemy = null;
        if (GType_ == GriddableObjectType.Character) {
            obj_character = (GridCharacter)this;
            if (cell.color_type == GridCell.ColorType.Green)
            {
                MoveByWay(GetWay(cell_, cell), 0.2f);
                cell_.object_ = null;
                cell.object_ = this;
                cell_ = cell;
                obj_character.TryingToMove = false;
                obj_character.character_.cur_moves = 0;
            }
        }
        if (GType_ == GriddableObjectType.Enemy)
        {
            obj_enemy = (GridEnemy)this;

            MoveByWay(GetWay(cell_, cell), 0.2f);
            cell_.object_ = null;
            cell.object_ = this;
            cell_ = cell;
            obj_enemy.TryingToMove = false;
            obj_enemy.enemy_.cur_moves = 0;

        }

    }

    List<GridCell> GetWay(GridCell start_cell, GridCell finish_cell)
    {
        List<GridCell> way = new List<GridCell>();

        way.Add(finish_cell);
        GridCell cur_cell = finish_cell.ParentCell;
        while (cur_cell != start_cell)
        {
            way.Add(cur_cell);
            cur_cell = cur_cell.ParentCell;
        }

        way.Reverse();
        return way;
    }

    void MoveByWay(List<GridCell> way, float timing, int cur_index = 0)
    {
        if (cur_index == way.Count) return;
        while (way[cur_index].object_ != null && way[cur_index].object_ != this) { cur_index++; }
        GridCell cur_cell = way[cur_index];
        DestinationPosition = GetDestinationByCell(cur_cell);
        LeanTween.delayedCall(timing, () => { MoveByWay(way, timing, cur_index + 1); });
    }

    Vector3 GetDestinationByCell(GridCell cell)
    {
        return new Vector3(cell.transform.position.x, cell.transform.position.y - 0.5f, cell.transform.position.z);
    }

    public virtual Roll GetFirstRoll() { return null; }

    public virtual void RemoveFirstRoll(float offset = 0f) { }

    public virtual int GetLevel() { return 1; }

    public virtual void GetDamage(Damage damage) { }

    public virtual CharacterBase GetCharacter() { return null; }
    public enum GriddableObjectType
    {
        Enemy,
        Character,
        Obstacle,
        Breakable
    }
}
