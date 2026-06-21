using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class StaticFuncs : MonoBehaviour
{
    public static void DestroyChildren(Transform obj)
    {
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            children.Add(obj.transform.GetChild(i).gameObject);
        }
        foreach (GameObject child in children)
        {
            Destroy(child);
        }
    }

    public static void DestroyChildren(GameObject obj)
    {
        DestroyChildren(obj.transform);
    }
}