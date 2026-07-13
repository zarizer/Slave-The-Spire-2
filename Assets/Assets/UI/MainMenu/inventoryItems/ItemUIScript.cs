using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIScript : MonoBehaviour
{
    public Item item;
    public TextMeshProUGUI name_text;
    public TextMeshProUGUI count;
    public RawImage image;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void SetName(string Name)
    {
        name_text.text = Name;
    }

    public void SetCount(int Count)
    {
        count.text = Count.ToString();
    }

    public void SetTexture(Texture texture)
    {
        image.texture = texture;
    }
}
