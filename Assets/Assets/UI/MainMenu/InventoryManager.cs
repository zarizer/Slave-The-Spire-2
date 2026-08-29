using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using Unity.Properties;

public class InventoryManager : MonoBehaviour
{
    public static Dictionary<Item, ItemData> inventory = new Dictionary<Item, ItemData>();

    private static string SavePath;
    private static string DefaultPath;

    public static Dictionary<Item, ItemSO> itemDatabase = new Dictionary<Item, ItemSO>();

    [SerializeField] private List<ItemSO> allItems;
    [SerializeField] private List<Transform> ItemSpawnPoints;
    private List<GameObject> ItemUIs = new List<GameObject>();
    [SerializeField] private GameObject ItemPrefab;

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    public void Init()
    {
        InitializePaths();
        InitialiveItems();
    }


    void Awake()
    {
        InitializePaths();
        InitialiveItems();

        if (ItemSpawnPoints != null && ItemSpawnPoints.Count > 0)
        {
            foreach (var pos in ItemSpawnPoints)
            {
                if (pos != null)
                {
                    var obj = Instantiate(ItemPrefab, pos);
                    ItemUIs.Add(obj);
                }
            }
        }

        if (ItemPrefab != null)
            ItemPrefab.SetActive(false);
    }

    private void Start()
    {
        LoadInventory();
        AssignValueItems();
    }

    private void InitialiveItems()
    {
        if (allItems == null)
        {
            Debug.LogError("[InventoryManager] allItems == NULL!");
            return;
        }

        Debug.Log($"[InventoryManager] Initializing {allItems.Count} items");

        itemDatabase.Clear();

        for (int i = 0; i < allItems.Count; i++)
        {
            ItemSO item = allItems[i];

            if (item == null)
            {
                Debug.LogError(
                    $"[InventoryManager] allItems[{i}] == NULL!"
                );
                continue;
            }

            Debug.Log(
                $"[InventoryManager] Registering: {item.itemType} / {item.itemName}"
            );

            if (itemDatabase.ContainsKey(item.itemType))
            {
                Debug.LogWarning(
                    $"[InventoryManager] Duplicate ItemType: {item.itemType}"
                );
                continue;
            }

            itemDatabase.Add(item.itemType, item);
        }

        Debug.Log(
            $"[InventoryManager] Database initialized. Count = {itemDatabase.Count}"
        );
    }


    void AssignValueItems()
    {
        int i = 0;

        foreach (var item in inventory)
        {
            if (i < ItemUIs.Count)
            {
                var obj = ItemUIs[i].GetComponent<ItemUIScript>();

                if (obj != null)
                {
                    obj.SetName(item.Value.name);
                    obj.SetCount(item.Value.count);
                    obj.item = item.Key;

                    if (itemDatabase.TryGetValue(item.Key, out ItemSO itemSO))
                    {
                        Debug.Log($"ITEM: {item.Key} | ICON: {itemSO.icon}");

                        if (itemSO.icon != null)
                        {
                            obj.SetTexture(itemSO.icon);
                        }
                        else
                        {
                            Debug.LogError($"У ItemSO '{item.Key}' НЕ назначена icon!");
                        }
                    }
                    else
                    {
                        Debug.LogError($"ItemSO для '{item.Key}' не найден в itemDatabase!");
                    }

                    if (obj.image == null)
                    {
                        Debug.LogError($"RawImage 'image' не назначен в ItemUIScript на UI #{i}!");
                    }
                }

                i++;
            }
        }

        itemNameText.text = "";
        itemDescriptionText.text = "";
    }


    public static GameObject GetItemPrefab(Item itemType)
    {
        if (itemDatabase.ContainsKey(itemType))
        {
            return itemDatabase[itemType].prefab;
        }
        return null;
    }

    void OnEnable()
    {
        AssignValueItems();
    }

