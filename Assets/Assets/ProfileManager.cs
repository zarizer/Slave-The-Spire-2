using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    static public Profile profile;

    private static string SavePath;
    private static string DefaultPath;
    public static Texture profile_picture;
    public static Texture DeffaultImage;

    void Awake()
    {
        InitializePaths();
    }

    void Start()
    {
        DeffaultImage = GetComponent<RawImage>().texture;
        LoadProfile();
    }

    static public void LoadImage(string path)
    {
        path = CleanPath(path);

        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            //Debug.Log($"Путь к изображению не найден или не существует: {path}");
            profile_picture = DeffaultImage;
            return;
        }

        try
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            profile_picture = texture;
            //Debug.Log($"Изображение успешно загружено: {path}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка загрузки изображения: {e.Message}");
            profile_picture = DeffaultImage;
        }
    }
    private static string CleanPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return path;
        char[] invalidChars = new char[]
        {
            '\u200B',
            '\u200C',
            '\u200D',
            '\uFEFF',
            '\u00A0',
        };

        foreach (char c in invalidChars)
        {
            path = path.Replace(c.ToString(), "");
        }
        return path.Trim();
    }

    private static void InitializePaths()
    {
        SavePath = Path.Combine(Application.persistentDataPath, "profile", "profile.json");
        DefaultPath = Path.Combine(Application.streamingAssetsPath, "profile", "profile.json");
        string directory = Path.GetDirectoryName(SavePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
            Debug.Log($"Создана папка для сохранений: {directory}");
        }
    }

    [ContextMenu("SaveProfile")]
    public void SaveProfileMenu()
    {
        SaveProfile();
    }

    public static int GetLevelById(int id)
    {
        for(int i = 0; i < profile.CharacterIds.Count; i++)
        {
            if (id == profile.CharacterIds[i])
            {
                return profile.CharacterLevels[i];
            }
        }
        return 1;
    }
    static public void SaveProfile()
    {
        if (profile == null)
        {
            Debug.LogWarning("Профиль равен null. Создаём новый.");
            profile = new Profile();
        }
        try
        {
            string json = JsonConvert.SerializeObject(profile, Formatting.Indented);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Данные сохранены в: {SavePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Ошибка сохранения: {e.Message}");
        }
    }

    static public void LoadProfile()
    {
        if (File.Exists(SavePath))
        {
            try
            {
                string json = File.ReadAllText(SavePath);
                profile = JsonConvert.DeserializeObject<Profile>(json);

                if (profile != null)
                {
                    Debug.Log($"Профиль загружен из: {SavePath}");

                    if (!string.IsNullOrEmpty(profile.profile_picture_path))
                    {
                        profile.profile_picture_path = CleanPath(profile.profile_picture_path);
                    }

                    if (!string.IsNullOrEmpty(profile.profile_picture_path))
                    {
                        LoadImage(profile.profile_picture_path);
                    }
                    else
                    {
                        profile_picture = DeffaultImage;
                    }
                    return;
                }
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
                profile = JsonConvert.DeserializeObject<Profile>(json);

                if (profile != null)
                {
                    Debug.Log($"Профиль загружен из StreamingAssets: {DefaultPath}");
                    if (!string.IsNullOrEmpty(profile.profile_picture_path))
                    {
                        profile.profile_picture_path = CleanPath(profile.profile_picture_path);
                    }

                    SaveProfile();
                    return;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка загрузки из StreamingAssets: {e.Message}");
            }
        }
        profile = new Profile();
        profile_picture = DeffaultImage;
        SaveProfile();
    }

    static public bool ProfileExists()
    {
        return File.Exists(SavePath);
    }

    static public string GetSavePath()
    {
        return SavePath;
    }
    public static int GetJSONIdByCharacterId(int id)
    {
        for (int i = 0; i < ProfileManager.profile.CharacterIds.Count; i++)
        {
            if (id == ProfileManager.profile.CharacterIds[i]) return i;
        }
        return -1;
    }
}

[System.Serializable]
public class Profile
{
    public string name = "Player";
    public float music_level = 1.0f;
    public float sound_level = 1.0f;
    public int budget = 0;
    public int max_difficulty = 1;
    public string profile_picture_path;
    public List<int> CharacterLevels;
    public List<int> CharacterIds;
    public Dictionary<int, int> PullCounters;
    public float roll_speed = 1.2f;
    public int graphics_settings = 1;
    public int CurrentLevelId = 0;
    public int level_num;
    public int compaign_num;
    public int chapter_num;
    public bool is_in_game;

    public Profile()
    {
        name = "Player";
        music_level = 1.0f;
        sound_level = 1.0f;
        budget = 0;
        max_difficulty = 1;
        CharacterLevels = new List<int>();
        CharacterIds = new List<int>();
        PullCounters = new Dictionary<int, int>();
        profile_picture_path = "";
        roll_speed = 1.2f;
        graphics_settings = 3;
        CurrentLevelId = 0;
        level_num = 0;
        compaign_num = 0;
        chapter_num = 0;
        is_in_game = false;
    }


}
