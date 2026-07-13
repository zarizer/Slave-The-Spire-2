using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

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
    BattleMain battleMain;
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
        battleMain = ResoursesDict.ObjectSet["BattleMain"].GetComponent<BattleMain>();
    }

    void Update()
    {
        field_ = battleMain.current_field;

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
            bool ret_flag = false;
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);


            if (GetLastUI(results) != null) 
            {
                var UIElem = GetLastUI(results).GetComponent<UIElement>();
                if (UIElem == null) { }
                else if (UIElem.UIType == "move")
                {
                    field_.CellsNullify();
                    var obj = Target.GetComponent<GridCharacter>();
                    if (obj.CanMove())
                    {
                        obj.TryingToMove = true;
                        obj.field_.FindWaysPlayer(obj.cell_.x_, obj.cell_.y_, (obj).character_.cur_moves);
                    }
                }
                else  if (UIElem.UIType == "attack1")
                {
                    cur_skill = 1;
                    field_.CellsNullify();
                    var obj = Target.GetComponent<GridCharacter>();

                    obj.TRyingToAttack = true;
                    field_.FindAttacksPlayer(obj.cell_.x_, obj.cell_.y_, obj.character_.Skill1);
                }
                else if (UIElem.UIType == "attack2")
                {
                    cur_skill = 2;
                    field_.CellsNullify();
                    var obj = Target.GetComponent<GridCharacter>();
                    obj.TRyingToAttack = true;
                    field_.FindAttacksPlayer(obj.cell_.x_, obj.cell_.y_, obj.character_.Skill2);
                }
                else if (UIElem.UIType == "attack3")
                {
                    cur_skill = 3;
                    field_.CellsNullify();
                    var obj = Target.GetComponent<GridCharacter>();
                    obj.TRyingToAttack = true;
                    field_.FindAttacksPlayer(obj.cell_.x_, obj.cell_.y_, obj.character_.Skill3);
                }
                else if (UIElem.UIType == "attack4")
                {
                    cur_skill = 4;
                    field_.CellsNullify();
                    var obj = Target.GetComponent<GridCharacter>();
                    obj.TRyingToAttack = true;
                    field_.FindAttacksPlayer(obj.cell_.x_, obj.cell_.y_, obj.character_.Skill4);
                }
                else if (UIElem.UIType == "next_cycle")
                {
                    field_.CellsNullify();
                    var obj = Target.GetComponent<GridCharacter>();
                    ResoursesDict.GetClass<BattleMain>().NextCycle();
                }
                FloatingInfoMenu.CloseAllWindows();
                FloatingRollMenu.CloseAllWindows();
                ResoursesDict.GetClass<SkillFloatingWindow>().CloseAllWindows();
            }
            else
            {
                Ray ray = Camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                if (CheckAttack())
                {
                    var cell = GetCellByRayCast(Physics.RaycastAll(ray));
                    Debug.Log(cell);
                    if (cell.color_type == GridCell.ColorType.Red)
                    {
                        ret_flag = true;
                        Target.GetComponent<GridCharacter>().MakeCurrentRolls(cur_skill);
                        ResoursesDict.ObjectSet["BattleMain"].GetComponent<BattleMain>().MakeFight(
                            Target.GetComponent<GridCharacter>(),
                            ResoursesDict.ObjectSet["BattleMain"].GetComponent<BattleMain>().current_field.GetTargetedObjects());
                        field_.CellsNullify();
                        field_.GridObjectsActionNullify();
                    }
                }
                if (ret_flag) return;
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    //Debug.Log("Hitted: " + hit.transform.tag);
                    //Debug.Log("Hitted: " + hit.transform.name);

                    if (hit.transform.GetComponent<GriddableObject>() != null || hit.transform.tag == "target")
                    {
                        GriddableObject obj = RecursiveGriddableObjectFind(hit.transform);
                        PrevTarget = Target;
                        Target = obj.transform;
                        obj.OnTarget();
                        //Debug.Log("Hitted griddable object");
                        if (Target != PrevTarget)
                        {
                            field_.CellsNullify();
                            field_.GridObjectsActionNullify();
                        }
                        if (obj.GType_ == GriddableObject.GriddableObjectType.Character)
                        {
                            //Debug.Log("Hitted character");
                            UIController.UpdateTabCharacter(true);

                        }
                        else if (obj.GType_ == GriddableObject.GriddableObjectType.Enemy)
                        {

                            GridEnemy obj_enemy = hit.transform.GetComponent<GridEnemy>();
                            Debug.Log(obj_enemy.enemy_.CurrentRolls.Count);
                            UIController.UpdateTabEnemy(true, obj_enemy.GetComponent<GridEnemy>().enemy_);
                            obj.field_.FindWaysPlayer(obj.cell_.x_, obj.cell_.y_, obj_enemy.enemy_.cur_moves);

                        }
                        else
                        {
                            if (obj.GType_ == GriddableObject.GriddableObjectType.Obstacle &&
                                ((ObstacleBase)(obj.GetComponent<GridObstacle>().GetCharacter())).Interactable)
                            {
                                Debug.Log(Target);
                                UIController.UpdateTabObstacle(true);
                            }
                            else
                            {
                                UIController.CloseAllObjectTabs();
                            }
                                
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
                    if (hit.transform.tag != "UI" && hit.transform.tag != "CreateWindow")
                    {
                        FloatingInfoMenu.CloseAllWindows();
                    }

                }
            }
        }
    }

    bool CheckAttack()
    {
        if (Target == null) return false;
        if (Target.GetComponent<GriddableObject>() == null) return false;
        if (Target.GetComponent<GriddableObject>().GType_ != GriddableObject.GriddableObjectType.Character) return false;
        if (Target.GetComponent<GriddableObject>().GType_ == GriddableObject.GriddableObjectType.Character &&
                               Target.GetComponent<GridCharacter>().TRyingToAttack)
        {
            /*ResoursesDict.ObjectSet["BattleMain"].GetComponent<BattleMain>().MakeFight
                (
                    PrevTarget.GetComponent<GridCharacter>(),
                    Target.GetComponent<GriddableObject>()
                );*/
            return true;
        }
        return false;
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
                        ProcessCell(cell, character);
                    }
                    else if (hit.collider.gameObject.GetComponent<GriddableObject>() != null)
                    {
                        var cell = hit.collider.gameObject.GetComponent<GriddableObject>().cell_;
                        ProcessCell(cell, character);
                    }
                }
            }
        }

        void ProcessCell(GridCell cell, GridCharacter character)
        {
            if (!cell.IsMainTarget && (cell.GetColor() == GridCell.ColorType.Red || cell.GetColor() == GridCell.ColorType.Yellow))
            {
                prev_cell.IsMainTarget = false;
                prev_cell = cell;
                cell.IsMainTarget = true;


                if (cur_skill == 1)
                {
                    field_.ShowDamagePlayer(cell.x_, cell.y_, character.character_.Skill1, character.cell_.x_, character.cell_.y_);
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
    
    GridCell GetCellByRayCast(RaycastHit[] raycasts)
    {
        foreach(var obj in raycasts)
        {
            Debug.Log(obj.transform.gameObject);
            if (obj.transform.gameObject.tag == "cell")
            {
                return (obj.transform.gameObject.GetComponent<GridCell>());
            }
        }
        return null;
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

    GriddableObject RecursiveGriddableObjectFind(Transform t)
    {
        Debug.Log(t.name);
        GriddableObject g;
        g = t.gameObject.GetComponent<GriddableObject>();
        if (g != null)
        {
            return g;
        }
        else
        {
            
            return RecursiveGriddableObjectFind(t.parent);
        }
    }
}