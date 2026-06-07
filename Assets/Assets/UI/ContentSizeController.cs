using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ContentSizeController : MonoBehaviour
{
    public bool IsAuto;
    public ContentType type;
    public int SizeX;
    public int SizeY;
    public int Spacing = 10;
    public int ElementWidth = 100;
    public int LeftPadding = 10;
    public int RightPadding = 10;

    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        UpdateSize();
        AdjustContentPositions();
    }

    void OnTransformChildrenChanged()
    {
        UpdateSize();
        
    }

    private void FixedUpdate()
    {
        AdjustContentPositions();
    }

    public void UpdateSize()
    {
        if (!IsAuto) return;

        switch (type)
        {
            case ContentType.MinMaxRollsUIHorizontal:
                int childCount = transform.childCount;
                float newWidth = childCount * ElementWidth + (childCount -1) * Spacing + LeftPadding + RightPadding;

                rect.sizeDelta = new Vector2(newWidth, 0);
                break;
            default:
                Debug.Log("NoContentType or unknown type");
                break;
        }
    }

    public void AdjustContentPositions()
    {
        if (!IsAuto) return;

        switch (type)
        {
            case ContentType.MinMaxRollsUIHorizontal:
                
                for (int i = 0; i < transform.childCount; i++)
                {
                    float pos = (i + 0.5f) * ElementWidth + (i + 1) * Spacing + LeftPadding;
                    transform.GetChild(i).localPosition = new Vector2(pos, -75);
                    
                }
                break;
            default:
                Debug.Log("NoContentType or unknown type");
                break;
        }
    }
}
public enum ContentType
{
    MinMaxRollsUIHorizontal
}