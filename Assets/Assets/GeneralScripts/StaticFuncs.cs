using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class StaticFuncs : MonoBehaviour
{
    public static void DestroyChildren(Transform obj)
    {
        List<GameObject> children = new List<GameObject>();
        if (obj == null) return;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            children.Add(obj.transform.GetChild(i).gameObject);
        }
        foreach (GameObject child in children)
        {
            child.SetActive(false);
            child.transform.parent = ResoursesDict.GetClass<TrashManager>().transform;
        }
    }

    public static void DestroySingle(Transform obj)
    {
        obj.gameObject.SetActive(false);
        obj.parent = ResoursesDict.GetClass<TrashManager>().transform;
    }
    public static void DestroyChildren(GameObject obj)
    {
        DestroyChildren(obj.transform);
    }
                     
    public static bool RecursiveGetComponentDeep<T>(GameObject obj, out T component) 
    {
        component = default(T);
        List<GameObject> BFSList = new List<GameObject>();
        BFSList.Add(obj);
        int i = 0;
        while (BFSList.Count > 0)
        {
            GameObject cur_obj = BFSList[0];
            if (cur_obj.TryGetComponent<T>(out component)) return true;
            BFSList.AddRange(GetChildren(cur_obj.transform));
            BFSList.RemoveAt(0);
            if (i > 500)
            {
                Debug.LogError("RecursiveGetComponentDeep too deep recursion Error");
                break;
            }
            i++;
        }
        return false;
    }

    static List<GameObject> GetChildren(Transform t)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform obj in t)
        {
            children.Add(obj.gameObject);
        }
        return children;
    }

    public static int RandomRangeInclusive(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }
}