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

    void Awake()
    {
        InitializePaths();
        InitialiveItems();
        foreach (var pos in ItemSpawnPoints)
        {
            var obj = Instantiate(ItemPrefab, pos);
            ItemUIs.Add(obj);
        }
        ItemPrefab.SetActive(false);
    }

    private void Start()
    {

    }

    void InitialiveItems()
    {
        foreach (var item in allItems)
        {
            if (!itemDatabase.ContainsKey(item.itemType))
            {
                itemDatabase[item.itemType] = item;
            }
        }
    }

    void AssignValueItems()
    {
        int i = 0;
        foreach (var item in inventory)
        {
            var obj = ItemUIs[i].GetComponent<ItemUIScript>();
            obj.SetName(item.Value.name);
            obj.SetCount(item.Value.count);
            obj.SetTexture(itemDatabase[item.Key].icon);
            obj.item = item.Key;
            i++;
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
        LoadInventory();
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
                Debug.Log($"Инвентарь загружен из: {SavePath}");
                return;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка загрузки из {SavePath}: {e.Message}");
            }
        }

        if (File.Exists(DefaultPath))
        {
            try
            {
                string json = File.ReadAllText(DefaultPath);
                LoadFromJson(json);
                //Debug.Log($"Инвентарь загружен из StreamingAssets: {DefaultPath}");
                SaveInventory();
                return;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка загрузки из StreamingAssets: {e.Message}");
            }
        }

        Debug.Log("Файл инвентаря не найден. Создаём пустой инвентарь.");
    }

    private static void LoadFromJson(string json)
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
        }
    }

    public static void SaveInventory()
    {
        try
        {
            List<ItemDTO> itemsList = new List<ItemDTO>();

            foreach (var kvp in inventory)
            {
                if (kvp.Value.count > 0)
                {
                    itemsList.Add(new ItemDTO
                    {
                        item = kvp.Key.ToString(),
                        name = kvp.Value.name,
                        description = kvp.Value.description,
                        count = kvp.Value.count
                    });
                }
            }

            var wrapper = new { inventory = itemsList };
            string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
            File.WriteAllText(SavePath, json);
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
    public static ItemData GetItemData(Item itemType)
    {
        if (inventory.ContainsKey(itemType))
        {
            return inventory[itemType];
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
}