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
    public ColorType color_type = ColorType.None;

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

    public enum ColorType
    {
        None,
        Red,
        Yellow,
        Green,
        Blue
    }
}

