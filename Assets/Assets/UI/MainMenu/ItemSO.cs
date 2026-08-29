using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public Item itemType;
    public string itemName;

    [TextArea(3, 5)]
    public string description;

    public GameObject prefab;
    public Texture icon;

    public int maxStack = 99;
}
