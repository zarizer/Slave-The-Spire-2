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

    // Объекты уровня: позиция -> (тип, id, уровень)
    // Уровень = 0 означает "не кастомный" (используется общий уровень уровня)
    public Dictionary<(int x, int y), LevelObject> Objects =
        new Dictionary<(int x, int y), LevelObject>();

    private static Dictionary<int, LevelData> Levels = new Dictionary<int, LevelData>();
    private static string JsonPath;

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

    public LevelData GetLevelData(int id)
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
                LevelData level = dto.ToLevelData();
                Levels[id] = level;
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

#if UNITY_ANDROID && !UNITY_EDITOR
    private static string ReadFileFromStreamingAssets(string path)
    {
        UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(path);
        www.SendWebRequest();
        while (!www.isDone) { }
        
        if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            return www.downloadHandler.text;
        }
        else
        {
            Debug.LogError($"Ошибка чтения файла: {www.error}");
            return null;
        }
    }
#endif

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
                    Debug.LogWarning($"Файл {JsonPath} повреждён. Создаём новый.");
                    jsonObject = new JObject();
                }
            }
            else
            {
                Debug.Log($"Файл {JsonPath} пуст. Создаём новый.");
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

    // ================ КЛАССЫ ДЛЯ СЕРИАЛИЗАЦИИ ================

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
            this.Id = level_data.DebugSaveId;
            this.x_ = level_data.x_;
            this.y_ = level_data.y_;
            this.minLevel = level_data.minLevel;
            this.maxLevel = level_data.maxLevel;

            foreach (var kvp in level_data.Objects)
            {
                Objects.Add(new ObjectData
                {
                    posX = kvp.Key.x,
                    posY = kvp.Key.y,
                    objType = kvp.Value.type,
                    id = kvp.Value.id,
                    level = kvp.Value.level,
                    isCustomLevel = kvp.Value.isCustomLevel
                });
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

            foreach (var obj in Objects)
            {
                level.Objects[(obj.posX, obj.posY)] = new LevelObject
                {
                    type = obj.objType,
                    id = obj.id,
                    level = obj.level,
                    isCustomLevel = obj.isCustomLevel
                };
            }

            return level;
        }
    }

    [System.Serializable]
    private class ObjectData
    {
        public int posX;
        public int posY;
        public string objType; // "Enemy", "Character", "Obstacle"
        public int id;
        public int level; // Уровень объекта (0 если не кастомный)
        public bool isCustomLevel; // Флаг кастомного уровня
    }
}

// ================ ОТДЕЛЬНЫЙ КЛАСС ДЛЯ ОБЪЕКТА НА УРОВНЕ ================

[System.Serializable]
public class LevelObject
{
    public string type; // "Enemy", "Character", "Obstacle"
    public int id;
    public int level; // Уровень объекта (0 если не кастомный)
    public bool isCustomLevel; // true - используется свой уровень, false - уровень уровня

    public LevelObject()
    {
        type = "Obstacle";
        id = 0;
        level = 0;
        isCustomLevel = false;
    }

    public LevelObject(string type, int id, int level = 0, bool isCustomLevel = false)
    {
        this.type = type;
        this.id = id;
        this.level = level;
        this.isCustomLevel = isCustomLevel;
    }
}

// ================ ОТДЕЛЬНЫЕ КЛАССЫ ДЛЯ ХРАНЕНИЯ ДАННЫХ ================

[System.Serializable]
public class EnemyData
{
    public int id;
    public string name;
    public int baseHealth;
    public int baseDamage;
    public int defaultLevel; // Базовый уровень для врага
}

[System.Serializable]
public class CharacterData
{
    public int id;
    public string name;
    public int baseHealth;
    public int baseDamage;
    public int defaultLevel; // Базовый уровень для персонажа
}

[System.Serializable]
public class ObstacleData
{
    public int id;
    public string name;
    public int health;
    public bool isDestructible;
}