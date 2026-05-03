using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform Camera;
    public Transform Center;
    public Transform CameraDestination;
    public Transform Target;
    public Transform PrevTarget;
    public Transform CameraBack;
    public Transform DeffaultTarget;
    public GridField field_;
    public UIController UIController;
    public bool IsTargeted;
    public float XMovement;
    public float ZMovement;
    public float CameraSpeed = 0.1f;
    public float CameraSensativity;
    public float CameraY;
    public float MaxDist;
    public float MinDist;
    int cur_skill;
    public GridCell prev_cell = null;
    void Start()
    {

    }

    void Update()
    {
        XMovement = Input.GetAxis("Horizontal") * CameraSensativity;
        ZMovement = Input.GetAxis("Mouse ScrollWheel") * CameraSensativity;

        CameraReposition();
        GetTarget();
        ShowDamageCells();
    }

    void CameraReposition()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, CameraSpeed * 0.2f);
        DestinationReposition();
        DestinationRepositionZ();
        Camera.position = Vector3.Lerp(Camera.position, CameraDestination.position, CameraSpeed);
        Camera.rotation = Quaternion.Lerp(Camera.rotation, CameraDestination.rotation, CameraSpeed);
    }

    void DestinationReposition()
    {
        CameraDestination.RotateAround(Center.position, Vector3.up, XMovement);
        CameraBack.RotateAround(Center.position, Vector3.up, XMovement);
        CameraDestination.LookAt(Center);
    }

    void DestinationRepositionZ()
    {
        if (ZMovement < 0 && Vector3.Distance(CameraDestination.position, Center.position) > MinDist)
        {
            CameraDestination.position = Vector3.Lerp(CameraDestination.position, Center.position, CameraSpeed * 0.1f);
        }
        if (ZMovement > 0)
        {
            CameraDestination.position = Vector3.Lerp(CameraDestination.position, CameraBack.position, CameraSpeed * 0.1f);
        }
    }

    void GetTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (GetLastUI(results) != null) 
            {
                Debug.Log(GetLastUI(results));
                var UIElem = GetLastUI(results).GetComponent<UIElement>();
                if (UIElem.UIType == "move")
                {
                    field_.CellsNullify();
                    var obj = Target.GetComponent<GridCharacter>();
                    if (obj.CanMove())
                    {
                        obj.TryingToMove = true;
                        obj.field_.FindWaysPlayer(obj.cell_.x_, obj.cell_.y_, (obj).character_.cur_moves);
                    }
                }
                if (UIElem.UIType == "attack1")
                {
                    cur_skill = 1;
                    field_.CellsNullify();
                    var obj = Target.GetComponent<GridCharacter>();
                    obj.TRyingToAttack = true;
                    field_.FindAttacksPlayer(obj.cell_.x_, obj.cell_.y_, obj.character_.Skill1);
                }
                if (UIElem.UIType == "attack2")
                {
                    cur_skill = 2;
                    field_.CellsNullify();
                    var obj = Target.GetComponent<GridCharacter>();
                    obj.TRyingToAttack = true;
                    field_.FindAttacksPlayer(obj.cell_.x_, obj.cell_.y_, obj.character_.Skill2);
                }
                if (UIElem.UIType == "attack3")
                {
                    cur_skill = 3;
                    field_.CellsNullify();
                    var obj = Target.GetComponent<GridCharacter>();
                    obj.TRyingToAttack = true;
                    field_.FindAttacksPlayer(obj.cell_.x_, obj.cell_.y_, obj.character_.Skill3);
                }
                if (UIElem.UIType == "attack4")
                {
                    cur_skill = 4;
                    field_.CellsNullify();
                    var obj = Target.GetComponent<GridCharacter>();
                    obj.TRyingToAttack = true;
                    field_.FindAttacksPlayer(obj.cell_.x_, obj.cell_.y_, obj.character_.Skill4);
                }
            }
            else
            {
                Ray ray = Camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Debug.Log("Hitted: " + hit.transform.tag);
                    Debug.Log("Hitted: " + hit.transform.name);

                    if (hit.transform.GetComponent<GriddableObject>() != null)
                    {
                        GriddableObject obj = hit.transform.GetComponent<GriddableObject>();
                        PrevTarget = Target;
                        Target = obj.transform;
                        if (Target != PrevTarget)
                        {
                            field_.CellsNullify();
                            field_.GridObjectsActionNullify();
                        }
                        if (obj.GType_ == GriddableObject.GriddableObjectType.Character)
                        {
                            UIController.UpdateTabCharacter(true);

                        }
                        else if (obj.GType_ == GriddableObject.GriddableObjectType.Enemy)
                        {
                            GridEnemy obj_enemy = hit.transform.GetComponent<GridEnemy>();
                            UIController.UpdateTabEnemy(true);
                            obj.field_.FindWaysPlayer(obj.cell_.x_, obj.cell_.y_, obj_enemy.enemy_.cur_moves);

                        }
                        else
                        {

                            UIController.CloseAllObjectTabs();
                            field_.CellsNullify();
                            field_.GridObjectsActionNullify();
                        }
                    }
                    else if (hit.transform.tag == "cell")
                    {
                        if (Target.GetComponent<GridCharacter>() != null)
                        {
                            var character = Target.GetComponent<GridCharacter>();
                            if (character.player_ && character.TryingToMove)
                            {
                                character.MoveToCell(hit.transform.GetComponent<GridCell>());
                                field_.CellsNullify();
                                field_.GridObjectsActionNullify();
                                UIController.UpdateTabCharacter(true);
                            }
                            else
                            {
                                Target = DeffaultTarget;
                                UIController.CloseAllObjectTabs();
                                field_.CellsNullify();
                                field_.GridObjectsActionNullify();
                            }

                        }
                        else
                        {
                            PrevTarget = Target;
                            Target = DeffaultTarget;
                            UIController.CloseAllObjectTabs();
                            field_.CellsNullify();
                            field_.GridObjectsActionNullify();
                        }
                    }
                    else if (hit.transform.tag != "UI")
                    {
                        PrevTarget = Target;
                        Target = DeffaultTarget;
                        UIController.CloseAllObjectTabs();
                        field_.CellsNullify();
                        field_.GridObjectsActionNullify();
                    }


                }
            }
        }
    }

    void ShowDamageCells()
    {
        if (Target.GetComponent<GridCharacter>() != null)
        {
            var character = Target.GetComponent<GridCharacter>();
            if (character.TRyingToAttack)
            {
                var ray = Camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.tag == "cell")
                    {
                        var cell = hit.collider.GetComponent<GridCell>();
                        if (!cell.IsMainTarget && (cell.GetColor() == GridCell.ColorType.Red || cell.GetColor() == GridCell.ColorType.Yellow))
                        {
                            prev_cell.IsMainTarget = false;
                            prev_cell = cell;
                            cell.IsMainTarget = true;
                            
                           
                            if (cur_skill == 1)
                            {
                                field_.ShowDamagePlayer(cell.x_, cell.y_, character.character_.Skill1, character.cell_.x_, character.cell_.y_);
                                Debug.Log(9999);
                            }
                            if (cur_skill == 2)
                            {
                                field_.ShowDamagePlayer(cell.x_, cell.y_, character.character_.Skill2, character.cell_.x_, character.cell_.y_);
                            }
                            if (cur_skill == 3)
                            {
                                field_.ShowDamagePlayer(cell.x_, cell.y_, character.character_.Skill3, character.cell_.x_, character.cell_.y_);
                            }
                            if (cur_skill == 4)
                            {
                                field_.ShowDamagePlayer(cell.x_, cell.y_, character.character_.Skill4, character.cell_.x_, character.cell_.y_);
                            }
                        }
                    }
                }
            }
        }
    }

    GameObject GetLastUI(List<RaycastResult> Hits)
    {
        GameObject obj = null;
        foreach (var hit in Hits)
        {
            if (hit.gameObject.tag == "UI")
            {
                obj = hit.gameObject;
            }
        }
        return obj;
    }
    
}