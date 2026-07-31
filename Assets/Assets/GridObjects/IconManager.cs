using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    public static Dictionary<int, Sprite> PlayerIcons = new Dictionary<int, Sprite>();
    public static Dictionary<int, Sprite> EnemyIcons = new Dictionary<int, Sprite>();

    void Awake()
    {
        Init();
    }

    void Update()
    {

    }

    public static void Init()
    {
        string iconsPath = Path.Combine(Application.streamingAssetsPath, "character_icons/icons.json");
        string json;

        #if UNITY_ANDROID && !UNITY_EDITOR
                    json = ReadFileFromStreamingAssets(iconsPath);
        #else
                json = File.ReadAllText(iconsPath);
        #endif

        JObject iconsData = JObject.Parse(json);

        JArray playerIconsArray = iconsData["player_icons"] as JArray;
        if (playerIconsArray != null)
        {
            foreach (JToken obj in playerIconsArray)
            {
                int id = obj["id"].Value<int>();
                string fileName = obj["file"].Value<string>();
                Sprite sprite = LoadSprite(fileName);
                if (sprite != null)
                {
                    PlayerIcons[id] = sprite;
                }
            }
        }

        JArray enemyIconsArray = iconsData["enemy_icons"] as JArray;
        if (enemyIconsArray != null)
        {
            foreach (JToken obj in enemyIconsArray)
            {
                int id = obj["id"].Value<int>();
                string fileName = obj["file"].Value<string>();
                Sprite sprite = LoadSprite(fileName);
                if (sprite != null)
                {
                    EnemyIcons[id] = sprite;
                }
            }
        }
    }

    private static Sprite LoadSprite(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "character_icons", fileName);

        #if UNITY_ANDROID && !UNITY_EDITOR
                    byte[] imageData = ReadFileBytesFromStreamingAssets(filePath);
        #else
                byte[] imageData = File.ReadAllBytes(filePath);
        #endif

        if (imageData != null && imageData.Length > 0)
        {
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageData))
            {
                // Создаем Sprite из Texture2D
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f),
                    100f
                );
                return sprite;
            }
        }
        return null;
    }

    #if UNITY_ANDROID && !UNITY_EDITOR
        private static string ReadFileFromStreamingAssets(string path)
        {
            using (var streamReader = new StreamReader(path))
            {
                return streamReader.ReadToEnd();
            }
        }

        private static byte[] ReadFileBytesFromStreamingAssets(string path)
        {
            using (var fileStream = File.OpenRead(path))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
    #endif
}