using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LevelData
{
    public int DebugSaveId;
    public int x_;
    public int y_;
    public int minLevel;
    public int maxLevel;

    public Dictionary<(int x, int y), LevelObject> Objects =
        new Dictionary<(int x, int y), LevelObject>();

    private static Dictionary<int, LevelData> Levels = new Dictionary<int, LevelData>();
    private static string JsonPath;

    public static Dictionary<int, Dictionary<int, List<int>>> LevelAmounts = new Dictionary<int, Dictionary<int, List<int>>>();

    private static void InitializePath()
    {
        JsonPath = Path.Combine(Application.streamingAssetsPath, "levels/levels.json");
    }

    public static void SaveLevel(LevelData levelData)
    {
#if UNITY_EDITOR
        InitializePath();
        AddObjectToJson(levelData.DebugSaveId, levelData);
#else
        // В билде только чтение
#endif
    }

    public static LevelData GetLevelData(int id)
    {
        if (Levels.ContainsKey(id))
            return Levels[id];
        else
        {
            Debug.LogError($"Уровень с ID {id} не найден.");
            return null;
        }
    }

    public static void Init()
    {
        InitializePath();
        LoadLevels();
        UpdateLevelAmounts();
    }

    private static void LoadLevels()
    {
        string json = ReadJsonFile();

        if (string.IsNullOrWhiteSpace(json))
        {
            Debug.Log($"Файл уровней пуст или не найден по пути: {JsonPath}. Создаём пустой словарь.");
            Levels = new Dictionary<int, LevelData>();
            return;
        }

        try
        {
            JObject jsonObject = JObject.Parse(json);
            Levels.Clear();

            foreach (var property in jsonObject.Properties())
            {
                int id = int.Parse(property.Name);

                LevelDataSerializable dto = JsonConvert.DeserializeObject<LevelDataSerializable>(property.Value.ToString());

                if (dto != null)
                {
                    LevelData level = dto.ToLevelData();
                    if (level != null)
                    {
                        Levels[id] = level;
                    }
                    else
                    {
                        Debug.LogWarning($"Уровень с ID {id} не может быть загружен (данные null)");
                    }
                }
                else
                {
                    Debug.LogWarning($"DTO для уровня {id} равен null");
                }
            }
        }
        catch (JsonReaderException e)
        {
            Debug.LogError($"Ошибка парсинга JSON: {e.Message}. Файл повреждён.");
            Levels = new Dictionary<int, LevelData>();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка загрузки уровней: {e.Message}");
            Levels = new Dictionary<int, LevelData>();
        }
    }

    private static string ReadJsonFile()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return ReadFileFromStreamingAssets(JsonPath);
#else
        if (File.Exists(JsonPath))
        {
            return File.ReadAllText(JsonPath);
        }
        else
        {
            Debug.LogWarning($"Файл не найден по пути: {JsonPath}");
            return null;
        }
#endif
    }

    private static void AddObjectToJson(int key, LevelData obj)
    {
#if UNITY_EDITOR
        string directory = Path.GetDirectoryName(JsonPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Debug.Log($"Создана директория: {directory}");
        }

        JObject jsonObject;

        if (File.Exists(JsonPath))
        {
            string existingJson = File.ReadAllText(JsonPath);

            if (!string.IsNullOrWhiteSpace(existingJson))
            {
                try
                {
                    jsonObject = JObject.Parse(existingJson);
                }
                catch (JsonReaderException)
                {
                    Debug.LogWarning($"Файл {JsonPath} повреждён. Создал новый.");
                    jsonObject = new JObject();
                }
            }
            else
            {
                Debug.Log($"Файл {JsonPath} пуст. Создал новый.");
                jsonObject = new JObject();
            }
        }
        else
        {
            jsonObject = new JObject();
        }

        LevelDataSerializable dto = new LevelDataSerializable(obj);
        string newObjJson = JsonConvert.SerializeObject(dto, Formatting.Indented);
        JToken newObjToken = JToken.Parse(newObjJson);

        jsonObject[key.ToString()] = newObjToken;

        File.WriteAllText(JsonPath, jsonObject.ToString(Formatting.Indented));

        if (Levels.ContainsKey(key))
            Levels[key] = obj;
        else
            Levels.Add(key, obj);
#endif
    }

    private static void UpdateLevelAmounts()
    {
        LevelAmounts.Clear();

        foreach (var levelEntry in Levels)
        {
            int levelId = levelEntry.Key;
            string idString = levelId.ToString();
            if (idString.Length < 9)
            {
                continue;
            }
            string companyStr = idString.Substring(2, 2);
            string chapterStr = idString.Substring(4, 2);

            if (!int.TryParse(companyStr, out int companyId))
            {
                Debug.LogWarning($"Не удалось распарсить компанию из ID {levelId}");
                continue;
            }

            if (!int.TryParse(chapterStr, out int chapterId))
            {
                Debug.LogWarning($"Не удалось распарсить главу из ID {levelId}");
                continue;
            }
            if (!LevelAmounts.ContainsKey(companyId))
            {
                LevelAmounts[companyId] = new Dictionary<int, List<int>>();
            }

            if (!LevelAmounts[companyId].ContainsKey(chapterId))
            {
                LevelAmounts[companyId][chapterId] = new List<int>();
            }

            LevelAmounts[companyId][chapterId].Add(levelId);
        }
        foreach (var companyEntry in LevelAmounts)
        {
            foreach (var chapterEntry in companyEntry.Value)
            {
                chapterEntry.Value.Sort();
            }
        }

        Debug.Log($"LevelAmounts обновлен. Компаний: {LevelAmounts.Count}");
    }

    [System.Serializable]
    private class LevelDataSerializable
    {
        public int Id;
        public int x_;
        public int y_;
        public int minLevel;
        public int maxLevel;
        public List<ObjectData> Objects = new List<ObjectData>();

        public LevelDataSerializable(LevelData level_data)
        {
            if (level_data == null)
            {
                //Debug.LogError("LevelData is null in LevelDataSerializable constructor");
                return;
            }

            this.Id = level_data.DebugSaveId;
            this.x_ = level_data.x_;
            this.y_ = level_data.y_;
            this.minLevel = level_data.minLevel;
            this.maxLevel = level_data.maxLevel;

            if (level_data.Objects != null)
            {
                foreach (var kvp in level_data.Objects)
                {
                    if (kvp.Value != null)
                    {
                        Objects.Add(new ObjectData
                        {
                            posX = kvp.Key.x,
                            posY = kvp.Key.y,
                            objType = kvp.Value.type ?? "Obstacle",
                            id = kvp.Value.id,
                            level = kvp.Value.level,
                            isCustomLevel = kvp.Value.isCustomLevel,
                            specialValue = kvp.Value.specialValue,

                        });
                    }
                    else
                    {
                        Debug.LogWarning($"Объект на позиции ({kvp.Key.x}, {kvp.Key.y}) равен null");
                    }
                }
            }
        }

        public LevelData ToLevelData()
        {
            LevelData level = new LevelData();
            level.DebugSaveId = this.Id;
            level.x_ = this.x_;
            level.y_ = this.y_;
            level.minLevel = this.minLevel;
            level.maxLevel = this.maxLevel;
            level.Objects = new Dictionary<(int, int), LevelObject>();

            if (this.Objects != null)
            {
                foreach (var obj in this.Objects)
                {
                    if (obj != null)
                    {
                        level.Objects[(obj.posX, obj.posY)] = new LevelObject
                        {
                            type = obj.objType ?? "Obstacle",
                            id = obj.id,
                            level = obj.level,
                            isCustomLevel = obj.isCustomLevel,
                            specialValue = obj.specialValue,
                        };
                    }
                }
            }

            return level;
        }
    }

    [System.Serializable]
    private class ObjectData
    {
        public int posX;
        public int posY;
        public string objType;
        public int id;
        public int level;
        public bool isCustomLevel;
        public int specialValue;
    }
}

[System.Serializable]
public class LevelObject
{
    public string type;
    public int id;
    public int level;
    public bool isCustomLevel;
    public int specialValue;

    public LevelObject()
    {
        type = "Obstacle";
        id = 0;
        level = 0;
        isCustomLevel = false;
    }

    public LevelObject(string type, int id, int level = 0, bool isCustomLevel = false, int specialValue = 0)
    {
        this.type = type ?? "Obstacle";
        this.id = id;
        this.level = level;
        this.isCustomLevel = isCustomLevel;
        this.specialValue = specialValue;
    }
}

[System.Serializable]
public class EnemyData
{
    public int id;
    public string name;
    public int baseHealth;
    public int baseDamage;
    public int defaultLevel;
    public int specialValue;
}

[System.Serializable]
public class CharacterData
{
    public int id;
    public string name;
    public int baseHealth;
    public int baseDamage;
    public int defaultLevel;
    public int specialValue;
}

[System.Serializable]
public class ObstacleData
{
    public int id;
    public string name;
    public int health;
    public bool isDestructible;
    public int specialValue;
}