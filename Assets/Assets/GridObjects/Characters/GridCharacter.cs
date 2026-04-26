using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCharacter : GriddableObject
{
    public Vector3 DestinationPosition;

    public bool player_;

    public GameObject TexturePlane;
    public CharacterBase character_;
    public int CharacterId_ = -1;
    

    public bool TryingToMove = false;
    void Start()
    {
        character_ = GetCharacterByID(CharacterId_);
    }

    void Update()
    {
        LookAtCamera();
        MoveToDestination();
    }

    private void OnEnable()
    {
        GType_ = GriddableObjectType.Character;
    }

    void LookAtCamera()
    {
        TexturePlane.transform.LookAt(field_.Camera.Camera);
        TexturePlane.transform.localEulerAngles = new Vector3(0,
                                                              TexturePlane.transform.localEulerAngles.y,
                                                              TexturePlane.transform.localEulerAngles.z);
    }

    void MoveToDestination()
    {
        transform.position = Vector3.Lerp(transform.position, DestinationPosition, 0.1f);
    }

    public void MoveToCell(GridCell cell)
    {
        if (cell.color_type == GridCell.ColorType.Green)
        {
            DestinationPosition = new Vector3(cell.transform.position.x, cell.transform.position.y - 0.5f, cell.transform.position.z);
            cell_.object_ = null;
            cell.object_ = this;
            cell_ = cell;
        }
    }


    CharacterBase GetCharacterByID(int id)
    {
        CharacterBase ret_character;
        ret_character = new TestCharacter();

        /*
         ÏÐÎÂÅÐÊÈ ÑÞÄÀ
        */

        ret_character.Init();
        return ret_character;
    }
}






