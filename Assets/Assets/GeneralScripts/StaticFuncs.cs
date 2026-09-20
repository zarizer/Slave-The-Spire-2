using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

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
        obj.SetParent(ResoursesDict.GetClass<TrashManager>().transform);
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

    public static Texture2D GenerateTextureByNoise(int type, int width, int height, float xOrg, float yOrg, float scale = 1f)
    {
        Texture2D noiseTex;
        Color[] pix;
        noiseTex = new Texture2D(width, height);
        pix = new Color[noiseTex.width * noiseTex.height];
        switch (type)
        {
            case 0:

                float y = 0.0F;
                while (y < noiseTex.height)
                {
                    float x = 0.0F;
                    while (x < noiseTex.width)
                    {
                        float xCoord = xOrg + x / noiseTex.width * scale;
                        float yCoord = yOrg + y / noiseTex.height * scale;
                        float sample = Mathf.PerlinNoise(xCoord, yCoord);
                        Color use_color = Color.white;
                        if (((sample * 4) % 1) < 0.03f) { use_color = Color.black; }
                        else if (sample < 0.25f) { use_color = new Color(0.05f, 0.3f, 0.03f); }
                        else if (sample < 0.5f) { use_color = new Color(0.1f, 0.35f, 0.08f); }
                        else if (sample < 0.75f) { use_color = new Color(0.15f, 0.4f, 0.13f); }
                        else if (sample < 1) { use_color = new Color(0.2f, 0.45f, 0.18f); }

                        pix[(int)(y * noiseTex.width + x)] = use_color;
                        x++;
                    }
                    y++;
                }
                noiseTex.SetPixels(pix);
                noiseTex.Apply();
                return noiseTex;
        }

        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        return noiseTex;
    }

}