    [ContextMenu("Copy Inventory File to Desktop")]
    public void CopyInventoryToDesktop()
    {
        CopyInventoryToDesktopStatic();
    }

    public static void CopyInventoryToDesktopStatic()
    {
        try
        {
            InitializePaths();

            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

            string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string fileName = $"inventory_backup_{timestamp}.json";
            string destinationPath = Path.Combine(desktopPath, fileName);

            if (!File.Exists(SavePath))
            {
                Debug.LogError($"Файл инвентаря не найден по пути: {SavePath}");

                if (File.Exists(DefaultPath))
                {
                    Debug.Log($"Копирую из StreamingAssets: {DefaultPath}");
                    File.Copy(DefaultPath, destinationPath, overwrite: true);
                    Debug.Log($"Файл скопирован из StreamingAssets на рабочий стол: {destinationPath}");
                }
                else
                {
                    Debug.LogError("Файл инвентаря не найден ни в AppData, ни в StreamingAssets");
                }
                return;
            }

            File.Copy(SavePath, destinationPath, overwrite: true);
            Debug.Log($"Файл инвентаря успешно скопирован на рабочий стол: {destinationPath}");

#if UNITY_EDITOR
            UnityEditor.EditorUtility.RevealInFinder(destinationPath);
#endif
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка при копировании файла на рабочий стол: {e.Message}");
        }
    }

    public static void SaveInventoryToCustomPath(string directoryPath, string fileName = null)
    {
        try
        {
            InitializePaths();

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"inventory_backup_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
            }

            string destinationPath = Path.Combine(directoryPath, fileName);

            if (!File.Exists(SavePath))
            {
                Debug.LogError($"Файл инвентаря не найден по пути: {SavePath}");
                return;
            }

            string directory = Path.GetDirectoryName(destinationPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.Copy(SavePath, destinationPath, overwrite: true);
            Debug.Log($"Файл инвентаря сохранен по пути: {destinationPath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка при сохранении файла: {e.Message}");
        }
    }

    private static void InitializePaths()
    {
        SavePath = Path.Combine(Application.persistentDataPath, "inventory", "inventory.json");
        DefaultPath = Path.Combine(Application.streamingAssetsPath, "inventory", "inventory.json");

        string directory = Path.GetDirectoryName(SavePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Debug.Log($"Создана папка для инвентаря: {directory}");
        }
    }

    public static void LoadInventory()
    {
        inventory.Clear();

        if (File.Exists(SavePath))
        {
            try
            {
                string json = File.ReadAllText(SavePath);
                LoadFromJson(json);
                Debug.Log($"Инвентарь загружен из AppData: {SavePath}");
                return;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка загрузки из AppData: {e.Message}");
            }
        }

        if (File.Exists(DefaultPath))
        {
            try
            {
                string json = File.ReadAllText(DefaultPath);
                LoadFromJson(json);
                Debug.Log($"Инвентарь загружен из StreamingAssets: {DefaultPath}");
                SaveInventory();
                return;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка загрузки из StreamingAssets: {e.Message}");
            }
        }

        Debug.Log("Файл инвентаря не найден. Создаём пустой инвентарь.");
        SaveInventory();
    }

