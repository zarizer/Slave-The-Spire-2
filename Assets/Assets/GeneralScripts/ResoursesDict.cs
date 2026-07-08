using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ResoursesDict : MonoBehaviour
{
    public static Dictionary<string, Texture> TextureSet = new Dictionary<string, Texture>();
    public static Dictionary<string, GameObject> ObjectSet = new Dictionary<string, GameObject>();
    public static Dictionary<string, AudioClip> SoundSet = new Dictionary<string, AudioClip>();
    public static List<GameObject> ModelSet = new List<GameObject>();

    public Texture none_element;
    public Texture fire_element;
    public Texture water_element;
    public Texture dendro_element;
    public Texture light_element;
    public Texture darkness_element;

    public Texture st_line;
    public Texture dg_line;
    public Texture radius;
    public Texture any;
    public Texture custom;
    public Texture single;
    public Texture field;
    public Texture target_radius;
    public Texture player_radius;

    public Texture atk_icon;
    public Texture def_icon;
    public Texture evade_icon;

    public GameObject UICanvas;
    public GameObject battle_main;
    public GameObject character_tab;
    public GameObject SoundMain;
    public GameObject profileManager;

    public AudioClip restricted_sound;

    public GameObject battleField;
    public GameObject level1_1;

    public GameObject ModelSimpleStone;
    public GameObject ModelSpawnPoint;
    public void Awake()
    {
        TextureSet["atk_icon"] = atk_icon;
        TextureSet["def_icon"] = def_icon;
        TextureSet["evade_icon"] = evade_icon;

        TextureSet["NoneElement"] = none_element;
        TextureSet["FireElement"] = fire_element;
        TextureSet["WaterElement"] = water_element;
        TextureSet["DendroElement"] = dendro_element;
        TextureSet["LightElement"] = light_element;
        TextureSet["DarknessElement"] = darkness_element;

        TextureSet["st_line"] = st_line;
        TextureSet["dg_line"] = dg_line;
        TextureSet["radius"] = radius;
        TextureSet["any"] = any;
        TextureSet["custom"] = custom;
        TextureSet["single"] = single;
        TextureSet["field"] = field;
        TextureSet["target_radius"] = target_radius;
        TextureSet["player_radius"] = player_radius;

        ObjectSet["UICanvas"] = UICanvas;
        ObjectSet["BattleMain"] = battle_main;
        ObjectSet["CharacterTab"] = character_tab;
        ObjectSet["SoundMain"] = SoundMain;
        ObjectSet["ProfileManager"] = profileManager;

        SoundSet["RestrictSound"] = restricted_sound;

        ObjectSet["Field"] = battleField;
        ObjectSet["Level1.1"] = level1_1;

        ModelSet.Add(ModelSimpleStone);
        ModelSet.Add(ModelSpawnPoint);
    }


    void Update()
    {
        
    }
    
    public static T GetClass<T>()
    {
        T obj;
        foreach (var i in ObjectSet)
        {
            if (i.Value.TryGetComponent<T>(out obj))
            {
                return obj;
            }
        }
        return (T)(object)null;
    }
}
