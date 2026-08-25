using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public float YMovement;
    public float FMovement;
    public float SMovement;
    public float FMovement_speed;
    public float SMovement_speed;
    public float MXMovement;
    public float MYMovement;
    public float MXMovement_speed;
    public float MYMovement_speed;
    public bool Shift;
    public float ZMovement;
    public float CameraSpeed = 0.1f;
    public float CameraSensativity;
    public float CameraSensativityY;
    public float CameraSensativityYLerp;
    public float CameraY;
    public float MaxDist;
    public float MinDist;
    public float CameraXSave;
    int cur_skill;
    public GridCell prev_cell = null;
    public bool is_redactor_moving;
    public GameObject SubSettingMenu;
    public bool lock_navigation = false;
    public bool freecam_mode = false;


    void Start()
    {
        battleMain = ResoursesDict.ObjectSet["BattleMain"].GetComponent<BattleMain>();
    }

    void Update()
    {
        if (!ResoursesDict.GetClass<BattleMain>().IsInBattle) return;
        field_ = battleMain.current_field;
        CheckHotKeys();

        if (lock_navigation) return;
        if (!freecam_mode)
        {
            XMovement = Input.GetAxis("Horizontal") * CameraSensativity * Time.deltaTime;
            YMovement = Input.GetAxis("Vertical") * CameraSensativityY * Time.deltaTime;
            ZMovement = Input.GetAxis("Mouse ScrollWheel") * CameraSensativity * Time.deltaTime;
            CameraReposition();
        }
        else
        {
            Shift = Input.GetKey(KeyCode.LeftShift);
            FMovement = Input.GetAxis("Vertical") * Time.deltaTime;
            SMovement = Input.GetAxis("Horizontal") * Time.deltaTime;
            MXMovement = Input.GetAxis("Mouse Y") * MXMovement_speed * Time.deltaTime;
            MYMovement = Input.GetAxis("Mouse X") * MYMovement_speed * Time.deltaTime;
            CameraRotation();
            CameraMovement();
        }
       
        GetTarget();
        ShowDamageCells();
       
    }

    void CameraRotation()
    {
        Camera.transform.Rotate(new Vector3(MXMovement , MYMovement , 0));
    }
    void CameraMovement()
    {
        Camera.transform.position += FMovement * FMovement_speed * Camera.transform.forward * (Shift ? 3 : 1);
        Camera.transform.position += SMovement * SMovement_speed * Camera.transform.right * (Shift ? 3 : 1); 
    }
    void CameraReposition()
    {
        if (Target == null) Target = field_.transform;
        transform.position = Vector3.Lerp(transform.position, Target.position, CameraSpeed * 0.2f);
        DestinationReposition();
        DestinationRepositionY();
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
        CameraDestination.transform.position = new Vector3(CameraDestination.transform.position.x, CameraY, CameraDestination.transform.position.z);
        if (ZMovement < 0 && Vector3.Distance(CameraDestination.position, Center.position) > MinDist)
        {
            CameraDestination.position = Vector3.Lerp(CameraDestination.position, Center.position, CameraSpeed * 0.1f);
        }
        if (ZMovement > 0)
        {
            CameraDestination.position = Vector3.Lerp(CameraDestination.position, CameraBack.position, CameraSpeed * 0.1f);
        }
    }

    void DestinationRepositionY()
    {
        if (YMovement < 0 && CameraY < 1.33f) YMovement = 0;
        if (YMovement > 0 && CameraY > 20f) YMovement = 0;
        CameraY = Mathf.Lerp(CameraY, CameraY + YMovement, CameraSensativityYLerp);
    }

    void ProcessRedactorMode()
    {
        if (IsPointerOverUIElementWithTag("CANTHIT"))
        {
            return;
        }

        Ray ray = Camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        var cell = GetCellByRayCast(Physics.RaycastAll(ray));
        Debug.Log(cell);

        if (cell != null)
        {
            if (is_redactor_moving)
            {
                if (cell.object_ == null)
                {
                    GridCell temp = PrevTarget.GetComponent<GriddableObject>().cell_;
                    temp.object_ = null;
                    cell.object_ = PrevTarget.GetComponent<GriddableObject>();
                    PrevTarget.GetComponent<GriddableObject>().cell_ = cell;
                    cell.SnapObject();
                }
                else
                {
                    ResoursesDict.GetClass<SoundMain>().Restrict();
                }
                is_redactor_moving = false;
                return;
            }
            field_.CellsNullify();
            cell.ColorCell(GridCell.ColorType.Blue);
            
            if (cell.object_ != null)
            {
                Target = cell.object_.transform;
                var obj = cell.object_.GetComponent<GriddableObject>();
                if (obj.GType_ == GriddableObject.GriddableObjectType.Character)
                {
                    UIController.UpdateTabCharacter(true);
                }
                else if (obj.GType_ == GriddableObject.GriddableObjectType.Enemy)
                {
                    UIController.UpdateTabEnemy(true, obj.GetComponent<GridEnemy>().enemy_);
                }
                else if (obj.GType_ == GriddableObject.GriddableObjectType.Obstacle)
                {
                    UIController.UpdateTabObstacle(true);
                }
                UIController.redactor_object_tab.RequestedUpdate(true);
            }
            else
            {
                Target = cell.transform;
                UIController.CloseAllObjectTabs();
                Debug.Log(888);
                UIController.redactor_add_tab.RequestedUpdate(true);
            }
        }
    }
    void GetTarget()
    {
        if (lock_navigation) return;
        if (Input.GetMouseButtonDown(0))
        {
            bool ret_flag = false;
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            if (field_ == null) return;
            if (field_.is_redactor)
            {
                ProcessRedactorMode();
                PrevTarget = Target;
                return;
            }

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
                else if (UIElem.UIType == "attack1")
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
                    ResoursesDict.GetClass<BattleMain>().NextTurn();
                }
                
                //ResoursesDict.GetClass<SkillFloatingWindow>().CloseAllWindows();
            }
            else
            {
                ResoursesDict.GetClass<SkillFloatingWindow>().CloseAllWindows();
                FloatingInfoMenu.CloseAllWindows();
                FloatingRollMenu.CloseAllWindows();
                Ray ray = Camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                if (CheckAttack())
                {
                    var cell = GetCellByRayCast(Physics.RaycastAll(ray));
                    //Debug.Log(cell);
                    if (cell.color_type == GridCell.ColorType.Red)
                    {
                        ret_flag = true;
                        var rolls = Target.GetComponent<GridCharacter>().GetCurrentRolls(cur_skill);
                        Target.GetComponent<GridCharacter>().MakeCurrentRolls(cur_skill);
                        StartCoroutine( ResoursesDict.ObjectSet["BattleMain"].GetComponent<BattleMain>().MakeFight(
                            Target.GetComponent<GridCharacter>(),
                            ResoursesDict.ObjectSet["BattleMain"].GetComponent<BattleMain>().current_field.GetTargetedObjects()));
                        var cells = field_.GetTargetedCells();
                        RollContext context = new RollContext();
                        context.Attacker = Target.GetComponent<GridCharacter>().GetCharacter();
                        context.Cells = cells;
                        foreach (var roll in rolls) 
                        {
                            roll.ProcessOnCellEffects(Target.GetComponent<GridCharacter>().GetCharacter(), null, context);
                        }
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
                        //Debug.Log(obj.GetCharacter().object_);
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
                                //Debug.Log(Target);
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
                        
                        Debug.Log("close:" + hit.transform.name + hit.transform.tag);
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
                var ray_all = Physics.RaycastAll(ray);
                var cell = GetCellByRayCast(ray_all);
                if (cell != null)
                {
                    if (cell.GetColor() == GridCell.ColorType.Yellow) ProcessCell(cell, character);
                }
                /*if (Physics.Raycast(ray, out RaycastHit hit))
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
                    else if (hit.collider.tag == "target")
                    {
                        var obj = RecursiveGriddableObjectFind(hit.collider.transform);
                        var cell = obj.cell_;
                        ProcessCell(cell, character);
                    }
                }*/
            }
        }

        void ProcessCell(GridCell cell, GridCharacter character)
        {
            if (cell == null) return;
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
        GridCell cell = null;
        foreach(var obj in raycasts)
        {
            //Debug.LogWarning(obj.transform.name);
            if (obj.transform.tag == "CANTHIT") return null;
            if (obj.transform.gameObject.tag == "cell")
            {
                cell = obj.transform.gameObject.GetComponent<GridCell>();
            }
        }
        return cell;
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
        //Debug.Log(t.name);
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

    bool IsPointerOverUIElementWithTag(string tag)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    void CheckHotKeys()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UIController.CloseTabs();
            UIController.CloseAllObjectTabs();
            if (!SubSettingMenu.activeSelf)
            {
                lock_navigation = true;
                ResoursesDict.GetClass<MainMenuManager>().SetMenuAndClearStack(SubSettingMenu);
            }
            else
            {
                lock_navigation = false;
                ResoursesDict.GetClass<MainMenuManager>().CloseCurrentMenu();
            }
        }
        else if (Input.GetKeyUp(KeyCode.R))
        {
            UIController.CloseAllObjectTabs();
            UIController.CloseTabs();
        }
        else if (Input.GetKeyUp(KeyCode.T))
        {
            freecam_mode = !freecam_mode;
            if (freecam_mode)
            {
                CameraXSave = Camera.transform.localRotation.x;
            }
            else
            {
                Camera.transform.localRotation = new Quaternion(CameraXSave, 0, 0, 0);
            }
        }

    }
}