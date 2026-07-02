using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x_;
    public int y_;
    public GriddableObject object_;
    public GridField field_;
    public List<GameObject> coloring_objects;
    public List<GameObject> coloring_objects_trans;
    public List<Material> materials;
    public bool visited = false;
    public int moves = 0;
    public int color_id = 0;
    public ColorType color_type = ColorType.None;
    public GridCell ParentCell = null;
    public ColorType PrevColor = ColorType.None;
    public bool IsTargeted = false;
    public bool IsMainTarget = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SnapObject()
    {
        Debug.Log("snapping:" + object_.name);
        if (object_ == null) return;
        if (object_.GType_ == GriddableObject.GriddableObjectType.Obstacle)
        {
            object_.transform.position = transform.position + Vector3.up * 0.45f;
        }
        if (object_.GType_ == GriddableObject.GriddableObjectType.Character)
        {
            object_.GetComponent<GridCharacter>().DestinationPosition = transform.position - Vector3.up * 0.5f;
        }
        if (object_.GType_ == GriddableObject.GriddableObjectType.Enemy)
        {
            object_.GetComponent<GridEnemy>().DestinationPosition = transform.position - Vector3.up * 0.5f;
        }
    }

    

    public void ColorCell(ColorType type)
    {
        if (PrevColor != type)
        {
            PrevColor = GetColor();
        } 
        color_type = type;
        if (type == ColorType.None)
        {
            foreach (var obj in coloring_objects)
            {
                obj.GetComponent<Renderer>().material = materials[0];
            }
            foreach (var obj in coloring_objects_trans)
            {
                obj.GetComponent<Renderer>().material = materials[1];
            }
            color_id = 0;
            
        }
        if (type == ColorType.Red)
        {
            foreach (var obj in coloring_objects)
            {
                obj.GetComponent<Renderer>().material = materials[2];
            }
            foreach (var obj in coloring_objects_trans)
            {
                obj.GetComponent<Renderer>().material = materials[3];
            }
            color_id = 1;
        }
        if (type == ColorType.Yellow)
        {
            foreach (var obj in coloring_objects)
            {
                obj.GetComponent<Renderer>().material = materials[4];
            }
            foreach (var obj in coloring_objects_trans)
            {
                obj.GetComponent<Renderer>().material = materials[5];
            }
            color_id = 2;
        }
        if (type == ColorType.Green)
        {
            foreach (var obj in coloring_objects)
            {
                obj.GetComponent<Renderer>().material = materials[6];
            }
            foreach (var obj in coloring_objects_trans)
            {
                obj.GetComponent<Renderer>().material = materials[7];
            }
            color_id = 3;
        }
        if (type == ColorType.Blue)
        {
            foreach (var obj in coloring_objects)
            {
                obj.GetComponent<Renderer>().material = materials[8];
            }
            foreach (var obj in coloring_objects_trans)
            {
                obj.GetComponent<Renderer>().material = materials[9];
            }
            color_id = 4;   
        }
    }

    public void ColorCell(int color_id)
    {
        if (PrevColor != GetColorById(color_id))
        {
            PrevColor = GetColor();
        }
        if (color_id == 0)
        {
            foreach (var obj in coloring_objects)
            {
                obj.GetComponent<Renderer>().material = materials[0];
            }
            foreach (var obj in coloring_objects_trans)
            {
                obj.GetComponent<Renderer>().material = materials[1];
            }
            color_id = 0;
        }
        if (color_id == 1)
        {
            foreach (var obj in coloring_objects)
            {
                obj.GetComponent<Renderer>().material = materials[2];
            }
            foreach (var obj in coloring_objects_trans)
            {
                obj.GetComponent<Renderer>().material = materials[3];
            }
            color_id = 1;
        }
        if (color_id == 2)
        {
            foreach (var obj in coloring_objects)
            {
                obj.GetComponent<Renderer>().material = materials[4];
            }
            foreach (var obj in coloring_objects_trans)
            {
                obj.GetComponent<Renderer>().material = materials[5];
            }
            color_id = 2;
        }
        if (color_id == 3)
        {
            foreach (var obj in coloring_objects)
            {
                obj.GetComponent<Renderer>().material = materials[6];
            }
            foreach (var obj in coloring_objects_trans)
            {
                obj.GetComponent<Renderer>().material = materials[7];
            }
            color_id = 3;
        }
        if (color_id == 4)
        {
            foreach (var obj in coloring_objects)
            {
                obj.GetComponent<Renderer>().material = materials[8];
            }
            foreach (var obj in coloring_objects_trans)
            {
                obj.GetComponent<Renderer>().material = materials[9];
            }
            color_id = 4;
        }
    }

    public bool IsMovable(bool isPlayer = true)
    {
        if (object_ == null) return true;
        if (object_.GType_ == GriddableObject.GriddableObjectType.Character &&
            ((GridCharacter)object_).player_ == isPlayer) return true;
        return false;
    }

    public bool IsStoppable()
    {
        if (object_ == null) return true;
        return false;
    }

    public void UpdateTargeting()
    {
        if (IsTargeted)
        {
            ColorCell(ColorType.Red);
        }
        else
        {
            ColorCell(color_id);
        }
    }

    public enum ColorType
    {
        None,
        Red,
        Yellow,
        Green,
        Blue
    }

    public ColorType GetColor()
    {
        if (color_id == 0) return ColorType.None;
        if (color_id == 3) return ColorType.Green;
        if (color_id == 1) return ColorType.Red;
        if (color_id == 2) return ColorType.Yellow;
        if (color_id == 4) return ColorType.Blue;
        return ColorType.None;
    }

    public ColorType GetColorById(int color_id_)
    {
        if (color_id_ == 0) return ColorType.None;
        if (color_id_ == 3) return ColorType.Green;
        if (color_id_ == 1) return ColorType.Red;
        if (color_id_ == 2) return ColorType.Yellow;
        if (color_id_ == 4) return ColorType.Blue;
        return ColorType.None;
    }

    public void FlashColor(Color flashColor, float duration)
    {
        
        foreach (var obj in coloring_objects)
        {
            Color originalColor = obj.GetComponent<Renderer>().material.color;
            LeanTween.color(obj, flashColor, duration)
            .setEase(LeanTweenType.easeInQuad)
            .setLoopPingPong(1)
            .setOnComplete(() => {
                obj.GetComponent<Renderer>().material.color = originalColor;
            });
        }
        
    }
}