    private static void LoadFromJson(string json)
    {
        try
        {
            JObject jsonObject = JObject.Parse(json);
            JArray itemsArray = jsonObject["inventory"] as JArray;

            if (itemsArray == null)
            {
                Debug.LogWarning("В JSON нет поля 'inventory'");
                return;
            }

            foreach (JToken itemToken in itemsArray)
            {
                string itemType = itemToken["item"].ToString();
                string name = itemToken["name"].ToString();
                string description = itemToken["description"].ToString();
                int count = itemToken["count"].Value<int>();

                if (System.Enum.TryParse<Item>(itemType, true, out Item itemEnum))
                {
                    if (inventory.ContainsKey(itemEnum))
                    {
                        inventory[itemEnum].count += count;
                    }
                    else
                    {
                        inventory[itemEnum] = new ItemData
                        {
                            name = name,
                            description = description,
                            count = count
                        };
                    }
                }
                else
                {
                    Debug.LogWarning($"Неизвестный тип предмета: {itemType}");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка парсинга JSON инвентаря: {e.Message}");
        }
    }

    public static void SaveInventory()
    {
        try
        {
            List<ItemDTO> itemsList = new List<ItemDTO>();
            foreach (var itemType in itemDatabase.Keys)
            {
                int count = 0;
                string name = "";
                string description = "";

                if (inventory.ContainsKey(itemType))
                {
                    count = inventory[itemType].count;
                    name = inventory[itemType].name;
                    description = inventory[itemType].description;
                }
                else
                {
                    var itemSO = itemDatabase[itemType];
                    name = itemSO.itemName;
                    description = itemSO.description;
                    count = 0;
                }

                itemsList.Add(new ItemDTO
                {
                    item = itemType.ToString(),
                    name = name,
                    description = description,
                    count = count
                });
            }

            var wrapper = new { inventory = itemsList };
            string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Инвентарь сохранён в AppData: {SavePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка сохранения инвентаря: {e.Message}");
        }
    }

    public static void AddItem(Item itemType, string name, string description, int count = 1)
    {
        if (inventory.ContainsKey(itemType))
        {
            inventory[itemType].count += count;
        }
        else
        {
            inventory[itemType] = new ItemData
            {
                name = name,
                description = description,
                count = count
            };
        }

        SaveInventory();
    }

    public static bool RemoveItem(Item itemType, int count = 1)
    {
        if (!inventory.ContainsKey(itemType))
        {
            Debug.LogWarning($"Предмет {itemType} не найден");
            return false;
        }

        if (inventory[itemType].count < count)
        {
            return false;
        }

        inventory[itemType].count -= count;

        if (inventory[itemType].count <= 0)
        {
            inventory.Remove(itemType);
        }

        SaveInventory();
        return true;
    }
    public static int GetItemCount(Item itemType)
    {
        if (inventory.ContainsKey(itemType))
        {
            return inventory[itemType].count;
        }
        return 0;
    }

    public static string GetItemName(Item itemType)
    {
        if (inventory.ContainsKey(itemType))
        {
            return inventory[itemType].name;
        }
        return "Unknown";
    }

    public static string GetItemDescription(Item itemType)
    {
        if (inventory.ContainsKey(itemType))
        {
            return inventory[itemType].description;
        }
        return "";
    }
    public static ItemSO GetItemData(Item itemType)
    {
        if (itemDatabase.ContainsKey(itemType))
        {
            return itemDatabase[itemType];
        }
        return null;
    }
    public static bool HasItem(Item itemType, int count = 1)
    {
        return GetItemCount(itemType) >= count;
    }


    public static Dictionary<Item, ItemData> GetAllItems()
    {
        Dictionary<Item, ItemData> result = new Dictionary<Item, ItemData>();

        foreach (var kvp in inventory)
        {
            if (kvp.Value.count > 0)
            {
                result[kvp.Key] = kvp.Value;
            }
        }

        return result;
    }
    public static void ClearInventory()
    {
        inventory.Clear();
        SaveInventory();
    }

    public void ShowItemdata(ItemUIScript item)
    {
        itemNameText.text = inventory[item.item].name;
        itemDescriptionText.text = inventory[item.item].description;
    }

    [System.Serializable]
    private class ItemDTO
    {
        public string item;
        public string name;
        public string description;
        public int count;
    }

}

[System.Serializable]
public class ItemData
{
    public string name;
    public string description;
    public int count;
}

public enum Item
{
    xp_ticket,
    stick1,
    stick2,
    stick3,
    asphalt1,
    asphalt2,
    asphalt3,
    mat1,
    mat2,
    mat3,
    character_event,
    character,
}