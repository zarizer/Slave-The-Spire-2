using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEnemy : GriddableObject
{

    public GameObject TexturePlane;
    public EnemyBase enemy_;
    public int EnemyId_ = -1;
    void Start()
    {
        enemy_ = GetEnemyByID(EnemyId_);
        player_ = false;
    }

    void Update()
    {
        LookAtCamera();
        MoveToDestination();
    }

    private void OnEnable()
    {
        GType_ = GriddableObjectType.Enemy;
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

    public void ReplaceEnemy(int id)
    {
        enemy_ = GetEnemyByID(id);
    }
    EnemyBase GetEnemyByID(int id)
    {
        EnemyBase ret_character;

        ret_character = DataDicts.EnemySet[id].Clone();
        if (ret_character == null) ret_character = new TestEnemy();

        ret_character.Init();
        return ret_character;
    }

    public bool CanMove()
    {
        if (enemy_.cur_moves > 0) return true;
        return false;
    }


}
