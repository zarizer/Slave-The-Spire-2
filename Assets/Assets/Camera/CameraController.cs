using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform Camera;
    public Transform Center;
    public Transform CameraDestination;
    public Transform Target;
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
    void Start()
    {
        
    }

    void Update()
    {
        XMovement = Input.GetAxis("Horizontal") * CameraSensativity;
        ZMovement = Input.GetAxis("Mouse ScrollWheel") * CameraSensativity;
        
        CameraReposition();
        GetTarget();
    }

    void CameraReposition()
    {
        transform.position = Vector3.Lerp(transform.position, Target.position, CameraSpeed*0.2f);
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
            CameraDestination.position = Vector3.Lerp(CameraDestination.position, Center.position, CameraSpeed*0.1f);
            Debug.Log("in");
        }
        if (ZMovement > 0)
        {
            CameraDestination.position = Vector3.Lerp(CameraDestination.position, CameraBack.position, CameraSpeed*0.1f);
            Debug.Log("out");
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
                var UIElem = GetLastUI(results).GetComponent<UIElement>();
                if (UIElem.UIType == "move")
                {
                    var obj = Target.GetComponent<GridCharacter>();
                    if (obj.CanMove())
                    {
                        obj.TryingToMove = true;
                        obj.field_.FindWaysPlayer(obj.cell_.x_, obj.cell_.y_, (obj).character_.moves);
                    }
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
                        Target = obj.transform;
                        if (obj.GType_ == GriddableObject.GriddableObjectType.Character)
                        {
                            UIController.character_tab_controller.RequestedUpdate(true);
                            
                        }
                        else
                        {

                            UIController.character_tab_controller.RequestedUpdate(false);
                            field_.CellsNullify();
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
                                UIController.character_tab_controller.RequestedUpdate(true);
                            }
                            else
                            {
                                Target = DeffaultTarget;
                                UIController.character_tab_controller.RequestedUpdate(false);
                            }

                        }
                        else
                        {
                            Target = DeffaultTarget;
                            UIController.character_tab_controller.RequestedUpdate(false);
                        }
                    }
                    else if (hit.transform.tag != "UI")
                    {
                        Target = DeffaultTarget;
                        UIController.character_tab_controller.RequestedUpdate(false);
                        field_.CellsNullify();
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
