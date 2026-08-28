using System.Collections.Generic;
using UnityEngine;
using static InventoryManager;

public class LootDropAnimationWorld : MonoBehaviour
{
    [SerializeField] private GameObject lootWorldPrefab;
    [SerializeField] private float flyDuration = 1.2f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float totalLifetime = 2.5f;
    [SerializeField] private float spreadRadius = 2f;
    [SerializeField] private float flyHeight = 1f;
    [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private static LootDropAnimationWorld instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public static void ShowLootDrop(Vector3 worldPosition, List<Loot> lootItems)
    {
        if (instance == null) return;
        instance.AnimateLootDrop(worldPosition, lootItems);
    }

    private void AnimateLootDrop(Vector3 worldPosition, List<Loot> lootItems)
    {
        List<Loot> expandedLoot = new List<Loot>();
        foreach (var loot in lootItems)
        {
            for (int i = 0; i < loot.count; i++)
            {
                expandedLoot.Add(new Loot(loot.item, 1));
            }
        }

        foreach (var loot in expandedLoot)
        {
            CreateFlyingWorldObject(worldPosition, loot);
        }
    }

    private void CreateFlyingWorldObject(Vector3 startPosition, Loot loot)
    {
        GameObject obj = Instantiate(lootWorldPrefab, startPosition, Quaternion.identity);
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        ItemSO itemData = InventoryManager.GetItemData(loot.item);
        if (itemData != null && itemData.icon != null)
        {
            Texture2D texture = itemData.icon as Texture2D;
            if (texture != null)
            {
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
                renderer.sprite = sprite;
            }
        }

        Vector3 direction = Random.insideUnitSphere;
        direction.y = Mathf.Abs(direction.y);
        direction.Normalize();

        float distance = Random.Range(spreadRadius * 0.5f, spreadRadius);
        Vector3 endPosition = startPosition + direction * distance;
        endPosition.y += flyHeight;

        float delay = Random.Range(0f, 0.3f);

        LeanTween.move(obj, endPosition, flyDuration)
            .setDelay(delay)
            .setEase(movementCurve);

        LeanTween.rotate(obj, new Vector3(0, 0, Random.Range(0, 360)), flyDuration)
            .setDelay(delay)
            .setEase(LeanTweenType.easeInOutQuad);

        LeanTween.alpha(obj.GetComponent<Renderer>().gameObject, 0f, fadeDuration)
            .setDelay(delay + flyDuration + (totalLifetime - flyDuration))
            .setOnComplete(() => Destroy(obj));
    }
}