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
        if (!File.Exists(path))
        {
            Debug.Log("Wrong path");
            profile_picture = DeffaultImage;
            return;
        }
        try
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            profile_picture = texture;
            Debug.Log("good");
        }
        catch (System.Exception)
        {
            Debug.Log("Wrong");
            profile_picture = DeffaultImage;
        }
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
            //Debug.Log($"Данные сохранены в: {SavePath}");
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
                    //Debug.Log($"Профиль загружен из: {SavePath}");
                    profile_picture = DeffaultImage;
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
                    //Debug.Log($"Профиль загружен из StreamingAssets: {DefaultPath}");
                    SaveProfile();
                    return;
                }
            }
            catch (System.Exception e)
            {
                profile_picture = DeffaultImage;
                Debug.LogError($"Ошибка загрузки из StreamingAssets: {e.Message}");
            }
        }
        profile = new Profile();
        LoadImage(profile.profile_picture_path);
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
    public List<int> CharacterIds;
    public Dictionary<int, int> PullCounters;
    public Dictionary<Item, int> ItemCounters;

    public Profile()
    {
        name = "Player";
        music_level = 1.0f;
        sound_level = 1.0f;
        budget = 0;
        max_difficulty = 1;
        CharacterIds = new List<int>();
        PullCounters = new Dictionary<int, int>();
        ItemCounters = new Dictionary<Item, int>();
        profile_picture_path = "";

    }
}

public enum Item
{
    xp_ticket,
    iron,
    gold,
    titan,
};